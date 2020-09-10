using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    [Serializable]
    public class ChatInfo
    {
        public ChatType Type { get; set; }
        public String Name { get; set; }
        public String Id { get; set; }
    }
}
