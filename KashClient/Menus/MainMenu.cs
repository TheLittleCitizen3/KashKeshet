using KashClient.Menus.Validations;
using MenuBuilder;
using MenuBuilder.menu;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KashClient.Menus
{
    public class MainMenu
    {
        private StringMenu Menu;
        private IClient client;

        public MainMenu()
        {
            Dictionary<string, ITakeAction> ActionItems = new Dictionary<string, ITakeAction>();
            client = new Client(IPAddress.Parse("127.0.0.1"), 9000);
            ITakeAction globalChat = new GlobalChat(client);
            ActionItems.Add("1", globalChat);
            List<IInputvalidation> inputvalidations = new List<IInputvalidation>() { new IntInputValidation(), new MainMenuValidation() };
            Menu = new StringMenu(ActionItems, inputvalidations);
        }
        public void Start()
        {
            client.Start();
            while (true)
            {
                Menu.Start();
                Console.WriteLine("Press any key to continue....");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
