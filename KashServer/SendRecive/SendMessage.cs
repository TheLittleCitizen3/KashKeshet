using KashServer.Clients;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashServer.SendRecive
{
    public static class SendMessage
    {
        readonly static object _lock = new object();
        public static void Send(string data, List<Client> clients)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            lock (_lock)
            {
                foreach (Client c in clients)
                {
                    NetworkStream stream = c.TcpClient.GetStream();

                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
