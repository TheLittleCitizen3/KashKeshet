using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.ResponsesHandler
{
    public class ResponseContext
    {
        private IResponseAction _responseAction;
        
        public void SetResponseAction(IResponseAction responseAction)
        {
            _responseAction = responseAction;
        }
        public void Invoke(Response response)
        {
            _responseAction.Invoke(response);
        }
    }
}
