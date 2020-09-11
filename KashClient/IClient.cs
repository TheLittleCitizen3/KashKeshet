using Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashClient
{
    public interface IClient
    {
        public TcpClient TcpClient { get; set; }
        void Start();
        string GetUserInput();
        public void SendRequest(RequestType requestType, object obj);
        void Stop();
    }
}
