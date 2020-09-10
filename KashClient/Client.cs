using Common;
using KashClient.RequestCreator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace KashClient
{
    class Client : IClient
    {
        private IPAddress ServerIp;
        private int ServerPort;
        private IDataHandler dataHandler;
        public TcpClient client { get; set; }
        public NetworkStream NetworkStream { get; set; }
        private Thread _thread;
        public ClientInfo ClientInfo { get; set; }
        

        private readonly object _lock = new object();
        public Client(IPAddress ip, int port)
        {
            ServerIp = ip;
            ServerPort = port;
            dataHandler = new DataHandler();
            client = new TcpClient();
        }

        private void Connect()
        {
            try
            {
                client.Connect(ServerIp, ServerPort);
                Console.WriteLine("Client Connected to Server");
            }
            catch (Exception ex)
            {
                Console.WriteLine("could not connect server:" + ex.Message);
                throw;
            }
        }
        public void Start()
        {
            Connect();
            NetworkStream = client.GetStream();
            ClientInfo = LoginClient(client);
            Console.WriteLine($"You are now logged in as: {ClientInfo.DisplayName}");
            _thread = new Thread(o => dataHandler.Recivedata((TcpClient)o));
            _thread.Start(client);
        }
        public string GetUserInput()
        {            
            Console.Write(">");
            string message = Console.ReadLine();
            return message;
        }
        public void SendRequest(RequestType requestType, object obj)
        {
            IRequestCreation requestCreator = new RequestCreator.RequestCreator();
            Common.Request request = requestCreator.Create(ClientInfo, requestType, obj);
            byte[] buffer = Serializator.Serialize(request);
            NetworkStream.Write(buffer, 0, buffer.Length); ;
        }
        public void Stop()
        {
            client.Client.Shutdown(SocketShutdown.Send);
            _thread.Join();
            NetworkStream.Close();
            client.Close();
            Console.WriteLine("Disconnected From Server");

        }
        private ClientInfo LoginClient(TcpClient tcpClient)
        {
            Response response;
            do
            {
                Console.WriteLine("Enter your user name or create new");
                string userName = Console.ReadLine().ToLower();
                while (string.IsNullOrEmpty(userName))
                {
                    Console.WriteLine("Enter user name");
                    userName = Console.ReadLine().ToLower();
                }
                byte[] buffer = Encoding.ASCII.GetBytes(userName);
                NetworkStream.Write(buffer, 0, buffer.Length);
                NetworkStream stream = tcpClient.GetStream();
                byte[] ResponseBuffer = new byte[1024];
                int byte_count = stream.Read(ResponseBuffer, 0, ResponseBuffer.Length);
                response = (Response)Serializator.Deserialize(ResponseBuffer);
                if (response.ResponseType == ResponseType.ClientAllreadyExist)
                {
                    Console.WriteLine("User allready exist!");
                }
            } while (response.ResponseType == ResponseType.ClientAllreadyExist);
            return (ClientInfo)response.Content;
        }
    }
}
