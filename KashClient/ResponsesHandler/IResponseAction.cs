using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.ResponsesHandler
{
    public interface IResponseAction
    {
        void Invoke(Response response);
    }
}
