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

        [STAThread]
        public static void Main(string[] args)
        {
            ProgramMode mode = null;
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                    switch (args[i])
                    {
                        case "-nogui":
                        case "-console":
                            mode = new BattleshipConsole();
                            break;
                        case "-gui2D":
                            mode = new WPFAppMode();
                            break;
                        case "-gui":
                            mode = new Battleship3D();
                            break;
                    }
            }
            if (mode == null)
            {
                Program.PrintDebugMessage("Display mode not specified, defaulting to WPF.");
                mode = new WPFAppMode();
            }
            Program.PrintDebugMessage("Starting...");
            mode.Start();
            /*
            Application.EnableVisualStyles();
            Battleship2D mode2D = new Battleship2D();
            Application.Run(mode2D);*/

            //BattleshipConsole consoleMode = new BattleshipConsole();

            //consoleMode.Start();
            Program.PrintDebugMessage("Program Main function has ended.");
        }
    }
}
