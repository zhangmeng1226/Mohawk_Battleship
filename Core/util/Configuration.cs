using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MBC.Core.Util
{
    /// <summary>
    /// <para>Stores a number of objects that are associated with a string. All objects that have a TypeConverter
    /// may be used, including but not limited to, <b>bool</b>s, <b>int</b>s <b>string</b>s <b>Enum</b>s etc.
    /// Features special support for arrays of any type of object through comma-separated-values in string
    /// values.
    /// </para>
    /// <para>Note that the values that are stored are restricted by the default value type from a given
    /// key. The type that has been made as default cannot be changed.</para>
    /// <para>
    /// All default values are loaded by reading in the <see cref="ConfigurationAttribute"/> metadata that is
    /// given for a class. This is done via a call to <see cref="Configuration.Initialize(string)"/>.</para>
    /// <para>
    /// The system must be initialized by a call to <see cref="Configuration.Initialize(string)"/> before
    /// any part of the system may be used.</para>
    /// </summary>
    public class Configuration
    {
        private static SortedDictionary<string, ConfigurationAttribute> compiledConfiguration;
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
            LoadConfigFile();
        }

        /// <summary>
        /// Creates an empty, unloaded <see cref="Configuration"/>. Used in
        /// <see cref="Configuration.Initialize(string)"/>.
        /// </summary>
        private Configuration()
        {
            simpleConfig = new SortedDictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets a <see cref="Configuration"/> that may be accessed statically throughout an
        /// application. Attempting to set to null will cause nothing to change.
        /// </summary>
        [XmlIgnore]
        public static Configuration Global
        {
            get
            {
                if (globalInstance == null)
                {
                    throw new InvalidOperationException("Configuration has not been initialized.");
                }
                return globalInstance;
            }
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
            set
            {
                configName = value;
            }
        }

        /// <summary>
        /// Adds a the <paramref name="defAttrib"/> as a default value in the <see cref="Configuration"/> system.
        /// </summary>
        /// <param name="defAttrib">The <see cref="ConfigurationAttribute"/> to add to the default collection.</param>
        /// <returns>A value indicating whether or not the <paramref name="defAttrib"/> was entered.
        /// true is returned if the key of the <paramref name="defAttrib"/> previously did not exist,
        /// and was added. false if the key did exist, and the <paramref name="defAttrib"/>
        /// was not added.</returns>
        public static bool AddDefault(ConfigurationAttribute defAttrib)
        {
            if (compiledConfiguration.ContainsKey(defAttrib.Key))
            {
                return false;
            }
            compiledConfiguration[defAttrib.Key] = defAttrib;
            return true;
        }

        /// <summary>
        /// Generates a list of strings that represents keys that have been defined by the application.
        /// </summary>
        /// <returns>A list of strings.</returns>
        public static List<string> GetAllKnownKeys()
        {
            var keys = new List<string>();
            foreach (var configs in compiledConfiguration)
            {
                keys.Add(configs.Key);
            }
            return keys;
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
            string[] absFiles = Directory.GetFiles(Global.GetValue<string>("app_data_root") +
                "config\\", "*.ini");
            var res = new List<string>();
            foreach (var f in absFiles)
            {
                string[] spl = f.Split('\\');
                res.Add(spl[spl.Length].Split('.')[0]);
            }
            return res;
        }

        /// <summary>
        /// Gets the default value from a <paramref name="key"/> in the <see cref="Configuration"/> system.
        /// </summary>
        /// <typeparam name="T">The expected type of the default value.</typeparam>
        /// <param name="key">A string that identifies the key of the value.</param>
        /// <returns>The default value.</returns>
        public static T GetDefaultValue<T>(string key)
        {
            return (T)compiledConfiguration[key].Value;
        }

        /// <summary>
        /// Gets the string from a <see cref="ConfigurationAttribute"/> associated with the <paramref name="key"/> that provides
        /// a description.
        /// </summary>
        /// <param name="key">The string key to look up</param>
        /// <returns>A string. If <paramref name="key"/> was not found, null.</returns>
        public static string GetDescription(string key)
        {
            if (compiledConfiguration.ContainsKey(key))
            {
                return compiledConfiguration[key].Description;
            }
            return null;
        }

        /// <summary>
        /// Gets the string from a <see cref="ConfigurationAttribute"/> associated with the <paramref name="key"/> that provides
        /// the name used to show the user.
        /// </summary>
        /// <param name="key">The key string to look up</param>
        /// <returns>A string. If <paramref name="key"/> was not found, null.</returns>
        public static string GetDisplayName(string key)
        {
            if (compiledConfiguration.ContainsKey(key))
            {
                return compiledConfiguration[key].DisplayName;
            }
            return null;
        }

        /// <summary>
        /// Initializes the <see cref="Configuration"/> system by reading <see cref="ConfigurationAttribute"/>
        /// meta-data in all assemblies. Sets the default application data path to <paramref name="appDataPath"/>.
        /// </summary>
        /// <param name="appDataPath">The absolute path to the desired application path.</param>
        public static void Initialize(string appDataPath)
        {
            compiledConfiguration = new SortedDictionary<string, ConfigurationAttribute>();
            LoadConfigurationDefaults();

            if (appDataPath.Last() != '\\')
            {
                appDataPath += '\\';
            }

            //Special case needs to be made for this certain default value (must be created at runtime).
            var rootAttrib = new ConfigurationAttribute("app_data_root", appDataPath);
            rootAttrib.DisplayName = "Data Root";
            rootAttrib.Description = "The absolute folder path of the root directory that will contain saved files.";
            compiledConfiguration.Add(rootAttrib.Key, rootAttrib);

            //Constructing the global instance.
            globalInstance = new Configuration();
            globalInstance.Name = "global";
            globalInstance.LoadConfigFile();
        }

        /// <summary>
        /// Converts a <paramref name="value"/> string to the value of the type <paramref name="valueType"/> using
        /// its TypeConverter.
        /// </summary>
        /// <param name="valueType">The Type of the object to convert the <paramref name="value"/> to.</param>
        /// <param name="value">The string representation of a value to parse.</param>
        /// <returns>A non-null object.</returns>
        /// <exception cref="Exception">Thrown if the conversion failed.</exception>
        public static object ParseString(Type valueType, string value)
        {
            var converter = TypeDescriptor.GetConverter(valueType);
            return converter.ConvertFromString(value);
        }

        /// <summary>
        /// Checks if a file with the same name exists in the configuration folder.
        /// </summary>
        /// <returns>A value indicating whether the path exists or not.</returns>
        public bool FileExists()
        {
            return File.Exists(GetValue<string>("app_data_root") + "config\\" + configName + ".ini");
        }

        /// <seealso cref="GetList"/>
        [Obsolete("GetConfigValueArray() has been renamed to GetList(), as it is much shorter to write.")]
        public List<T> GetConfigValueArray<T>(string key)
        {
            return GetList<T>(key);
        }

        /// <summary>
        /// Attempts to parse a string containing commas to a list of values. If no
        /// commas are present, this list will contain only one element.
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        /// <typeparam name="T">The type of list to create and attempt to parse
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
            var getValues = new List<T>();
            var configValues = GetValue<List<object>>(key);
            foreach (var val in configValues)
            {
                getValues.Add((T)val);
            }
            return getValues;
        }

        /// <summary>
        /// Returns a copied list all of the KeyValuePairs of keys and values differing from the
        /// default values.
        /// </summary>
        /// <returns>A copied list of KeyValuePairs.</returns>
        public List<KeyValuePair<string, object>> GetPairs()
        {
            return simpleConfig.ToList();
        }

        /// <summary>Gets a single value stored as the given <paramref name="key"/> with the specified type. If the
        /// <paramref name="key"/> does not exist, the default value will be given.</summary>
        /// <param name="key">The key that a sought value is associated with.</param>
        /// <typeparam name="T">The type of value to cast the object value as.</typeparam>
        /// <returns>A value associated with the given <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">The given <paramref name="key"/> has no value set.</exception>
        /// <exception cref="InvalidCastException">Type mismatch between the requested value and actual value.</exception>
        public T GetValue<T>(string key)
        {
            if (simpleConfig.ContainsKey(key))
            {
                return (T)simpleConfig[key];
            }
            return (T)compiledConfiguration[key].Value;
        }

        /// <summary>
        /// Generates a string representation of the value in the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The string of the key to look up.</param>
        /// <returns>The string representation of the value associated with <paramref name="key"/>.</returns>
        public string GetValueString(string key)
        {
            object result = GetValue<object>(key);
            if (result.GetType() == typeof(List<object>))
            {
                StringBuilder str = new StringBuilder();
                var objList = (List<object>)result;
                str.Append(objList[0].ToString());
                for (var i = 1; i < objList.Count; i++)
                {
                    str.Append(',');
                    str.Append(objList[i]);
                }
                return str.ToString();
            }
            return result.ToString();
        }

        /// <summary>Saves the keys and values that differ from default values
        /// into a file in the configuration folder as <see cref="Configuration.Name"/>.ini.</summary>
        /// <exception cref="IOException">The file could not be written to.</exception>
        public void SaveConfigFile()
        {
            var writer = new StreamWriter(GetValue<string>("app_data_root") + "config\\"
                + configName + ".ini", false);
            foreach (var entry in simpleConfig)
            {
                writer.WriteLine(entry.Key + " = " + entry.Value.ToString());
            }
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Parses a given value from a string through <see cref="Configuration.ParseString(Type, string)"/> and
        /// ensures the type is consistent with the default value if it exists. Sets
        /// the value to the given key if successful.
        /// </summary>
        /// <param name="key">The key associated with the value.</param>
        /// <param name="val">The string representation of a value.</param>
        /// <exception cref="Exception">Thrown if the given string value type did not match the default
        /// value type.</exception>
        public void SetValue(string key, string val)
        {
            var compiledType = compiledConfiguration[key].Value.GetType();
            if (compiledType == typeof(List<object>))
            {
                compiledType = ((List<object>)compiledConfiguration[key].Value)[0].GetType();
                var newList = new List<object>();
                var valSplit = val.Split(',');
                foreach (var valToken in valSplit)
                {
                    object parsedToken = ParseString(compiledType, valToken.Trim());
                    if (parsedToken.GetType() != compiledType)
                    {
                        throw new InvalidDataException("A value in the parsed string list did not have a matched type.");
                    }
                    newList.Add(parsedToken);
                }
                simpleConfig[key] = newList;
            }
            else
            {
                simpleConfig[key] = ParseString(compiledType, val);
            }
        }

        /// <summary>
        /// Searches all application assemblies for <see cref="ConfigurationAttribute"/>s and fills the
        /// internal compiled attributes table with them.
        /// </summary>
        private static void LoadConfigurationDefaults()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var thisAssembly in assemblies)
            {
                foreach (var classType in thisAssembly.GetTypes())
                {
                    object[] attribs = classType.GetCustomAttributes(typeof(ConfigurationAttribute), false);
                    foreach (ConfigurationAttribute defaultAttrib in attribs)
                    {
                        AddDefault(defaultAttrib);
                    }
                }
            }
        }

        /// <summary>
        /// Loads a configuration file from the "app_data_root" + "app_config_root" values.
        /// Creates a directory if the path does not exist.
        /// </summary>
        private void LoadConfigFile()
        {
            var configRootPath = Global.GetValue<string>("app_data_root") + "config\\";
            if (!Directory.Exists(configRootPath))
            {
                Directory.CreateDirectory(configRootPath);
            }
            try
            {
                var reader = new StreamReader(configRootPath + configName + ".ini");
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