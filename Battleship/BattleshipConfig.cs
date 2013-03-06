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
     * <summary>BattleshipConfig deals with a single configuration file and parses each key/value pair.
     * Do not use this class to load a single key many times; it is probably slow.</summary>
     */
    public class BattleshipConfig
    {
        private Dictionary<string, string> simpleConfig;
        private Dictionary<string, Type> loadedRobots = new Dictionary<string, Type>();
        private string rootPath;
        private static BattleshipConfig globalInstance;

        /**
         * <summary>Gets a common configuration object for use. The default configuration file is loaded at \..\config.ini</summary>
         */
        public static BattleshipConfig GetGlobalDefault()
        {
            if(globalInstance != null)
                globalInstance = new BattleshipConfig(Environment.CurrentDirectory + "\\..\\config.ini");
            return globalInstance;
        }

        public object GetDataSourceObject()
        {
            return (from row in simpleConfig
                    select
                        new { Name = row.Key, Value = row.Value }).ToArray();
        }

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
         * <returns>The value loaded from the configuration, otherwise, the default value.</returns>
         */
        public T GetConfigValue<T>(string s, T def)
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
         * <summary>Sets a value in the configuration</summary>
         */
        public void SetConfigValue<T>(string s, T val)
        {
            simpleConfig[s] = val.ToString();
        }

        /**
         * <summary>Gets a comma-delimited array (list) from the configuration</summary>
         * <param name="s">The key to get the value from</param>
         * <param name="def">The default value for a given key if the key is not found.</param>
         * <returns>The list loaded from the configuration, otherwise, the default value</returns>
         */
        public List<T> GetConfigValueArray<T>(string s, string def)
        {
            List<T> vals = new List<T>();
            try
            {
                foreach (string val in GetConfigValue<string>(s, def).Split(','))
                    vals.Add((T)Convert.ChangeType(val.Trim(), typeof(T)));
            }
            catch
            {
                foreach (string val in def.Split(','))
                    vals.Add((T)Convert.ChangeType(val.Trim(), typeof(T)));
            }
            return vals;
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
