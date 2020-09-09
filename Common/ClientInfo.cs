using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [Serializable]
    public class ClientInfo
    {
        public string DisplayName { get; set; }
        public ClientInfo(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
