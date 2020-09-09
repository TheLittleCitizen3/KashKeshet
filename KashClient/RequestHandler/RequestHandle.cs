using Common;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace KashClient.RequestHandler
{
    public class RequestHandle : IRequestHandler
    {
        public Request Create(ClientInfo clientInfo, RequestType requestType, object content)
        {
            Request request = new Request
            {
                ClientsInfo = clientInfo,
                Type = requestType,
                Content = content
                
            };
            return request;
        }
    }
}
