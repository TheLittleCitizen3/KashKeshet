﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [Serializable]
    public class Request
    {
        public ClientInfo ClientsInfo;
        public RequestType Type;
        public Object Content;
    }
}
