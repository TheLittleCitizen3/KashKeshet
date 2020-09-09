using Common;
using KashServer.Clients;
using KashServer.requestHandler;
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
                Client client = new Client(tcpClient);
                ClientInfo clientInfo = client.ClientInfo;
                Clients.TryAdd(clientInfo, client);
                _logger.LogInformation("New Client Was Added!");
                //SendMessage.Send($"The user: {clientInfo.DisplayName} joinned", Clients.Values.ToList());
                Thread t = new Thread(HandleClient);
                t.Start(clientInfo);

            }
        }
        private void HandleClient(object obj)
        {
            ClientInfo clientInfo = (ClientInfo)obj;
            Client client;
            client = Clients[clientInfo];
            while (true)
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

                    Request request = Serializator.Deserialize<Request>(buffer);
                    //string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    //data = MessageFormatter.FormatMessage(data, client);
                    RequsetManager requestManager = new RequsetManager();
                    requestManager.HandleRequest(request, Clients);
                    //SendMessage.Send(data, Clients.Values.ToList());
                    _logger.LogInformation($"Client id:{clientInfo.UID} Write:{request.Content.ToString()}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("user disconnected with massage:" + ex.Message);
                    break;
                }
                
            }

            Clients.TryRemove(clientInfo, out _);
           
            //SendMessage.Send($"The user: {clientInfo.DisplayName} disconnected", Clients.Values.ToList());
            client.TcpClient.Client.Shutdown(SocketShutdown.Both);
            client.TcpClient.Close();
        }
    }
}

