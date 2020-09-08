using Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private ConcurrentDictionary<ClientInfo, TcpClient> Clients { get; set; }
        readonly object _lock = new object();
        IPAddress Ip;
        int Port;
        public Server(IPAddress ip, int port, ILogger<Worker> logger)
        {
            Clients = new ConcurrentDictionary<ClientInfo, TcpClient>();
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
                TcpClient client = ServerSocket.AcceptTcpClient();
                ClientInfo clientInfo = new ClientInfo("");
                Clients.TryAdd(clientInfo, client);
                _logger.LogInformation("New Client Was Added!");

                Thread t = new Thread(HandleClient);
                t.Start(clientInfo);

            }
        }
        private void HandleClient(object obj)
        {
            ClientInfo clientInfo = (ClientInfo)obj;
            TcpClient client;
            client = Clients[clientInfo];
            while (true)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int byte_count = stream.Read(buffer, 0, buffer.Length);

                    if (byte_count == 0)
                    {
                        break;
                    }

                    string data = Encoding.ASCII.GetString(buffer, 0, byte_count);
                    Brodcast(data);
                    _logger.LogInformation($"Client id:{clientInfo.UID} Write:{data}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("user disconnected with massage:" + ex.Message);
                    break;
                }
                
            }

            TcpClient tcpClient;
            Clients.TryRemove(clientInfo, out tcpClient);
            client.Client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        private void Brodcast(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data + Environment.NewLine);

            lock (_lock)
            {
                foreach (TcpClient c in Clients.Values)
                {
                    NetworkStream stream = c.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}

