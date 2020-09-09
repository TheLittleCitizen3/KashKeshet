using KashClient.Menus;
using System;
using System.Net;

namespace KashClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu menu = new MainMenu();
            menu.Start();
            IClient client = new Client(IPAddress.Parse("127.0.0.1"),9000);
            client.Start();
            client.Stop();
        }
    }
}
