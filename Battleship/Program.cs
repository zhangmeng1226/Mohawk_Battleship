namespace Battleship
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.IO;
    using System.Collections.Generic;

    using IrrlichtLime;
    using IrrlichtLime.Core;
    using IrrlichtLime.Video;
    using IrrlichtLime.Scene;
    using IrrlichtLime.GUI;

    class Program
    {
        private static Dictionary<string, Type> loadedRobots = new Dictionary<string, Type>();
        private static BattleshipConfig config;

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
                    LoadRobots(file);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine("The bot directory could not be found.");
            }
        }

        static void Main(string[] args)
        {
            config = new BattleshipConfig(Environment.CurrentDirectory+"\\config.ini");
            LoadRobotFolder();

            IrrlichtDevice dev = config.GetConfiguredDevice();
            //logic, rendering, updating, whatever, then done..

            dev.Close();

            config.SaveConfigFile();
            Console.ReadKey();
        }
    }
}
