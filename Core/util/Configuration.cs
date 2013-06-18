using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MBC.Core.Util
{
    /// <summary>
    /// Stores <c>bool</c>s, <c>long</c>s, <c>double</c>s, and <c>string</c>s each identified by a unique
    /// string. Provides a <see cref="Configuration.Default"/> instance that indicates the values set
    /// compile-time via <see cref="ConfigurationAttribute"/>s present in the application assembly. The
    /// types of the values set at compile time cannot be changed. Provides functionality for loading
    /// and saving the keys and values to a file with the same name as given to the constructor. Values
    /// that differ from <see cref="Configuration.Default"/> are not initially entered. Values that
    /// do not exist will then be checked in the <see cref="Configuration.Default"/> configuration.
    /// The static functionality of the configuration must be initialized before being used with a
    /// call to <see cref="Configuration.InitializeConfiguration(string)"/>; this provides the folder path
    /// where configuration files are kept and initializes the static usage of a Configuration.
    /// </summary>
    public class Configuration
    {
        private static string configsPath;
        private static Configuration defaultInstance;
        private static Configuration globalInstance;
        private string configName;
        private SortedDictionary<string, object> simpleConfig;

        /// <summary>
        /// Initializes with the given name and attempts to load a file with the same name.
        /// </summary>
        public Configuration(string name)
        {
            simpleConfig = new SortedDictionary<string, object>();
            configName = name;
            if (name != "default")
            {
                LoadConfigFile();
            }
        }

        /// <summary>
        /// Gets a <see cref="Configuration"/> that contains the default values set at compile-time
        /// </summary>
        public static Configuration Default
        {
            get { return defaultInstance; }
        }

        /// <summary>
        /// Gets or sets a <see cref="Configuration"/> that may be accessed statically throughout an
        /// application. Attempting to set to null will cause nothing to change.
        /// </summary>
        public static Configuration Global
        {
            get { return globalInstance; }
            set
            {
                if (value != null)
                {
                    globalInstance = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name that is used to save or load from a config file.
        /// </summary>
        public string Name
        {
            get
            {
                return configName;
            }
        }

        /// <summary>
        /// Generates a List of strings containing the names of the files that are contained
        /// in the defined configuration folder.
        /// </summary>
        /// <returns>
        /// The names of existing configuration files.
        /// </returns>
        public static IEnumerable<string> GetAvailableConfigs()
        {
            string[] absFiles = Directory.GetFiles(configsPath, "*.ini");
            var res = new List<string>();
            foreach (var f in absFiles)
            {
                string[] spl = f.Split('\\');
                res.Add(spl[spl.Length].Split('.')[0]);
            }
            return res;
        }

        /// <summary>
        /// Initializes the static functionality of the <see cref="Configuration"/> with the path
        /// to where configuration files are to be stored. Must be called before using <see cref="Configuration"/>.
        /// </summary>
        /// <param name="confPath">A string to the absolute path of the directory to save config files in.</param>
        public static void InitializeConfiguration(string confPath)
        {
            //Initializes some global Configuration properties.
            configsPath = confPath;
            if (!Directory.Exists(configsPath))
                Directory.CreateDirectory(configsPath);
            defaultInstance = new Configuration("default");
            globalInstance = new Configuration("config");
            LoadConfigurationDefaults();
        }

        /// <summary>
        /// Converts a string into an integral type that may be placed into a <see cref="Configuration"/>.
        /// Attempts to convert to these values in this order:
        /// <list type="number">
        /// <item>bool</item>
        /// <item>long</item>
        /// <item>double</item>
        /// </list>
        /// After unsuccessfully parsing to one of the aforementioned types, it will simply store the string.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <returns>An object as described in the summary.</returns>
        public static object ParseString(string value)
        {
            bool boolVal;
            long integerVal;
            double decimalVal;
            if (bool.TryParse(value, out boolVal))
            {
                return boolVal;
            }
            else if (long.TryParse(value, out integerVal))
            {
                return integerVal;
            }
            else if (double.TryParse(value, out decimalVal))
            {
                return decimalVal;
            }
            return value;
        }

        /// <summary>Gets a single value stored as the given <paramref name="key"/> with the specified type. If the
        /// <paramref name="key"/> does not exist, the <see cref="Configuration.Default"/> will be checked for the given
        /// <paramref name="key"/>.</summary>
        /// <param name="key">The key that a sought value is associated with.</param>
        /// <typeparam name="T">The type of value to cast the object value as.</typeparam>
        /// <returns>A value associated with the given <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">The given <paramref name="key"/> has no value set.</exception>
        /// <exception cref="InvalidCastException">The generic type specified was not the type set at compile-time.</exception>
        public T GetValue<T>(string key)
        {
            if (simpleConfig.ContainsKey(key))
            {
                return (T)simpleConfig[key];
            }
            else if (configName != "default")
            {
                return Default.GetValue<T>(key);
            }
            else
            {
                throw new KeyNotFoundException("The following Configuration key was not set: " + key);
            }
        }

        /// <summary>
        /// Attempts to parse a string containing commas to a <see cref="List"/> of values. If no
        /// commas are present, this <see cref="List"/> will contain only one element.
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        /// <typeparam name="T">The type of <see cref="List"/> to create and attempt to parse
        /// the string for.</typeparam>
        /// <returns>The parsed list of a given type.</returns>
        /// <example>
        /// <para>For a list of ints;      1,5,6,6,15545,6445,9</para>
        /// <para>For a list of strings;   hello,goodbye,thank you,whenever it is time.,null</para>
        /// <para>For a list of doubles;   15.5,14.2,0.000096</para>
        /// <para>etc.</para>
        /// <remarks>Be aware that because commas are used to separate the values. If a list of
        /// strings are used, this may cause problems if the commas are stored in the strings.</remarks>
        /// </example>
        public List<T> GetList<T>(string key)
        {
            var vals = new List<T>();
            var value = GetValue<object>(key);
            if (value.GetType() != typeof(string))
            {
                vals.Add((T)value);
            }
            else
            {
                foreach (var csvToken in ((string)value).Split(','))
                {
                    vals.Add((T)Convert.ChangeType(csvToken.Trim(), typeof(T)));
                }
            }
            return vals;
        }

        /// <summary>
        /// Parses a given value from a string through <see cref="Configuration.ParseString(string)"/> and
        /// ensures the type is consistent with the <see cref="Configuration.Default"/> if it exists. Sets
        /// the value to the given key if successful.
        /// </summary>
        /// <param name="key">The key associated with the value.</param>
        /// <param name="val">The string representation of a value.</param>
        public bool SetValue(string key, string val)
        {
            object newValue = ParseString(val);

            if (defaultInstance.simpleConfig.Keys.Contains(key) && newValue.GetType() != defaultInstance.simpleConfig[key].GetType())
            {
                return false;
            }

            simpleConfig[key] = newValue;
            return true;
        }

        /// <summary>
        /// Checks if a file with the same name exists in the configuration folder.
        /// </summary>
        /// <returns>A value indicating whether the path exists or not.</returns>
        public bool FileExists()
        {
            return File.Exists(configsPath + "\\" + configName + ".ini");
        }

        /// <seealso cref="GetList"/>
        [Obsolete("GetConfigValueArray() has been renamed to GetList(), as it is much shorter to write.")]
        public List<T> GetConfigValueArray<T>(string key)
        {
            return GetList<T>(key);
        }
        /// <summary>
        /// Returns a copies list all of the <see cref="KeyValuePair"/>s in the <see cref="Dictionary"/>.
        /// Does not include <see cref="Configuration.Default"/> keys and values.
        /// </summary>
        /// <returns>A copies list of <see cref="KeyValuePair"/>s.</returns>
        public List<KeyValuePair<string, object>> GetPairs()
        {
            var mergedConfigs = new Dictionary<string, object>(defaultInstance.simpleConfig);
            foreach (var configPairs in simpleConfig)
            {
                mergedConfigs[configPairs.Key] = configPairs.Value;
            }
            return mergedConfigs.ToList();
        }
        /// <summary>Saves the keys and values that differ from <see cref="Configuration.Default"/>
        /// into a file in the configuration folder as <see cref="Configuration.Name"/>.ini.</summary>
        /// <exception cref="IOException">The file could not be written to.</exception>
        public void SaveConfigFile()
        {
            var writer = new StreamWriter(configsPath + "\\" + configName + ".ini", false);
            foreach (var entry in simpleConfig)
            {
                writer.WriteLine(entry.Key + " = " + entry.Value.ToString());
            }
            writer.Flush();
            writer.Close();
        }
        /// <summary>
        /// Searches all application assemblies for <see cref="ConfigurationAttribute"/>s and initializes
        /// the <see cref="Configuration.Default"/> instance with those keys and values found.
        /// </summary>
        private static void LoadConfigurationDefaults()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var thisAssembly in assemblies)
            {
                foreach (var classType in thisAssembly.GetTypes())
                {
                    object[] attribs = classType.GetCustomAttributes(typeof(ConfigurationAttribute), false);
                    foreach (ConfigurationAttribute defaultPair in attribs)
                    {
                        defaultInstance.simpleConfig[defaultPair.Key] = defaultPair.Value;
                    }
                }
            }
        }

        /// <summary>Loads a configuration file from the configuration folder.</summary>
        private void LoadConfigFile()
        {
            try
            {
                var reader = new StreamReader(configsPath + "\\" + configName + ".ini");
                do
                {
                    var line = reader.ReadLine();
                    var tLine = line.Split('=');
                    tLine[0] = tLine[0].Trim();
                    tLine[1] = tLine[1].Trim();

                    SetValue(tLine[0], tLine[1]);
                } while (reader.Peek() != -1);
                reader.Close();
            }
            catch
            {
                //We don't care, use default values from now on.
            }
        }
    }
}