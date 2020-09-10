﻿using Common;
using KashServer.Chats;
using KashServer.Clients;
using KashServer.responseHandler;
using KashServer.SendRecive;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KashServer.requestHandler
{
    class SendConnectedClients : IRequestAction
    {
        public void Invoke(Request request, ConcurrentDictionary<ClientInfo, Client> clients, List<BaseChat> chats)
        {
            var connectedClients = clients
                .Select(c => c.Value)
                .Where(c => c.ClientStatus == ClientStatus.Connected).ToList();
            var selfClient = connectedClients.Where(c => c.ClientInfo == request.ClientsInfo);
            connectedClients.Remove(selfClient.FirstOrDefault());
            var connectedClientsInfo = connectedClients.Select(c => c.ClientInfo).ToList();
            IResponseAction responseAction = new ResponseHandler();
            Response response = responseAction.CreateResponse(ResponseType.ClientsInfo, connectedClientsInfo);
            byte[] serializedResponse = Serializator.Serialize(response);
            SendMessage.Send(serializedResponse,selfClient.ToList());
        }
    }
}
