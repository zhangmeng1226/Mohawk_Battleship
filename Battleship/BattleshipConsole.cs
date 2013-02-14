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

        public BattleshipConsole()
        {
            config = new BattleshipConfig(Environment.CurrentDirectory + "\\..\\config.ini");
        }

        public void Start()
        {

            Console.WriteLine("Welcome to the Battleship Competition!\n\n");

            ReadOnlyCollection<string> botNames = config.GetRobotNames();
            if (botNames.Count == 0)
            {
                Console.WriteLine("There are no bots that can be listed. Program ending.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Console.WriteLine("Here is a list of bots currently available:\n");

            int count = 1;
            foreach (string botName in botNames)
            {
                Console.WriteLine((count++) + ": " + botName);
            }
            Console.WriteLine();

            IBattleshipOpponent[] opponents = new IBattleshipOpponent[2];
            string[] fsStrings = { "first", "second" };
            for (int i = 0; i < 2; i++)
            {
                Console.Write("Enter the number for the " + fsStrings[i] + " opponent: ");
                int choice = int.Parse(Console.ReadLine()) - 1;
                while (choice < 0 || choice >= botNames.Count)
                {
                    Console.Write("There doesn't exist a bot for that number. Try again: ");
                    choice = int.Parse(Console.ReadLine()) - 1;
                }
                opponents[i] = config.GetRobot(botNames.ElementAt(choice));
            }

            BattleshipCompetition bc = new BattleshipCompetition(
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

            Console.WriteLine("\n\nEnd of competition! Press a key to exit.");

            config.SaveConfigFile();
            Console.ReadKey();
        }
    }
}
