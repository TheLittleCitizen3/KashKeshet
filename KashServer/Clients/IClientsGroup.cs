using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace KashServer.Clients
{
    public interface IClientsGroup
    {
        ConcurrentDictionary<ClientInfo,Client> GroupMembers { get; set; }
        ClientInfo GroupAdmin { get; set; }
        public void AddMember(Client client);
        public void RemoveMember(Client client);
        public void ListMembers();
    }
}
