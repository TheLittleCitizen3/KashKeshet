using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [Serializable]
    public class ClientInfo
    {
        public readonly Guid UID;
        public string DisplayName { get; set; }
        public ClientInfo(string displayName)
        {
            UID = Guid.NewGuid();
            DisplayName = displayName;
        }
    }
}
