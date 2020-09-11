using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace KashClient
{
    public interface IDataHandler
    {
        void Recivedata(IClient client);
    }
}
