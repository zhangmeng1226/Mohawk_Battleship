using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Battleship.Cons
{
    /**
     * <summary>Provides an interactive console for the user.</summary>
     */
    public class BattleshipConsole : ProgramMode
    {
        private BattleshipConfig config; //The main configuration object
        private string[] fsStrings = { "first", "second" }; //Strings used to denote "first" and "second"

        public BattleshipConfig Config
        {
            get{ return config; }
        }

        /**
         * <summary>Class constructor. Initializes the configuration object.</summary>
         */
        public BattleshipConsole()
        {
            config = new BattleshipConfig(Environment.CurrentDirectory + "\\..\\config.ini");
        }

        public void Start()
        {
            Console.Title = "BATTLESHIP COMPETITION FRAMEWORK";
            Console.WriteLine("Welcome to the Battleship Competition!\n\n");

            ConsoleState state = new SelectorState(this);

            while (true)
            {
                state.Display();
                string input = Console.ReadLine();
                state = state.GetInput(input);
                if (state == null)
                    break;
            }

            config.SaveConfigFile();
        }
    }
}
