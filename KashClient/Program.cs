using System;
using System.Net;

namespace KashClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IClient client = new Client(IPAddress.Parse("127.0.0.1"),9000);
            client.Start();
            client.Stop();
        }
    }
}
