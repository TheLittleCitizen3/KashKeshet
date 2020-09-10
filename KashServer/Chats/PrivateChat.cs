﻿using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace KashServer.Chats
{
    public class PrivateChat : BaseChat
    {
        public PrivateChat():base()
        {
            ChatInfo.Type = ChatType.PrivateChat;
        }

    }
}
