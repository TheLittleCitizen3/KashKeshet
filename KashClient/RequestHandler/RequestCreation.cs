using Common;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace KashClient.RequestCreator
{
    public class RequestCreator : IRequestCreation
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
