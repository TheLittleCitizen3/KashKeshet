using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Common;


namespace KashServer
{
    public interface IChat
    {
        List<TcpClient> Clients { get; set; }
        List<ClientInfo> Admin { get; set; }
        IDisplay Display { get; set; }
    }
}
