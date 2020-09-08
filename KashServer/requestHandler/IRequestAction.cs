using Common;
using KashServer.Clients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace KashServer
{
    public interface IRequestAction
    {
        void Invoke(Request request,ConcurrentDictionary<ClientInfo,Client> clients);
    }
}
