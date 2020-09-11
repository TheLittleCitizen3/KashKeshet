using System;
using System.Collections.Generic;
using System.Text;
using MenuBuilder.menu;
using Microsoft.Win32.SafeHandles;

namespace KashClient.Menus.Validations
{
    class IntOptionValidation : IInputvalidation
    {
        private int _minValue;
        private int _maxValue;
        public IntOptionValidation(int min, int max)
        {
            _minValue = min;
            _maxValue = max;
        }
        public bool Validate(string userInput)
        {
            int choice = int.Parse(userInput);
            return choice >= 1 && choice <= 5;
        }
    }
}
