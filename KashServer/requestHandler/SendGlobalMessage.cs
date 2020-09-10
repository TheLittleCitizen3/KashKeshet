using Common;
using KashServer.Chats;
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
        public void Invoke(Request request, ConcurrentDictionary<ClientInfo, Client> clients,List<BaseChat> chats)
        {
            String data = (string)request.Content;
            data = MessageFormatter.FormatMessage(data,request.ClientsInfo);

            IResponseAction responseAction = new ResponseHandler();
            Response response = responseAction.CreateResponse(ResponseType.Text, data);
            byte[] serializedData = Serializator.Serialize(response);

            var globalChatClients = chats.FirstOrDefault(c=>c.ChatInfo.Type == ChatType.GlobalChat)
                .ActiveClientsInChat
                .Values
                .ToList();
            SendMessage.Send(serializedData,globalChatClients);
        }
    }
}
