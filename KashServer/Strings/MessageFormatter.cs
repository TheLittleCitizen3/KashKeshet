using KashServer.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashServer.Strings
{
    public static class MessageFormatter
    {
        public static string FormatMessage(string data, Client client)
        {
            DateTime dateTime = DateTime.Now;
            string displayName = client.ClientInfo.DisplayName;
            data = $"[{dateTime}] " + $"{displayName} > " + data;
            return data;
        }
    }
}
