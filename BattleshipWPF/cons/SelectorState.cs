using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship
{
    public class SelectorState : ConsoleState
    {
        private int controllerChoiceCount = 0; //The current battleship bot the user is selecting.
        private IBattleshipController[] controllers = new IBattleshipController[2]; //The battleship bots being selected.
        private InputError error = InputError.none; //The error, based on user input.
        private string[] fsStrings = { "first", "second" };

        public SelectorState(BattleshipConsole main)
            : base(main)
        {
        }

        private enum InputError
        {
            none,
            noExist,
            badNumber
        }

        private void ListBots()
        {
            Console.WriteLine("Here is a list of bots currently available:\n");

            int count = 1;
            foreach (string botName in main.Config.BotNames)
                Console.WriteLine((count++) + ": " + botName);
            Console.WriteLine();
        }

        protected override void StateDisplay()
        {
            WriteCenteredText("Bot choice menu", headerEnds);
            Console.WriteLine();
            if (main.Config.BotNames.Count == 0)
            {
                Console.WriteLine("There are no bots that can be listed.");
                return;
            }

            ListBots();
            Console.WriteLine("\n\n");
            switch (error)
            {
                case InputError.noExist:
                    Console.WriteLine("> That selection doesn't exist!");
                    break;
                case InputError.badNumber:
                    Console.WriteLine("> Invalid input.");
                    break;
            }
            WriteCenteredText("Enter a number for the " + fsStrings[controllerChoiceCount] + " opponent", "===");
        }

        protected override ConsoleState Response(string input)
        {
            error = InputError.none;
            int choice = -1;

            try
            {
                choice = int.Parse(input) - 1;
            }
            catch
            {
                error = InputError.badNumber;
                return this;
            }

            if (choice < 0 || choice >= main.Config.BotNames.Count)
            {
                error = InputError.noExist;
                return this;
            }
            controllers[controllerChoiceCount++] = main.Config.GetRobot(main.Config.BotNames.ElementAt(choice));
            if (controllerChoiceCount > 1)
                return new CompetitionState(main, controllers);
            return this;
        }
    }
}
