using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.ResponsesHandler
{
    public class PrintMessage : IResponseAction
    {
        public void Invoke(Response response)
        {
            string data = response.Content.ToString();
            Console.WriteLine(data);
        }
    }
}
