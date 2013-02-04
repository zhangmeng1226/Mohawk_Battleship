namespace Battleship
{
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.IO;
    using System.Collections.Generic;

    class Program
    {
        private static Dictionary<string, Type> loadedRobots = new Dictionary<string, Type>();

        /**
         * <summary>Returns the object that is identified by name.</summary>
         * <returns>A Robot loaded externally by a DLL, having the same Name property from IBattleshipOpponent as the parameter.</returns>
         */
        public static IBattleshipOpponent GetRobot(string name)
        {
            Type result = null;
            loadedRobots.TryGetValue(name, out result);
            return (IBattleshipOpponent)Activator.CreateInstance(result);
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
                Console.Write("Failed to load "+fName+": "+e.StackTrace);
            }
        }

        /**
         * <summary>Opens each .DLL file in the working directory's "bots" folder.</summary>
         */
        private static void LoadRobotFolder()
        {
            try
            {
                List<string> files = new List<string>(Directory.GetFiles(Environment.CurrentDirectory+"\\bots", "*.dll"));
                foreach (string file in files)
                {
                    LoadRobots(file);
                }
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine("The bot directory could not be found.");
            }
        }

        static void Main(string[] args)
        {
            LoadRobotFolder();

            IBattleshipOpponent op1 = GetRobot("Random");
            IBattleshipOpponent op2 = GetRobot("Random");

            BattleshipCompetition bc = new BattleshipCompetition(
                op1,
                op2,
                new TimeSpan(0, 0, 1),  // Time per game
                51,                     // Wins per match
                true,                   // Play out?
                new Size(10, 10),       // Board Size
                2, 3, 3, 4, 5           // Ship Sizes
            );

            var scores = bc.RunCompetition();

            foreach (var key in scores.Keys.OrderByDescending(k => scores[k]))
            {
                Console.WriteLine("{0} {1}:\t{2}", key.Name, key.Version, scores[key]);
            }
            Console.ReadKey(true);
        }
    }
}
