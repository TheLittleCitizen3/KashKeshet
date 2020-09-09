using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.RequestHandler
{
    public interface IRequestHandler
    {
        public Request Create(ClientInfo clientInfo, RequestType requestType, Object content);
    }
}
