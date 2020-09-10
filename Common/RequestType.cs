﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [Serializable]
    public enum RequestType
    {
        Register,
        GetConnectedUsers,
        StartPrivteChat,
        SendGlobalMessage,
        ChangeCurrentChat,
        GetPrivateChats
    }
}
