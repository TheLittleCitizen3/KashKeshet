using Common;
using KashServer.Clients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace KashServer.requestHandler
{
    class RegisterClient : IRequestAction
    {
        public void Invoke(Request request,ConcurrentDictionary<ClientInfo,Client> clients)
        {
            Client client = clients[request.ClientInfo];

        }
    }
}
