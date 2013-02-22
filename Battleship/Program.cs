namespace Battleship
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.IO;
    using System.Collections.Generic;
    using System.Drawing;

    public class Program
    {
        private Program() {}

        /**
         * <summary>Used for printing verbose messages while in Debug mode only.</summary>
         * <param name="message">The message to print to the console</param>
         */
        public static void PrintDebugMessage(string message)
        {
#if DEBUG
            string[] mLineArray = message.Split('\n');
            foreach (string line in mLineArray)
                Console.WriteLine("DEBUG: " + line);
#endif
        }

        static void Main(string[] args)
        {
            BattleshipConsole consoleMode = new BattleshipConsole();

            consoleMode.Start();
        }
    }
}
