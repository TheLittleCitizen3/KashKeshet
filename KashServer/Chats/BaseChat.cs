using Common;
using KashServer.Clients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace KashServer.Chats
{
    public abstract class BaseChat
    {

        public ConcurrentDictionary<ClientInfo,Client> ChatMembers { get; set; }
        public ConcurrentDictionary<ClientInfo,Client> ActiveClientsInChat { get; set; }
        public ChatInfo ChatInfo { get; set; }

        public BaseChat()
        {
            ChatMembers = new ConcurrentDictionary<ClientInfo, Client>();
            ActiveClientsInChat = new ConcurrentDictionary<ClientInfo, Client>();
            ChatInfo = new ChatInfo
            {
                Id = Guid.NewGuid().ToString()
            };
        }
        public virtual void  AddMemberToChat(Client client)
        {
            ChatMembers.TryAdd(client.ClientInfo, client);
        }
        public virtual void MakeMemberActiveInChat(ClientInfo clientInfo)
        {
            ActiveClientsInChat.TryAdd(clientInfo, ChatMembers[clientInfo]);
        }
        public virtual void MakeMemberActiveInChat(Client client)
        {
            ActiveClientsInChat.TryAdd(client.ClientInfo, client);
        }
        public virtual void DiconnectClientFromChat(ClientInfo clientInfo)
        {
            ActiveClientsInChat.TryRemove(clientInfo, out _);
        }
        public virtual void DiconnectClientFromChat(Client client)
        {
            ActiveClientsInChat.TryRemove(client.ClientInfo, out _);
        }
        public virtual void RemoveMemberFromChat(Client client)
        {
            ChatMembers.TryRemove(client.ClientInfo, out _);
        }
        public virtual void RemoveMemberFromChat(ClientInfo clientInfo)
        {
            ChatMembers.TryRemove(clientInfo, out _);
        }
    }
}
