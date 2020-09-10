using Common;
using KashClient.RequestHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
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
        TcpClient client;
        NetworkStream NetworkStream;
        Thread thread;
        ClientInfo ClientInfo;
        private readonly object _lock = new object();
        public Client(IPAddress ip, int port)
        {
            ServerIp = ip;
            ServerPort = port;
            dataHandler = new DataHandler();
            client = new TcpClient();
        }

        private void connect()
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
            connect();
            NetworkStream = client.GetStream();
            ClientInfo = LoginClient(client);
            Console.WriteLine($"You are now logged in as: {ClientInfo.DisplayName}");
            thread = new Thread(o => dataHandler.Recivedata((TcpClient)o));
            thread.Start(client);
        }
        public void GetUserInput()
        {
            Console.WriteLine("Enter Message to Global Chat or enter 'EXIT'");
            Console.Write(">");
            string message = Console.ReadLine();

            while (message != "EXIT")
            {
                IRequestHandler requestHandler = new RequestHandle();
                Request request = requestHandler.Create(ClientInfo, RequestType.SendGlobalMessage, message);
                byte[] buffer = Serializator.Serialize(request);
                NetworkStream.Write(buffer, 0, buffer.Length);;
                message = Console.ReadLine();


            }
        }
        public void Stop()
        {
            client.Client.Shutdown(SocketShutdown.Send);
            thread.Join();
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
                string userName = Console.ReadLine();
                while (string.IsNullOrEmpty(userName))
                {
                    Console.WriteLine("Enter user name");
                    userName = Console.ReadLine();
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
