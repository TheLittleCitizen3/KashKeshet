using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient
{
    interface IClient
    {
        void Start();
        string GetUserInput();
        public void SendRequest(RequestType requestType, object obj);
        void Stop();
    }
}
