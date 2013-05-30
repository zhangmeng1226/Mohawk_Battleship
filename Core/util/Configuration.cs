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
     * <summary>BattleshipConfig deals with a single configuration file and parses each key/value pair.
     * 
     * Use the static variable Configuration.Global to access a configuration that represents the entirety
     * of the application. This global configuration is loaded through the file named "config.ini" in the config
     * directory of the root of the project directory.
     * 
     * Use the static variable Configuration. Default to access a configuration that represents all default
     * values of the application. This should be utilized before using the default Configuration.
     * The default values should not be changed after startup. Avoid using static constructors to set default
     * values as the values may not be set before using the default object, which results in errors.
     * 
     * Usage of the Configuration class is simple. Use two/three methods to get and set configuration values:
     *      SetValue<type>(string key, type val) - Sets the key value with the given value.
     *      GetValue<type>(string key) - Returns the value at the given key string.
     *      GetConfigValueArray<type>(string key) - Returns a list of the given type from the given key.
     *                                              Use when setting comma-separated values.
     * 
     * A list of Configurations stored into files can be known by using the GetAvailableConfigs() class member.
     * 
     * When constructing a new Configuration object, the Configuration will be loaded with either default values,
     * or values that have been stored in the correspondingly named file.
     * 
     * Note that configuration files are not automatically saved. Invoke the method SaveConfigFile() to
     * accomplish this.
     * </summary>
     */
    public class Configuration
    {
        private Dictionary<string, string> simpleConfig;
        private string configName;

        public static string configsPath;
        private static Configuration globalInstance;
        private static Configuration defaultInstance;

        static Configuration()
        {
            configsPath = Util.WorkingDirectory() + "configs\\";
            defaultInstance = new Configuration("default");
            globalInstance = new Configuration("config");
            if (!Directory.Exists(configsPath))
                Directory.CreateDirectory(configsPath);
        }

        /**
         * <summary>Gets a common configuration object for use.</summary>
         */
        public static Configuration Global
        {
            get { return globalInstance; }
        }

        /**
         * <summary>Gets the default configuration.</summary>
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
         * <param name="key">The key to get the value from</param>
         * <param name="def">The default value for a given key if the key is not found.</param>
         * <returns>The value loaded from the configuration, otherwise, the default value.</returns>
         */
        public T GetValue<T>(string key)
        {
            try
            {
                return (T)Convert.ChangeType(simpleConfig[key], typeof(T));
            }
            catch(KeyNotFoundException e)
            {
                if (configName == "default")
                    throw e;
                return defaultInstance.GetValue<T>(key);
            }
        }

        /**
         * <summary>Sets a value in the configuration</summary>
         */
        public void SetValue<T>(string key, T val)
        {
            simpleConfig[key] = val.ToString();
        }

        /**
         * <summary>Gets a comma-delimited array (list) from the configuration</summary>
         * <param name="key">The key to get the value from</param>
         * <param name="def">The default value for a given key if the key is not found.</param>
         * <returns>The list loaded from the configuration, otherwise, the default value</returns>
         */
        public List<T> GetConfigValueArray<T>(string key)
        {
            List<T> vals = new List<T>();
            try
            {
                foreach (string val in GetValue<string>(key).Split(','))
                    vals.Add((T)Convert.ChangeType(val.Trim(), typeof(T)));
            }
            catch
            {
                vals.Clear();
                foreach (string val in defaultInstance.GetValue<string>(key).Split(','))
                    vals.Add((T)Convert.ChangeType(val.Trim(), typeof(T)));
            }
            return vals;
        }

        /**
         * <summary>Loads a delimited configuration file, delimited by '=', and stores each key and value.</summary>
         */
        private void LoadConfigFile()
        {
            int eLine = 0;
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
                    eLine++;
                } while (reader.Peek() != -1);
                reader.Close();
            }
            catch (IOException)
            {
                Util.PrintDebugMessage("Could not load the configuration file " + configName + ". Default values will be used.");
            }
            catch
            {
                Util.PrintDebugMessage("There was an error while parsing the configuration file on line "+eLine);
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
