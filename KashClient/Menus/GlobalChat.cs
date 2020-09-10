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
        { //cahnge here to init client before!
            Client.GetUserInput();
        }
    }
}
