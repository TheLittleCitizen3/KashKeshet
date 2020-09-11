using Common;
using KashClient.Menus.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace KashClient.ResponsesHandler
{
    class ActiveUsersToChat : IResponseAction
    {
        private List<ClientInfo> _clientInfos;
        private IClient _client;

        public ActiveUsersToChat(IClient client)
        {
            _client = client;
        }
      
        public void Invoke(Response response)
        {
            _clientInfos = (List<ClientInfo>)response.Content;
            if (_clientInfos.Count == 0)
            {
                Console.WriteLine("No Connected users");
                return;
            }
            Console.WriteLine($"Choose user numbe to start chat");
            PrintConnectedUsers();
            string userInput = Console.ReadLine();
            IntOptionValidation validation = new IntOptionValidation(1, _clientInfos.Count);
            while (!validation.Validate(userInput))
            {
                Console.WriteLine("Enter Valid Number");
                userInput = Console.ReadLine();
            }
            int.TryParse(userInput, out int privateUserIndex);
            ClientInfo privateChateClientInfo = _clientInfos[privateUserIndex -1];
            _client.SendRequest(RequestType.StartPrivteChat, privateChateClientInfo);
        }

        private void PrintConnectedUsers()
        {
            int index = 1;
            foreach (var clientInfo in _clientInfos)
            {
                Console.WriteLine($"{index}. {clientInfo.DisplayName}");
            }
        }
    }
}
