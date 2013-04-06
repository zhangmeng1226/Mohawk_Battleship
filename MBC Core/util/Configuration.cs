using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using System.Reflection;
using MBC.Core;

namespace MBC.Core
{
    /**
     * <summary>BattleshipConfig deals with a single configuration file and parses each key/value pair.</summary>
     */
    public class Configuration
    {
        private Dictionary<string, string> simpleConfig;
        private string configName;

        private static string configsPath;
        private static Configuration globalInstance;
        private static Configuration defaultInstance;

        static Configuration()
        {
            configsPath = Util.WorkingDirectory() + "configs\\";
            defaultInstance = new Configuration("default");
            globalInstance = new Configuration("config");
        }

        /**
         * <summary>Gets a common configuration object for use.</summary>
         */
        public static Configuration Global
        {
            get { return globalInstance; }
        }

        /**
         * <summary>Gets the default configuration. Used to set the default values for the 
         */
        public static Configuration Default
        {
            get { return defaultInstance; }
        }

        /**
         * <returns>The names of existing configuration files</returns>
         */
        public static List<string> GetAvailableConfigs()
        {
            string[] absFiles = Directory.GetFiles(Util.WorkingDirectory() + "configs", "*.ini");
            List<string> res = new List<string>();
            foreach (string f in absFiles)
            {
                string[] spl = f.Split('\\');
                res.Add(spl[spl.Length].Split('.')[0]);
            }
            return res;
        }

        /**
         * <summary>Initializes this BattleshipConfig by loading from a config file. If it does not
         * exist, then it loads the default values.</summary>
         */
        public Configuration(string name)
        {
            simpleConfig = new Dictionary<string, string>();
            configName = name;
            if(name != "default")
                LoadConfigFile();
        }

        /**
         * <summary>Renames this configuration file for file input or output</summary>
         */
        public void Rename(string newName)
        {
            configName = newName;
        }

        /**
         * <summary>Gets a value from the configuration</summary>
         * <param name="s">The key to get the value from</param>
         * <param name="def">The default value for a given key if the key is not found.</param>
         * <returns>The value loaded from the configuration, otherwise, the default value.</returns>
         */
        public T GetConfigValue<T>(string s)
        {
            try
            {
                return (T)Convert.ChangeType(simpleConfig[s], typeof(T));
            }
            catch(KeyNotFoundException e)
            {
                if (configName == "default")
                    throw e;
                return defaultInstance.GetConfigValue<T>(s);
            }
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
        public List<T> GetConfigValueArray<T>(string s)
        {
            List<T> vals = new List<T>();
            try
            {
                foreach (string val in GetConfigValue<string>(s).Split(','))
                    vals.Add((T)Convert.ChangeType(val.Trim(), typeof(T)));
            }
            catch
            {
                vals.Clear();
                foreach (string val in defaultInstance.GetConfigValue<string>(s).Split(','))
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
                StreamReader reader = new StreamReader(configsPath + configName + ".ini");
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
                Util.PrintDebugMessage("Could not load the configuration file " + configName + "for reading. Loading defaults.");
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
                StreamWriter writer = new StreamWriter(configsPath + configName + ".ini", false);
                foreach (KeyValuePair<string, string> entry in simpleConfig)
                    writer.WriteLine(entry.Key + " = " + entry.Value);
                writer.Flush();
                writer.Close();
            }
            catch
            {
                Util.PrintDebugMessage("Could not open the configuration file for writing.");
            }
        }
    }
}
