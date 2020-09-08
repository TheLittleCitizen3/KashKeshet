using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashClient
{
    public class DataHandler : IDataHandler
    {
        public void Recivedata(TcpClient client)
        {
            NetworkStream networkStream = client.GetStream();
            byte[] recivedBytes = new byte[1024];
            int byteCount;
            while ((byteCount = networkStream.Read(recivedBytes, 0, recivedBytes.Length)) > 0)
            {
                string message = Encoding.ASCII.GetString(recivedBytes, 0, byteCount);
                Console.WriteLine(message);
            }
        }
    }
}
