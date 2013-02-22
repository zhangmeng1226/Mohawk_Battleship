using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Battleship
{
    public class BattleshipConsole
    {
        private BattleshipConfig config;
        private string[] fsStrings = { "first", "second" };

        public BattleshipConsole()
        {
            config = new BattleshipConfig(Environment.CurrentDirectory + "\\..\\config.ini");
        }

        abstract class ConsoleState
        {
            protected BattleshipConsole main;
            protected string extraMenu = "";
            protected string headerEnds = "=====================";

            public ConsoleState(BattleshipConsole main)
            {
                this.main = main;
            }

            public void WriteChars(char c, int n)
            {
                for (int i = 0; i < n; i++)
                    Console.Write(c);
            }

            public ConsoleState GetInput(string input)
            {
                switch (input)
                {
                    case "E": //Exit key
                        return null;
                    case "C": //Quit key
                        return this;
                }
                return Response(input);
            }

            public void WriteCenteredText(string text, string ends)
            {
                int maxChars = Console.WindowWidth - ends.Length * 2;
                int count = 0;
                while (count < text.Length)
                {
                    int printLen = text.Length - count > maxChars ? maxChars : text.Length - count;
                    int cen = maxChars / 2 - printLen / 2;
                    //Program.PrintDebugMessage("" + cen);

                    Console.Write(ends);
                    WriteChars(' ', cen);

                    Console.Write(text.Substring(count, printLen));

                    WriteChars(' ', Console.WindowWidth - (ends.Length * 2 + cen + printLen));
                    Console.Write(ends);
                    count += printLen;
                }
            }

            public void Display()
            {
                Console.Clear();
                WriteCenteredText("BATTLESHIP COMPETITION FRAMEWORK", headerEnds);
                StateDisplay();
                //WriteCenteredText("MENU", headerEnds);
                WriteCenteredText("[E]xit   [C]onfigure   " + extraMenu, "===");
                Console.Write("Input : ");
            }

            protected abstract void StateDisplay();
            protected abstract ConsoleState Response(string input);
        }

        class BotChoiceState : ConsoleState
        {

            private int opponentChoiceCount = 0;
            private IBattleshipOpponent[] opponents = new IBattleshipOpponent[2];
            private InputError error = InputError.none;

            public BotChoiceState(BattleshipConsole main)
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
                foreach (string botName in main.config.BotNames)
                {
                    Console.WriteLine((count++) + ": " + botName);
                }
                Console.WriteLine();
            }

            protected override void StateDisplay()
            {
                WriteCenteredText("Bot choice menu", headerEnds);
                Console.WriteLine();
                if (main.config.BotNames.Count == 0)
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
                WriteCenteredText("Enter a number for the " + main.fsStrings[opponentChoiceCount] + " opponent", "===");
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

                if (choice < 0 || choice >= main.config.BotNames.Count)
                {
                    error = InputError.noExist;
                    return this;
                }
                opponents[opponentChoiceCount++] = main.config.GetRobot(main.config.BotNames.ElementAt(choice));
                if (opponentChoiceCount > 1)
                    return new CompetitionState(main, opponents);
                return this;
            }
        }

        class CompetitionState : ConsoleState
        {
            public CompetitionState(BattleshipConsole main, IBattleshipOpponent[] opp) : base(main)
            {

            }

            protected override void StateDisplay()
            {

            }

            protected override ConsoleState Response(string input)
            {
                return this;
            }

        }

        public void Start()
        {
            Console.Title = "BATTLESHIP COMPETITION FRAMEWORK";
            Console.WriteLine("Welcome to the Battleship Competition!\n\n");

            ConsoleState state = new BotChoiceState(this);

            while (true)
            {
                state.Display();
                string input = Console.ReadLine();
                state = state.GetInput(input);
                if (state == null)
                    break;
            }

            /*BattleshipCompetition bc = new BattleshipCompetition(
                opponents[0],
                opponents[1],
                new TimeSpan(0, 0, 1),
                51,
                true,
                new Size(10, 10),
                2, 3, 3, 4, 5);

            Dictionary<IBattleshipOpponent, int> scores = bc.RunCompetition();

            foreach (var key in scores.Keys.OrderByDescending(k => scores[k]))
                Console.WriteLine(key.Name + " has scored " + scores[key]);

            Console.WriteLine("\n\nEnd of competition! Press a key to exit.");*/

            config.SaveConfigFile();
        }
    }
}
