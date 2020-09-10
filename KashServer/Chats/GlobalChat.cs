using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace KashServer.Chats
{
    public class GlobalChat : BaseChat
    {
        public GlobalChat() : base()
        {
            ChatInfo.Type = ChatType.GlobalChat;
        }
    }
}
