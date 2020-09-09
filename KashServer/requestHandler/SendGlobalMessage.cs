using Common;
using KashServer.Clients;
using KashServer.responseHandler;
using KashServer.SendRecive;
using KashServer.Strings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KashServer.requestHandler
{
    class SendGlobalMessage : IRequestAction
    {
        public void Invoke(Request request, ConcurrentDictionary<ClientInfo, Client> clients)
        {
            String data = (string)request.Content;
            data = MessageFormatter.FormatMessage(data,request.ClientsInfo);
            IResponseAction responseAction = new ResponseHandler();
            Response response = responseAction.CreateResponse(ResponseType.Text, data);
            byte[] serializedData = Serializator.Serialize(response);
            SendMessage.Send(serializedData,clients.Values.ToList());
        }
    }
}
