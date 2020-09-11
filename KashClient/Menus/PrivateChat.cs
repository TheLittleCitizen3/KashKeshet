using KashClient.Menus.Validations;
using MenuBuilder;
using MenuBuilder.menu;
using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace KashClient.Menus
{
    class PrivateChat : ITakeAction
    {
        public string ActionName { get; set; }
        private IClient _Client { get; set; }
        private List<IInputvalidation> _inputvalidations;

        public PrivateChat(IClient client)
        {
            ActionName = "2.Private Chat";
            _Client = client;
            _inputvalidations = new List<IInputvalidation>() { new IntInputValidation(), new IntOptionValidation(1, 2) };
        }
        public void Act()
        {
            int option = GetNewOrExistChatOption();
            if (option == 1)
            {
                _Client.SendRequest(RequestType.GetPrivateChats, "");
            }
            _Client.SendRequest(RequestType.GetConnectedUsers, "");
            

        }
        private int GetNewOrExistChatOption()
        {
            Console.WriteLine("1.List existing private chats");
            Console.WriteLine("2.Create new private chat ");
            string userInput = Console.ReadLine();
            while(!_inputvalidations[0].Validate(userInput) && !_inputvalidations[1].Validate(userInput))
            {
                Console.WriteLine("Enter valid option between 1 - 2");
                userInput = Console.ReadLine();
            }
            int option;
            int.TryParse(userInput, out option);
            return option;
        }
    }
}
