using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal
{
    public class ConsoleColorSet
    {

        private ConsoleColor foreground;
        private ConsoleColor background;

        public ConsoleColorSet(ConsoleColor fore, ConsoleColor back)
        {
            foreground = fore;
            background = back;
        }

        public ConsoleColor Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                foreground = value;
            }
        }

        public ConsoleColor Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
            }
        }
    }
}
