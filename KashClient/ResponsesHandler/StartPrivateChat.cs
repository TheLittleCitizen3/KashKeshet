using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.ResponsesHandler
{
    public class StartPrivateChat : IResponseAction
    {
        private IClient _client;
        private ChatInfo _privateChatInfo;

        public StartPrivateChat(IClient client)
        {
            _client = client;
        }

        public void Invoke(Response response)
        {
            _privateChatInfo = (ChatInfo)response.Content;
            Console.WriteLine($"Enter messege to {_privateChatInfo.Name} or type EXIT to finish chat");
            string message = _client.GetUserInput();
            while (message != "EXIT")
            {
                _client.SendRequest(RequestType.SendPrivateOrGroupMessage, message);
                message = _client.GetUserInput();
            }
            ChatInfo exitChatInfo = new ChatInfo() { Type = ChatType.None };
            _client.SendRequest(RequestType.ChangeCurrentChat, exitChatInfo);
        }
    }
}
