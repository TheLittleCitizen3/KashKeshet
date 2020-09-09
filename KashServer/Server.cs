using Common;
using KashServer.Clients;
using KashServer.requestHandler;
using KashServer.responseHandler;
using KashServer.SendRecive;
using KashServer.Strings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KashServer
{
    public class Server : IServer
    {
        private readonly ILogger<Worker> _logger;
        private ConcurrentDictionary<ClientInfo, Client> Clients { get; set; }
        readonly object _lock = new object();
        IPAddress Ip;
        int Port;
        public Server(IPAddress ip, int port, ILogger<Worker> logger)
        {
            Clients = new ConcurrentDictionary<ClientInfo, Client>();
            Ip = ip;
            Port = port;
            _logger = logger;
        }
        public void StartServer()
        {
            TcpListener ServerSocket = new TcpListener(Ip, Port);
            ServerSocket.Start();
            while (true)
            {
                TcpClient tcpClient = ServerSocket.AcceptTcpClient();
                _logger.LogInformation("New Client Was Added!");
                //SendMessage.Send($"The user: {clientInfo.DisplayName} joinned", Clients.Values.ToList());
                Thread t = new Thread(HandleClient);
                t.Start(tcpClient);

            }
        }
        private void HandleClient(object tcpClientObject)
        {
            TcpClient tcpClient = (TcpClient)tcpClientObject;
            Client client;
            try
            {
                LoginClient(tcpClient);
            }
            catch (Exception ex)
            {
                _logger.LogError("Client disconnected Before Login compeletd with error: " + ex.Message);
            }
            finally
            {
                client = GetClientByTcpclient(tcpClient);
            }
            while (client != null)
            {
                try
                {
                    NetworkStream stream = client.TcpClient.GetStream();
                    byte[] buffer = new byte[1024];
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                    {
                        break;
                    }

                    Request request = (Request)Serializator.Deserialize(buffer);
                    //string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    //data = MessageFormatter.FormatMessage(data, client);
                    RequsetManager requestManager = new RequsetManager();
                    requestManager.HandleRequest(request, Clients);
                    //SendMessage.Send(data, Clients.Values.ToList());
                    _logger.LogInformation($"Client id:{client.ClientInfo.DisplayName} Write:{request.Content.ToString()}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("user disconnected with massage:" + ex.Message);
                    Clients[client.ClientInfo].ClientStatus = ClientStatus.Disconnected;
                    break;
                }

            }
            if (client!=null)
            {
                Clients[client.ClientInfo].ClientStatus = ClientStatus.Disconnected;
            }
            tcpClient.Client.Shutdown(SocketShutdown.Both);
            tcpClient.Close();
        }
        private void LoginClient(TcpClient tcpClient)
        {
            string userName = "";
            while (String.IsNullOrEmpty(userName))
            {
                NetworkStream stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];
                int byte_count = stream.Read(buffer, 0, buffer.Length);

                if (byte_count > 0)
                {
                    userName = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    if (IsUserExist(userName))
                    {
                        ClientInfo clientInfo = GetClientInfoByName(userName);
                        if (Clients[GetClientInfoByName(userName)].ClientStatus == ClientStatus.Disconnected)
                        {
                            ChangeExistingClientToConnected(clientInfo);
                            updateClientsTcpClient(clientInfo, tcpClient); 
                        }
                        else
                        {
                            userName = "";
                            SendLoginResponse(tcpClient, ResponseType.ClientAllreadyExist);
                        }
                    }
                    else
                    {
                        AddNewClient(tcpClient, userName);
                    }
                }
            }
            Client client = GetClientByTcpclient(tcpClient);
            List<Client> responseClients = new List<Client>() { client };
            IResponseAction responseAction = new ResponseHandler();
            Response response = responseAction.CreateResponse(ResponseType.ClientInfo, client.ClientInfo);
            byte[] serializedData = Serializator.Serialize(response);
            SendMessage.Send(serializedData, responseClients);
        }
        private bool IsUserExist(string userName)
        {
            var client = Clients.Select(c => c.Key)
                .Where(c => c.DisplayName == userName).ToList();

            return client.Count() > 0;
        }
        private ClientInfo GetClientInfoByName(string userName)
        {
            var client = Clients.Select(c => c.Key)
                .Where(c => c.DisplayName == userName).ToList();
            return client.FirstOrDefault();
        }
        private void ChangeExistingClientToConnected(ClientInfo clientInfo)
        {
            Clients[clientInfo].ClientStatus = ClientStatus.Connected;
            Clients[clientInfo].ClientInfo = clientInfo;
            return;
        }
        private void updateClientsTcpClient(ClientInfo clientInfo, TcpClient tcpClient)
        {
            Clients[clientInfo].TcpClient = tcpClient;
        }
        private void AddNewClient(TcpClient tcpClient, string userName)
        {
            Client client = new Client(tcpClient, userName);
            Clients.TryAdd(client.ClientInfo, client);
        }
        private Client GetClientByTcpclient(TcpClient tcpClient)
        {
            var client = Clients.FirstOrDefault(c => c.Value.TcpClient == tcpClient).Value;
            return client;
        }
        private void SendLoginResponse(TcpClient tcpClient, ResponseType responseType)
        {
            Client client;
            List<Client> responseClients = new List<Client>();
            IResponseAction responseAction = new ResponseHandler();
            Response response;
            if (responseType == ResponseType.ClientInfo)
            {
                client = GetClientByTcpclient(tcpClient);
                response = responseAction.CreateResponse(ResponseType.ClientInfo, client.ClientInfo);
            }
            else
            {
                response = responseAction.CreateResponse(ResponseType.ClientAllreadyExist, null);    
            }
            byte[] serializedData = Serializator.Serialize(response);
            NetworkStream stream = tcpClient.GetStream();

            stream.Write(serializedData, 0, serializedData.Length);

        }
    }
}

