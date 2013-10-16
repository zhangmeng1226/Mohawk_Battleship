using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.Terminal
{
    public class ColorStore
    {
        private ConsoleColor backgroundColor;
        private ConsoleColor foregroundColor;

        protected ColorStore(ConsoleColor back, ConsoleColor front)
        {
            backgroundColor = back;
            foregroundColor = front;
        }

        public static ColorStore StoreCurrentColors()
        {
            return new ColorStore(Console.BackgroundColor, Console.ForegroundColor);
        }

        public void Restore()
        {
            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
        }
    }
}