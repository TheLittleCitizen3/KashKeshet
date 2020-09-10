using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient
{
    interface IClient
    {
        void Start();
        void GetUserInput();
        void Stop();
    }
}
