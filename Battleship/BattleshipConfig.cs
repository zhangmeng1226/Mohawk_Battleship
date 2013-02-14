using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Battleship
{
    /**
     * <summary>BattleshipConfig deals with a single configuration file and parses each key/value pair</summary>
     */
    public class BattleshipConfig
    {
        private Dictionary<string, string> simpleConfig;
        private Dictionary<string, Type> loadedRobots = new Dictionary<string, Type>();
        private string rootPath;

        public List<string> BotNames
        {
            get
            {
                return loadedRobots.Keys.ToList();
            }
        }

        /**
         * <summary>Initializes this BattleshipConfig by loading from a config file, if it exists</summary>
         */
        public BattleshipConfig(string pathname)
        {
            simpleConfig = new Dictionary<string, string>();
            rootPath = pathname;
            LoadConfigFile();
            LoadRobotFolder();
        }

        /**
         * <summary>Returns the object that is identified by name.</summary>
         * <returns>A Robot loaded externally by a DLL, having the same Name property from IBattleshipOpponent as the parameter.
         * If the class was not found, then this will return null.</returns>
         */
        public IBattleshipOpponent GetRobot(string name)
        {
            Type result = null;
            loadedRobots.TryGetValue(name, out result);
            if (result == null)
                return null;
            return (IBattleshipOpponent)Activator.CreateInstance(result);
        }

        /**
         * <summary>Loads a .DLL file and saves objects that implement the IBattleshipOpponent interface.</summary>
         * <param name="fName">The absolute pathname to the .DLL file</param>
         * <remarks>It is currently possible to load more than one robot from a single DLL.</remarks>
         */
        public void LoadRobots(string fName)
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
                Program.PrintDebugMessage("Failed to load " + fName + ": " + e.ToString());
            }
        }

        /**
         * <summary>Opens each .DLL file in the working directory's "bots" folder.</summary>
         */
        private void LoadRobotFolder()
        {
            try
            {
                List<string> files = new List<string>(Directory.GetFiles(Environment.CurrentDirectory + "\\..\\bots", "*.dll"));
                foreach (string file in files)
                    LoadRobots(file);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Program.PrintDebugMessage("The bot directory could not be found. " + e.ToString());
            }
        }

        /**
         * <summary>Gets a value from the configuration</summary>
         * <param name="s">The key to get the value from</param>
         * <param name="def">The default value for a given key if the key is not found.</param>
         * <exception cref="Exception">If the value could not be converted</exception>
         */
        public T ConfigValue<T>(string s, T def)
        {
            string valString;
            try
            {
                if (simpleConfig.TryGetValue(s, out valString))
                    return (T)Convert.ChangeType(valString, typeof(T));
            }
            catch { }
            simpleConfig[s] = def.ToString();
            return def;
        }

        /**
         * <summary>Loads a delimited configuration file, delimited by '=', and stores each key and value.</summary>
         */
        private void LoadConfigFile()
        {
            try
            {
                StreamReader reader = new StreamReader(rootPath);
                do
                {
                    string line = reader.ReadLine();
                    string[] tLine = line.Split('=');
                    tLine[0] = tLine[0].Trim();
                    tLine[1] = tLine[1].Trim();
                    Console.WriteLine("Key: \"" + tLine[0] + "\"   Value: \"" + tLine[1] + "\"");
                    simpleConfig.Add(tLine[0], tLine[1]);
                } while (reader.Peek() != -1);
                reader.Close();
            }
            catch
            {
                Program.PrintDebugMessage("Could not load configuration file for reading.");
            }
        }

        /**
         * <summary>Saves the current configuration to a file.</summary>
         * <exception cref="IOException">The file could not be written to.</exception>
         */
        public void SaveConfigFile()
        {
            try
            {
                StreamWriter writer = new StreamWriter(rootPath, false);
                foreach (KeyValuePair<string, string> entry in simpleConfig)
                    writer.WriteLine(entry.Key + " = " + entry.Value);
                writer.Flush();
                writer.Close();
            }
            catch
            {
                Program.PrintDebugMessage("Could not open the configuration file for writing.");
            }
        }
    }
}
