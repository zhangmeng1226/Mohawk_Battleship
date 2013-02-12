namespace Battleship
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.IO;
    using System.Collections.Generic;
    using System.Drawing;

    class Program
    {
        private static Dictionary<string, Type> loadedRobots = new Dictionary<string, Type>();
        private static BattleshipConfig config;

        /**
         * <summary>Returns the object that is identified by name.</summary>
         * <returns>A Robot loaded externally by a DLL, having the same Name property from IBattleshipOpponent as the parameter.
         * If the class was not found, then this will return null.</returns>
         */
        public static IBattleshipOpponent GetRobot(string name)
        {
            Type result = null;
            loadedRobots.TryGetValue(name, out result);
            if (result == null)
                return (IBattleshipOpponent)result;
            return (IBattleshipOpponent)Activator.CreateInstance(result);
        }

        /**
         * <summary>Gets a list of all currently loaded robots</summary>
         */
        public static List<string> GetRobotNames()
        {
            return loadedRobots.Keys.ToList();
        }
        /**
         * <summary>Loads a .DLL file and saves objects that implement the IBattleshipOpponent interface.</summary>
         * <param name="fName">The absolute pathname to the .DLL file</param>
         * <remarks>It is currently possible to load more than one robot from a single DLL.</remarks>
         */
        public static void LoadRobots(string fName)
        {
            try
            {
                Assembly dllInfo = Assembly.LoadFile(fName);
                Type[] types = dllInfo.GetTypes();
                foreach (Type robot in types)
                    foreach (Type interfaces in robot.GetInterfaces())
                        if (interfaces.ToString() == "Battleship.IBattleshipOpponent")
                        {
                            IBattleshipOpponent opp = (IBattleshipOpponent)Activator.CreateInstance(robot);
                            loadedRobots.Add(opp.Name, robot);
                            break;
                        }
            }
            catch (Exception e)
            {
                Console.Write("Failed to load " + fName + ": " + e.StackTrace);
            }
        }

        /**
         * <summary>Opens each .DLL file in the working directory's "bots" folder.</summary>
         */
        private static void LoadRobotFolder()
        {
            try
            {
                List<string> files = new List<string>(Directory.GetFiles(Environment.CurrentDirectory + "\\..\\bots", "*.dll"));
                foreach (string file in files)
                    LoadRobots(file);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine("The bot directory could not be found. " + e.ToString());
            }
        }

        static void Main(string[] args)
        {
            config = new BattleshipConfig(Environment.CurrentDirectory + "\\..\\config.ini");
            LoadRobotFolder();

            Console.WriteLine("Welcome to the Battleship Competition!\n\n");

            List<string> botNames = GetRobotNames();
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
                opponents[i] = GetRobot(botNames.ElementAt(choice));
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
