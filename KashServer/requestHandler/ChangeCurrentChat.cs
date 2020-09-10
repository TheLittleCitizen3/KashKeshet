using Common;
using KashServer.Chats;
using KashServer.Clients;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KashServer.requestHandler
{
    class ChangeCurrentChat : IRequestAction
    {
        public void Invoke(Request request, ConcurrentDictionary<ClientInfo, Client> clients, List<BaseChat> chats)
        {
            Client client = clients.FirstOrDefault(c => c.Value.ClientInfo.DisplayName == request.ClientsInfo.DisplayName).Value;
            ChatInfo chatInfo = (ChatInfo)request.Content;
            if (chatInfo.Type == ChatType.GlobalChat)
            {
                ChangeChatToGlobal(client, chats);
                return;
            }
            if (chatInfo.Type == ChatType.None)
            {
                ChangeFromChatToMenu(client, chats, chatInfo);
                return;
            }
            ChangeFromMenuToChat(client, chats, chatInfo);


        }

        private void ChangeChatToGlobal(Client client, List<BaseChat> chats)
        {
            var globalChat = chats.FirstOrDefault(c => c.ChatInfo.Type == ChatType.GlobalChat);
            globalChat.ActiveClientsInChat.TryAdd(client.ClientInfo, client);
            client.ChatInfo = globalChat.ChatInfo;
        }
        private void ChangeFromMenuToChat(Client client, List<BaseChat> chats, ChatInfo newChat)
        {
            var chatToEnter = chats.FirstOrDefault(c => c.ChatInfo == newChat);
            chatToEnter.ActiveClientsInChat.TryAdd(client.ClientInfo, client);
            client.ChatInfo = newChat;
        }
        private void ChangeFromChatToMenu(Client client, List<BaseChat> chats, ChatInfo menuChat)
        {
            var chatToRemove = chats.FirstOrDefault(c => c.ChatInfo == client.ChatInfo);
            chatToRemove.ActiveClientsInChat.TryRemove(client.ClientInfo, out _);
            client.ChatInfo = menuChat;
        }
    }
}
