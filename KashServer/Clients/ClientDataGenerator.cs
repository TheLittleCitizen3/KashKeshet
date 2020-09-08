using System;
using System.Collections.Generic;
using System.Text;

namespace KashServer.Clients
{
    public static class ClientDataGenerator
    {
        public static string generateName()
        {
            string uniqID = Guid.NewGuid().ToString();
            uniqID = uniqID.Substring(uniqID.Length - 5);
            string name = "User" + uniqID;
            return name;
        }
    }
}
