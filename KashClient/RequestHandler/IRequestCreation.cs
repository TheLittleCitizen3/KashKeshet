using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.RequestCreator
{
    public interface IRequestCreation
    {
        public Request Create(ClientInfo clientInfo, RequestType requestType, Object content);
    }
}
