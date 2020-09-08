﻿using Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace KashServer.Clients
{
    public class Client
    {
        public ClientInfo ClientInfo { get; set; }
        public TcpClient TcpClient { get; set; }
        public ClientStatus ClientStatus { get; set; }
        public List<IClientsGroup> MemberInGroups { get; set; }
        public Client(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
            ClientInfo = new ClientInfo(ClientDataGenerator.generateName());
            ClientStatus = ClientStatus.Connected;
            MemberInGroups = new List<IClientsGroup>();
        }

    }
}
