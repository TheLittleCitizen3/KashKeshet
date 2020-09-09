using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.ResponsesHandler
{
    public class ResponseManager
    {
        public void HandleResponse(Response response)
        {
            ResponseContext ResponseContext = new ResponseContext();

            switch (response.ResponseType)
            {
                case ResponseType.Text:
                    ResponseContext.SetResponseAction(new PrintMessage());
                    break;
                case ResponseType.ClientsInfo:
                    break;
                default:
                    break;
            }
            ResponseContext.Invoke(response);
        }
    }
}
