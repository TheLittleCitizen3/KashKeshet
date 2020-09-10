using Common;
using KashServer.Chats;
using KashServer.Clients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace KashServer.requestHandler
{
    public class RequestContext
    {
        private IRequestAction _requestAction;
        public void SetRequestAction(IRequestAction requestAction)
        {
            _requestAction = requestAction;
        }
        public void Invoke(Request request, ConcurrentDictionary<ClientInfo,Client> clients, List<BaseChat> chats)
        {
            _requestAction.Invoke(request,clients,chats);
        }
    }
}
