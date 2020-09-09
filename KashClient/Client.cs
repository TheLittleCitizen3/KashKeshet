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
            thread = new Thread(o => dataHandler.Recivedata((TcpClient)o));
            thread.Start(client);
            GetUserInput();
        }
        private void GetUserInput()
        {
            string s;
            while (!string.IsNullOrEmpty((s = Console.ReadLine())))
            {
                IRequestHandler requestHandler = new RequestHandle();
                Request request = requestHandler.Create(ClientInfo, RequestType.SendGlobalMessage, s);
                byte[] buffer = Serializator.Serialize(request);
                NetworkStream.Write(buffer, 0, buffer.Length);
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
            } while (response.ResponseType == ResponseType.ClientAllreadyExist);
            return null;
        }
    }
}
