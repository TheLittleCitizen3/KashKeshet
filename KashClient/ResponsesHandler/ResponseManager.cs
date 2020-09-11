using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.ResponsesHandler
{
    public class ResponseManager
    {
        private IClient _client { get; set; }
        public ResponseManager(IClient client)
        {
            _client = client;
        }
        public void HandleResponse(Response response)
        {
            ResponseContext ResponseContext = new ResponseContext();

            switch (response.ResponseType)
            {
                case ResponseType.Text:
                    ResponseContext.SetResponseAction(new PrintMessage());
                    break;
                case ResponseType.ClientsInfo:
                    ResponseContext.SetResponseAction(new ActiveUsersToChat(_client));
                    break;
                case ResponseType.StartPrivateChat:
                    ResponseContext.SetResponseAction(new StartPrivateChat(_client));
                    break;
                default:
                    break;
            }
            ResponseContext.Invoke(response);
        }
    }
}
