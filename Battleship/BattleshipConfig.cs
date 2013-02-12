using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Battleship
{
    /**
     * <summary>BattleshipConfig deals with a single configuration file and parses each key/value pair</summary>
     */
    public class BattleshipConfig
    {
        private Dictionary<string, string> simpleConfig;
        private string absPath;

        /**
         * <summary>Initializes this BattleshipConfig by loading from a config file, if it exists</summary>
         */
        public BattleshipConfig(string pathname)
        {
            simpleConfig = new Dictionary<string, string>();
            absPath = pathname;
            LoadConfigFile();
        }

        /**
         * <summary>Gets a value from the configuration</summary>
         * <param name="s">The key to get the value from</param>
         * <param name="def">The default value for a given key if the key is not found.</param>
         * <exception cref="Exception">If the value could not be converted</exception>
         */
        private T ConfigValue<T>(string s, T def)
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
                StreamReader reader = new StreamReader(absPath);
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
                Console.WriteLine("Could not load configuration file for reading.");
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
                StreamWriter writer = new StreamWriter(absPath, false);
                foreach (KeyValuePair<string, string> entry in simpleConfig)
                    writer.WriteLine(entry.Key + " = " + entry.Value);
                writer.Flush();
                writer.Close();
            }
            catch
            {
                Console.WriteLine("Could not open the configuration file for writing.");
            }
        }
    }
}
