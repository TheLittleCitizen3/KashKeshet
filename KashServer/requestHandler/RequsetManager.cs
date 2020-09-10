using Common;
using KashServer.Chats;
using KashServer.Clients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace KashServer.requestHandler
{
    class RequsetManager
    {
        public void HandleRequest(Request request, ConcurrentDictionary<ClientInfo, Client> clients, List<BaseChat> chats)
        {
            RequestContext requestContext = new RequestContext();
            switch (request.Type)
            {
                case RequestType.ChangeCurrentChat:
                    requestContext.SetRequestAction(new ChangeCurrentChat());
                    break;
                case RequestType.GetConnectedUsers:
                    requestContext.SetRequestAction(new SendConnectedClients());
                    break;
                case RequestType.StartPrivteChat:
                    break;
                case RequestType.SendGlobalMessage:
                    requestContext.SetRequestAction(new SendGlobalMessage());
                    break;
                default:
                    break;
            }
            requestContext.Invoke(request, clients,chats);
        }
    }
}