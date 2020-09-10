﻿using Common;
using MenuBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.Menus
{
    class GlobalChat : ITakeAction
    {
        public string ActionName { get; set; }
        IClient Client;

        public GlobalChat(IClient client)
        {
            ActionName = "1. Global Chat";
            Client = client;
            
        }
        public void Act()
        {
            ChatInfo chatInfo = new ChatInfo();
            chatInfo.Type = ChatType.GlobalChat;
            Client.SendRequest(RequestType.ChangeCurrentChat, chatInfo);
            Console.WriteLine("Enter Message to Global Chat or enter 'EXIT'");
            string message = Client.GetUserInput();
            while (message != "EXIT")
            {
                Client.SendRequest(RequestType.SendGlobalMessage, message);
                message = Client.GetUserInput();
            }
            chatInfo.Type = ChatType.None;
            Client.SendRequest(RequestType.ChangeCurrentChat, chatInfo);
        }
    }
}
