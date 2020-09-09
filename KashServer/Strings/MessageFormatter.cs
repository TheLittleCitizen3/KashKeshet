using Common;
using KashServer.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashServer.Strings
{
    public static class MessageFormatter
    {
        public static string FormatMessage(string data, ClientInfo clientInfo)
        {
            DateTime dateTime = DateTime.Now;
            string displayName = clientInfo.DisplayName;
            data = $"[{dateTime}] " + $"{displayName} > " + data;
            return data;
        }

    }
}
