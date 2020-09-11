using Common;
using KashClient.ResponsesHandler;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashClient
{
    public class DataHandler : IDataHandler
    {
        public void Recivedata(IClient client)
        {
            try
            {
                NetworkStream networkStream = client.TcpClient.GetStream();
                byte[] recivedBytes = new byte[1024];
                int byteCount;
                while ((byteCount = networkStream.Read(recivedBytes, 0, recivedBytes.Length)) > 0)
                {
                    Response response = (Response)Serializator.Deserialize(recivedBytes);
                    ResponseManager responseManager = new ResponseManager(client);
                    responseManager.HandleResponse(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server Error: " + ex.Message);
                Environment.Exit(0);
            }
        }
    }
}
