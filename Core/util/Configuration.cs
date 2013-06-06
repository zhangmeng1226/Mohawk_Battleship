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
    /// <summary>
    /// Configuration deals with a single configuration file and parses each key/value pair.
    /// 
    /// Use the static variable Configuration.Global to access a configuration that represents the entirety
    /// of the application. This global configuration is loaded through the file named "config.ini" in the config
    /// directory of the root of the project directory.
    /// 
    /// Use the static variable Configuration.Default to access a Configuration that represents all default
    /// values of the application. This values should be utilized before using the default Configuration.
    /// The default values should not be changed after startup. Avoid using static constructors to set default
    /// values as the values may not be set before using the default object, which results in errors. Use
    /// LoadConfigurationDefaults() to invoke all the static methods named "SetConfigDefaults()" in each class,
    /// where the default Configuration should be loaded.
    /// 
    /// Usage of the Configuration class is simple. Use two/three methods to get and set configuration values:
    /// <list type="bullet">
    ///     <item>
    ///         SetValue() - Sets the key value with the given value.
    ///     </item>
    ///     <item>
    ///         GetValue() - Returns the value at the given key string.
    ///     </item>
    ///     <item>
    ///         GetList() - Returns a list of the given type from the given key. Use when setting comma-separated values.
    ///     </item>
    /// </list>
    /// A list of Configurations stored into files can be known by using the GetAvailableConfigs() class member.
    /// 
    /// When constructing a new Configuration object, the Configuration will be loaded with either default values,
    /// or values that have been stored in the correspondingly named file.
    /// 
    /// Note that configuration files are not automatically saved. Invoke the method SaveConfigFile() to
    /// save Configuration objects to files.
    /// 
    /// 
    /// 
    /// <remarks>
    ///     To set default values for the application, implement the following method in the class to have it
    ///     invoked when LoadConfigurationDefaults() is called.
    /// </remarks>
    /// <code>
    ///     public static void SetConfigDefaults()
    ///     {
    ///         //Set Configuration defaults here.
    ///         Configuration.Default.SetValue<string>("config_key", "config_value");
    ///         Configuration.Default.SetValue<int>("number_of_init_hits", 50);
    ///         
    ///         //Example of setting an array/list using CSV:
    ///         Configuration.Default.SetValue<string>("ships_array", "3,3,4,5,5,6");
    ///     }
    /// </code>
    /// As a convention, use underscores to separate words in the key string as shown above.
    /// 
    /// <remarks>Here is an example of a typical use of the global configuration:</remarks>
    /// <code>
    ///     {
    ///         int initHits = Configuration.Global.GetValue<int>("number_of_init_hits");
    ///          
    ///         //Getting the ships_array list as set above.
    ///         List<int> shipArray = Configuration.Global.GetList<int>("ships_array");
    ///     }
    /// </code>
    /// 
    /// <remarks>More advanced uses of the Configuration file can be seen below:</remarks>
    /// <code>
    ///     {
    ///         Configuration conf = new Configuration("newconfig"); //Uses default values initially.
    ///         conf.SetValue<int>("rounds_temp_value", 40); //Overwrites default value (if exists), or creates new key with the value.
    ///          
    ///         conf.SaveConfigFile(); //Saves to newconfig.ini
    ///          
    ///         conf.Rename("newconfig245");
    ///          
    ///         conf.SaveConfigFile(); //Saves to newconfig245.ini
    ///          
    ///         conf = new Configuration("newconfig"); //Loading back the saved newconfig.ini configuration file.
    ///     }
    /// </code>
    /// </summary>
    public class Configuration
    {
        private Dictionary<string, object> simpleConfig;
        private string configName;

        private static string configsPath;
        private static Configuration globalInstance;
        private static Configuration defaultInstance;

        static Configuration()
        {
            //Initializes some global Configuration properties.
            configsPath = Utility.WorkingDirectory() + "configs\\";
            defaultInstance = new Configuration("default");
            globalInstance = new Configuration("config");
            if (!Directory.Exists(configsPath))
                Directory.CreateDirectory(configsPath);
        }

        
        /// <summary>
        /// Gets a common Configuration object for use.
        /// </summary>
        public static Configuration Global
        {
            get { return globalInstance; }
        }

        /// <summary>
        /// Gets the default Configuration instance for this application.
        /// </summary>
        public static Configuration Default
        {
            get { return defaultInstance; }
        }

        
        /// <summary>
        /// Call at the beginning of an application to invoke all the static methods that match
        /// the name "SetConfigDefaults". This will allow classes to set the default configuration values
        /// into the default Configuration object at startup.
        /// </summary>
        public static void LoadConfigurationDefaults()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var thisAssembly in assemblies)
            {
                foreach (var type in thisAssembly.GetTypes())
                {
                    foreach (var method in type.GetMethods())
                    {
                        if (method.Name == "SetConfigDefaults")
                        {
                            method.Invoke(null, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates a List of strings containing the names of the files that are contained
        /// in the configuration folder.
        /// </summary>
        /// <returns>
        /// The names of existing configuration files
        /// </returns>
        public static List<string> GetAvailableConfigs()
        {
            string[] absFiles = Directory.GetFiles(Utility.WorkingDirectory() + "configs", "*.ini");
            var res = new List<string>();
            foreach (var f in absFiles)
            {
                string[] spl = f.Split('\\');
                res.Add(spl[spl.Length].Split('.')[0]);
            }
            return res;
        }

        /// <summary>
        /// Initializes this Configuration by loading from a configuration file named by the given string. If it does not
        /// exist, then getting values from this Configuration will get the Configuration.Default values as needed.
        /// </summary>
        public Configuration(string name)
        {
            simpleConfig = new Dictionary<string, object>();
            configName = name;
            if (name != "default")
            {
                LoadConfigFile();
            }
        }
        /// <summary>
        /// Renames this Configuration object; effectively changing the name of the file this
        /// Configuration will be changed to.
        /// </summary>
        public void Rename(string newName)
        {
            configName = newName;
        }

        /// <summary>Gets a value from the configuration</summary>
        /// <param name="key">The key to get the value from</param>
        /// <returns>The value loaded from the configuration, otherwise, the default value.</returns>
        /// <exception cref="KeyNotFoundException">The given key has no value set.</exception>
        /// <exception cref="InvalidCastException">The generic type specified was not the type set in the Configuration.</exception>
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
                throw new KeyNotFoundException("The following Configuration key was not set: "+key);
            }
        }

        /// <summary>
        /// Sets a value in the configuration using the provided key and value.
        /// To store lists, provide comma-separated values as a string for the value. Overwrites the
        /// previous value identified by the given key.
        /// </summary>
        /// <param name="key">The key that identifies this value.</param>
        /// <param name="val">The value linked to the given key.</param>
        public void SetValue<T>(string key, T val)
        {
            simpleConfig[key] = val;
        }

        /// <summary>
        /// Gets a comma-delimited array (list) from the configuration. This value must be formatted
        /// as comma-separated values, as a string in the Configuration.
        /// </summary>
        /// <param name="key">The key to get the value from</param>
        /// <returns>The list loaded from the configuration, otherwise, the default value</returns>
        /// <example>
        /// For a list of ints;      1,5,6,6,15545,6445,9<br/>
        /// For a list of strings;   hello,goodbye,thank you,whenever it is time.,null<br/>
        /// For a list of doubles;   15.5,14.2,0.000096<br/>
        /// etc.<br/>
        /// Be sure that the values themselves do not contain commas, as they will be split in the process.
        /// </example>
        public List<T> GetList<T>(string key)
        {
            var vals = new List<T>();
            var csvStringSplit = GetValue<string>(key).Split(',');
            foreach (var csvToken in csvStringSplit)
            {
                vals.Add((T)Convert.ChangeType(csvToken, typeof(T)));
            }
            return vals;
        }

        /// <seealso cref="GetList"/>
        [Obsolete("GetConfigValueArray() has been renamed to GetList(), as it is much shorter to write.")]
        public List<T> GetConfigValueArray<T>(string key)
        {
            return GetList<T>(key);
        }

        /// <summary>Loads a configuration file from the configuration folder.</summary>
        private void LoadConfigFile()
        {
            var errorLineNum = 0;
            try
            {
                var reader = new StreamReader(configsPath + configName + ".ini");
                do
                {
                    var line = reader.ReadLine();
                    var tLine = line.Split('=');
                    tLine[0] = tLine[0].Trim();
                    tLine[1] = tLine[1].Trim();

                    int intVal = 0;
                    double doubVal = 0d;

                    if (int.TryParse(tLine[1], out intVal) || double.TryParse(tLine[1], out doubVal))
                    {
                        if (intVal == doubVal)
                        {
                            //Must be integer (no decimal point)
                            simpleConfig.Add(tLine[0], intVal);
                        }
                        else
                        {
                            //Must be double (has decimal point)
                            simpleConfig.Add(tLine[0], doubVal);
                        }
                    }
                    else
                    {
                        //Must be string (not a number)
                        simpleConfig.Add(tLine[0], tLine[1]);
                    }
                    errorLineNum++;
                } while (reader.Peek() != -1);
                reader.Close();
            }
            catch (IOException)
            {
                Utility.PrintDebugMessage("Could not load the configuration file " + configName + ". Default values will be used.");
            }
            catch
            {
                Utility.PrintDebugMessage("There was an error while parsing the configuration file on line " + errorLineNum);
            }
        }

        /// <summary>Saves the current configuration to a file.</summary>
        /// <exception cref="IOException">The file could not be written to.</exception>
        public void SaveConfigFile()
        {
            try
            {
                var writer = new StreamWriter(configsPath + configName + ".ini", false);
                foreach (var entry in simpleConfig)
                {
                    writer.WriteLine(entry.Key + " = " + entry.Value.ToString());
                }
                writer.Flush();
                writer.Close();
            }
            catch
            {
                Utility.PrintDebugMessage("Could not open the configuration file for writing.");
            }
        }
    }
}
