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
            thread = new Thread(o => dataHandler.Recivedata((TcpClient)o));
            thread.Start(client);
            GetUserInput();
        }
        private void GetUserInput()
        {
            string s;
            while (!string.IsNullOrEmpty((s = Console.ReadLine())))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(s);
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
    }
}
