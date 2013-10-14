using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MBC.Core.Util
{
    /// <summary> Maintains multiple associations between an object and a string. This class provides an application-wide configuration that is easily accessible and
    /// useable. </summary>
    /// <remarks> <para>All objects that have a TypeConverter may be used; including but not limited to <b>bool</b>s, <b>int</b>s <b>string</b>s <b>Enum</b>s etc. Features support
    /// for arrays of any type of object through comma-separated-values in string values.</para>
    /// <para>Any value modifications are restricted to the original value type that has been loaded at application startup.</para>
    /// <para>All default values are loaded at application startup by reading in the <see cref="ConfigurationAttribute"/> metadata that is set upon a class.</para>
    /// <para>The configuration must be initialized by a call to <see cref="Configuration.Initialize(string)"/> before any part of the it may be used.</para>
    /// </remarks>
    /// <example>
    /// <para>To set a default value association, refer to the documentation in <see cref="ConfigurationAttribute"/>. Below is an example of complete usage of the class.</para>
    /// <code title="C#" description="An application utilizing the basic features of the configuration class." lang="C#">
    /// [Configuration("wait_seconds", 5)]
    /// [Configuration("text_print", "Demo text")]
    /// [Configuration("text_color", ConsoleColor.Red)]
    /// [Configuration("custom_var", "something?")]
    /// class Application
    /// {
    ///
    ///     static void main(string[] args)
    ///     {
    ///         Configuration.Initialize(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "ApplicationData");
    ///
    ///         var globalConf = Configuration.Global;
    ///         var secondCounter = globalConf.GetValue&lt;int&gt;("wait_seconds");
    ///
    ///         Console.ForegroundColor = globalConf.GetValue&lt;ConsoleColor&gt;("text_color");
    ///
    ///         while(secondCounter-- &gt; 0)
    ///         {
    ///             Console.WriteLine(globalConf.GetValue&lt;string&gt;("text_print"));
    ///             Thread.sleep(secondCounter * 1000);
    ///         }
    ///         globalConf.SetValue("custom_var", Console.ReadLine());
    ///         Console.WriteLine("You set 'custom_var' to "+globalConf.GetValue&lt;string&gt;("custom_var"));
    ///     }
    /// }</code> </example>
    public class Configuration
    {
        /// <summary>
        /// This dictionary contains all of the original <see cref="ConfigurationAttribute"/>s loaded through <see cref="AddDefault(ConfigurationAttribute)"/>.
        /// </summary>
        private static Dictionary<string, ConfigurationAttribute> compiledConfiguration;

        /// <summary>
        /// This variable holds the global instance, accessed by the <see cref="Global">Global property</see>.
        /// </summary>
        private static Configuration globalInstance;

        /// <summary>
        /// This variable contains the name of the configuration.
        /// </summary>
        private string configName;

        /// <summary>
        /// This dictionary contains all of the values/objects differing from the default association. It may not hold *all* associations.
        /// </summary>
        private Dictionary<string, object> simpleConfig;

        /// <summary>Initializes with the given name and attempts to load a file with the same name. If no file could be loaded, the application-wide default values will be used
        /// until overridden.</summary>
        /// <param name="name">The name to give the configuration. This name is also used to determine the file name to load from or save to.</param>
        /// <example>
        /// To create separate configurations, the use of creating multiple configurations will be necessary.
        /// <code title="C#" description="Demonstrating the use of multiple configurations to define application behaviour." lang="C#">
        /// {
        ///     //Let us assume that the default value for the key "color" is ConsoleColor.Green.
        ///     var redConf = new Configuration("red");
        ///     var blueConf = new Configuration("blue");
        ///
        ///     redConf.SetValue("color", "Red");
        ///     blueConf.SetValue("color", "Blue");
        ///
        ///     Console.ForegroundColor = Configuration.Global.GetDefaultValue&lt;ConsoleColor&gt;("color");
        ///     Console.WriteLine("This text is using the default configuration.");
        ///
        ///     Console.ForegroundColor = redConf.GetValue&lt;ConsoleColor&gt;("color");
        ///     Console.WriteLine("This text is using the user "+redConf.Name+" configuration.");
        ///
        ///     Console.ForegroundColor = blueConf.GetValue&lt;ConsoleColor&gt;("color");
        ///     Console.WriteLine("This text is using the user "+blueConf.Name+" configuration.");
        ///
        ///     Configuration.Global.SaveConfigFile(); //Saves to config.ini
        ///     redConf.SaveConfigFile(); //Saves to red.ini
        ///     blueConf.SaveConfigFile(); //Saves to blue.ini
        /// }</code></example>
        public Configuration(string name)
        {
            simpleConfig = new Dictionary<string, object>();
            configName = name;
            LoadConfigFile();
        }

        /// <summary>
        /// Creates an empty, unloaded <see cref="Configuration"/>. Used in
        /// <see cref="Configuration.Initialize(string)"/>.
        /// </summary>
        private Configuration()
        {
            simpleConfig = new Dictionary<string, object>();
        }

        /// <summary> Gets or sets the configuration that is accessed application-wide. Setting to null has no effect.</summary>
        /// <example>
        /// <code title="C#" description="Demonstrating the basic use of the application-wide configuration." lang="C#">
        /// {
        ///     //Assuming that the default association "time_seconds" exists, and is set to 20.
        ///     Configuration.Global.SetValue("time_seconds", 5);
        ///
        ///     var oldConf = Configuration.Global;
        ///
        ///     Configuration.Global = new Configuration("user5.ini");
        ///     Console.WriteLine("The old config value was "+oldConf.GetValue&lt;int&gt;("time_seconds")+" and the new one is "+Configuration.Global.GetValue&lt;int&gt;("time_seconds"));
        /// }
        /// </code>
        /// </example>
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

        /// <summary>Gets or sets the name that is used to save or load from a config file.</summary>
        /// <remarks>Modifying this property will not rename a file that exists under the old name. This configuration would then have to be
        /// saved to the new filename.</remarks>
        /// <example>
        /// <code title="C#" description="Demonstrating the use and pitfalls of changing the name of a configuration." lang="C#">
        /// {
        ///     List&lt;Configuration&gt; configs = new List&lt;Configuration&gt;();
        ///     for(int i = 0; i &lt; 20; i++)
        ///     {
        ///         var newConf = new Configuration("user"+i);
        ///         newConf.AddDefault(new ConfigurationAttribute("my_unique_number", i));
        ///         newConf.AddDefault(new ConfigurationAttribute("my_name", "Nobody"));
        ///         newConf.SaveConfigFile();
        ///         configs.Add(newConf);
        ///     }
        ///
        ///     //Picking random configurations and saving them under new names
        ///     configs[3].Name = "spencer";
        ///     configs[3].SetValue("my_name", "spencer");
        ///     configs[9].Name = "jeremiah";
        ///     configs[9].SetValue("my_name", "jeremiah");
        ///     configs[18].Name = "lucas";
        ///     configs[18].SetValue("my_name", "lucas");
        ///
        ///     //These configurations are not yet saved under the files "spencer.ini", "jeremiah.ini" or "lucas.ini", so do this now.
        ///     configs[3].SaveConfigFile();
        ///     configs[9].SaveConfigFile();
        ///     configs[18].SaveConfigFile();
        ///
        ///     //Now we have configuration files named 0 - 19, and spencer.ini, jeremiah.ini, and lucas.ini.
        /// }
        /// </code>
        /// </example>
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

        /// <summary>Adds the <paramref name="defAttrib"/> as a default value in the configuration.</summary>
        /// <param name="defAttrib">The <see cref="ConfigurationAttribute"/> to add to the default collection.</param>
        /// <remarks><para>This method is mainly to be used during application startup to initialize the default values for associated strings. May be of use to code that deals with
        /// third-party code that is loaded later in application runtime.</para>
        /// <para>This method does <strong>not</strong> add <see cref="ConfigurationAttribute"/>s to specific configurations, rather, is added to an application-wide
        /// storage of these default associations.</para></remarks>
        /// <returns>A value indicating whether or not the <paramref name="defAttrib"/> was entered. <strong>true</strong> is returned if the key of the <paramref name="defAttrib"/> previously did not exist, and
        /// was added. <strong>false</strong> if the key did exist, and the <paramref name="defAttrib"/> was not added.</returns>
        /// <example>
        /// 	<code title="Example" description="Code that demonstrates the use of creating default values for strings later in application runtime." lang="C#">
        /// {
        ///     //Using fictitious code, or pseudocode to demonstrate usage.
        ///     var plugin = PluginFactory.LoadFromFile("plugin.dll");
        ///     var configList = plugin.GetConfigAttributes();
        ///
        ///     foreach(var attrib in configList)
        ///     {
        ///         if(!Configuration.Global.AddDefault(attrib))
        ///         {
        ///             Console.WriteLine("Could not set default value for key "+attrib.Key);
        ///         } else
        ///         {
        ///             Console.WriteLine("Set default value of "+attrib.Value+" to "+attrib.Key);
        ///         }
        ///     }
        /// }</code>
        /// </example>
        public static bool AddDefault(ConfigurationAttribute defAttrib)
        {
            if (compiledConfiguration.ContainsKey(defAttrib.Key))
            {
                return false;
            }
            compiledConfiguration[defAttrib.Key] = defAttrib;
            return true;
        }

        /// <summary>Generates a list of strings containing all of the keys that have an association with a value.</summary>
        /// <returns>A list of strings that represent <see cref="ConfigurationAttribute"/> keys.</returns>
        /// <remarks>This method will list the keys from every <see cref="ConfigurationAttribute"/> that is available to all
        /// configurations. There are no <see cref="ConfigurationAttribute"/>s that are specific to any one configuration,
        /// so this method will return all of them.</remarks>
        /// <example>
        /// <code title="C#" description="Code that demonstrates example usage of this method." lang="C#">
        /// {
        ///     var keys = Configuration.GetAllKnownKeys();
        ///
        ///     Console.WriteLine("Available configuration keys:");
        ///     foreach(var key in keys)
        ///     {
        ///         Console.WriteLine(key);
        ///     }
        ///     Console.Write("Enter the key you wish to edit: ");
        ///
        ///     var keyToEdit = Console.ReadLine();
        ///
        ///     Console.Write("Enter the value to set this key to: ");
        ///
        ///     var newValue = Console.ReadLine();
        ///
        ///     try {
        ///         Configuration.Global.SetValue(keyToEdit, newValue);
        ///         Console.WriteLine("The key "+keyToEdit+" was successfully set to "+newValue);
        ///     } catch {
        ///         Console.WriteLine("An invalid value was entered for the key "+keyToEdit);
        ///     }
        /// }
        /// </code>
        /// </example>
        public static List<string> GetAllKnownKeys()
        {
            var keys = new List<string>();
            foreach (var config in compiledConfiguration)
            {
                keys.Add(config.Key);
            }
            return keys;
        }

        /// <summary> Generates an array of all configuration files that are in the configuration folder.
        /// </summary>
        /// <returns>
        /// An enumerable collection of strings of all configuration files.
        /// </returns>
        /// <remarks>All configuration files contain the file extension ".ini", so only these files will be discovered and added to the list of
        /// configuration files.</remarks>
        /// <example>
        /// <code title="C#" description="Code demonstrating the use of multiple configuration selection." lang="C#">
        /// {
        ///     //Assuming that there are files in the configuration folder.
        ///     Console.WriteLine("Pick a configuration from the following list of discovered configuration files:");
        ///
        ///     var fileCount = 1;
        ///     var files = Configuration.GetAvailableConfigs();
        ///     foreach(var file in files)
        ///     {
        ///         Console.WriteLine("[{0}] {1}", fileCount++, file);
        ///     }
        ///     Console.Write("Make a numeric selection: ");
        ///     int selection = -1;
        ///     int.TryParse(Console.ReadLine(), out selection);
        ///     if(selection != -1)
        ///     {
        ///         Configuration.Global = new Configuration(files[selection].split(".")[0]);
        ///     } else
        ///     {
        ///         Console.WriteLine("An invalid selection has been made.");
        ///     }
        /// }
        /// </code>
        /// </example>
        public static IEnumerable<string> GetAvailableConfigs()
        {
            string[] absFiles = Directory.GetFiles(Global.GetValue<string>("configuration_folder") +
                "config\\", "*.ini");
            var res = new List<string>();
            foreach (var f in absFiles)
            {
                string[] spl = f.Split('\\');
                res.Add(spl[spl.Length].Split('.')[0]);
            }
            return res;
        }

        /// <summary>Gets the default value from a <paramref name="key"/>.</summary>
        /// <typeparam name="T">The expected type of the default value.</typeparam>
        /// <param name="key">A string that identifies the associated key.</param>
        /// <returns>The default value of type T</returns>
        /// <exception cref="InvalidCastException">Thrown when the type T does not match the type of the associated value.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the default <paramref name="key"/> does not have an associated value.</exception>
        /// <remarks>The value that is returned is the value that is used by all configurations when their containing values do not
        /// override the default value.</remarks>
        /// <example>
        /// <code title="C#" description="Code that demonstrates how this method can be used to gather information about an unknown value type." lang="C#">
        /// {
        ///     //Assume that some other code has loaded a configuration and some third-party plugins. Then the plugin adds their own default values.
        ///     //We do not know the type of these values so we want to know about their type.
        ///
        ///     //First gather all plugin-specific keys.
        ///     var keysPlugin = new List&lt;string&gt;;
        ///     foreach(var key in Configuration.GetAllKnownKeys())
        ///     {
        ///         if(key.StartsWith("plug_"))
        ///         {
        ///             keysPlugin.Add(key);
        ///         }
        ///     }
        ///
        ///     //Now start displaying their type
        ///     Console.WriteLine("Here are a list of keys and their value types from loaded plugins:");
        ///     foreach(var key in keysPlugin)
        ///     {
        ///         Console.WriteLine("{0} has value of type {1}", key, Configuration.GetDefaultValue&lt;object&gt;(key).GetType().Name);
        ///     }
        /// }
        /// </code>
        /// </example>
        public static T GetDefaultValue<T>(string key)
        {
            return (T)compiledConfiguration[key].Value;
        }

        /// <summary>Gets the description of a <paramref name="key"/> provided by an associated <see cref="ConfigurationAttribute"/>.
        /// </summary>
        /// <param name="key">The associating key to get the description from.</param>
        /// <returns>The <see cref="ConfigurationAttribute.Description"/> of the given <paramref name="key"/> association.</returns>
        /// <exception cref="KeyNotFoundException">The given <paramref name="key"/> is not associated with any value or <see cref="ConfigurationAttribute"/>.</exception>
        /// <remarks>This method is normally used for displaying user-friendly text that indicates the usage for such a configuration
        /// association. A description is optional and may not be set in the associated <see cref="ConfigurationAttribute"/>.</remarks>
        /// <example>
        /// <code title="C#" description="Code that demonstrates usage of this method." lang="C#">
        /// {
        ///     var allKeys = Configuration.GetAllKnownKeys();
        ///     Console.WriteLine("Displaying all configuration keys and a description of their usage:\n");
        ///
        ///     foreach(var key in allKeys)
        ///     {
        ///         Console.WriteLine("{0}\t\t{1}", key, Configuration.GetDescription(key));
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string GetDescription(string key)
        {
            return compiledConfiguration[key].Description;
        }

        /// <summary>Gets the user-friendly text string from the <see cref="ConfigurationAttribute"/> associated with the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the <see cref="ConfigurationAttribute"/> to look up.</param>
        /// <returns>The <see cref="ConfigurationAttribute.DisplayName"/> of the associated <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">The given <paramref name="key"/> is not associated with any value or <see cref="ConfigurationAttribute"/>.</exception>
        /// <remarks>This method is normally used for displaying user-friendly text that identifies a key with a title. This string
        /// is optional and may not be set in the <see cref="ConfigurationAttribute"/>.</remarks>
        /// <example>
        /// <code title="C#" description="Code that displays to the user all keys with title and description." lang="C#">
        /// {
        ///     Console.WriteLine("Displaying all available configuration options:");
        ///     var allKeys = Configuration.GetAllKnownKeys();
        ///
        ///     foreach(var key in allKeys)
        ///     {
        ///         Console.WriteLine("{0}: {1}", key, Configuration.GetDisplayName(key));
        ///         Console.WriteLine("\t{0}", Configuration.GetDescription(key));
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string GetDisplayName(string key)
        {
            if (compiledConfiguration.ContainsKey(key))
            {
                return compiledConfiguration[key].DisplayName;
            }
            return null;
        }

        /// <summary>Initializes the configuration class for use.</summary>
        /// <param name="appDataPath">The absolute path to the desired configuration folder.</param>
        /// <remarks>Initializes the application-wide configuration by reading <see cref="ConfigurationAttribute"/> meta-data in all loaded assemblies. Sets the folder where configuration files are
        /// kept to <paramref name="appDataPath"/>. This path is accessed as a configuration key: "configuration_folder".</remarks>
        /// <example>
        /// <code title="C#" description="Code that demonstrates how to initialize a configuration on application startup." lang="C#">
        /// class Application
        /// {
        ///     static void Main(string args[])
        ///     {
        ///         //Setting the configuration folder to the root of the drive under "conf"
        ///         Configuration.Initialize("C:\\conf");
        ///     }
        /// }
        /// </code>
        /// </example>
        public static void Initialize(string appDataPath)
        {
            compiledConfiguration = new Dictionary<string, ConfigurationAttribute>();
            LoadConfigurationDefaults();

            if (appDataPath.Last() != '\\')
            {
                appDataPath += '\\';
            }

            var rootAttrib = new ConfigurationAttribute("configuration_folder", appDataPath);
            rootAttrib.DisplayName = "User Configuration Folder";
            rootAttrib.Description = "The absolute folder path of the directory that will contain all saved configurations.";
            compiledConfiguration.Add(rootAttrib.Key, rootAttrib);

            globalInstance = new Configuration();
            globalInstance.Name = "global";
            globalInstance.LoadConfigFile();
        }

        /// <summary>
        /// Converts a <paramref name="value"/> string to the value of the type <paramref name="valueType"/>.
        /// </summary>
        /// <param name="valueType">The Type of the object to convert the <paramref name="value"/> to.</param>
        /// <param name="value">The string representation of a value to parse.</param>
        /// <returns>A non-null object.</returns>
        /// <exception cref="NotSupportedException">Thrown if the type cannot be converted from a string.</exception>
        /// <remarks>The type that is given must have a <see cref="System.ComponentModel.TypeConverter"/> that allows for string-to-object conversion.
        /// This is how this method makes conversions from strings to objects. It is not necessary to use this method at all outside of the
        /// configuration class.</remarks>
        /// <example>
        /// <code title="C#" description="Code that demonstrates how to convert strings to objects." lang="C#">
        /// {
        ///     Console.Write("Please enter a color to use in the console: ");
        ///     try
        ///     {
        ///         ConsoleColor color = (ConsoleColor)Configuration.ParseString(ConsoleColor, Console.ReadLine());
        ///         Console.ForegroundColor = color;
        ///     } catch(NotSupportedException ex)
        ///     {
        ///         Console.WriteLine("A non-existing ConsoleColor was entered.");
        ///     }
        ///     Console.WriteLine("This is your new console color.");
        /// }
        /// </code>
        /// </example>
        public static object ParseString(Type valueType, string value)
        {
            var converter = TypeDescriptor.GetConverter(valueType);
            return converter.ConvertFromString(value);
        }

        /// <summary>Checks if a file with the same <see cref="Name"/> as the configuration exists in the configuration folder.</summary>
        /// <returns>A value of <strong>true</strong> indicates that the file of the same <see cref="Name"/> exists. A value of <strong>false</strong> indicates that the file of the same
        /// <see cref="Name"/> does not exist.</returns>
        /// <example>
        /// <code title="C#" description="Code that checks if a configuration file already exists." lang="C#">
        /// {
        ///     Console.WriteLine("Enter a name to give the current configuration:");
        ///     Configuration.Global.Name = Console.ReadLine();
        ///     if(Configuration.Global.FileExists())
        ///     {
        ///         Console.WriteLine("That filename already exists, cannot overwrite. Try again.");
        ///         return;
        ///     } else
        ///     {
        ///         Configuration.Global.SaveConfigFile();
        ///         Console.WriteLine("Saved the current configuration.");
        ///     }
        /// }
        /// </code>
        /// </example>
        public bool FileExists()
        {
            return File.Exists(GetValue<string>("configuration_folder") + "config\\" + configName + ".ini");
        }

        /// <summary>
        /// <see cref="Configuration.GetList(string)"/>
        /// </summary>
        /// <typeparam name="T"><see cref="Configuration.GetList(string)"/></typeparam>
        /// <param name="key"><see cref="Configuration.GetList(string)"/></param>
        /// <returns><see cref="Configuration.GetList(string)"/></returns>
        /// <remarks>For more information, see <see cref="Configuration.GetList(string)"/>.</remarks>
        [Obsolete("GetConfigValueArray() has been renamed to GetList(), as it is much shorter to write.")]
        public List<T> GetConfigValueArray<T>(string key)
        {
            return GetList<T>(key);
        }

        /// <summary>Gets a list from the associated <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The string that associates a key with a list of values.</param>
        /// <typeparam name="T">The type of list that is stored under the associated <paramref name="key"/>.</typeparam>
        /// <returns>A list of objects of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">Thrown if the associated <see cref="ConfigurationAttribute"/> does not contain a list, or the list retrieved
        /// is not of the same type as <typeparamref name="T"/>.</exception>
        /// <remarks>The <see cref="ConfigurationAttribute"/> associated with the <paramref name="key"/> must be originally set to contain a
        /// list of objects. This list of objects will then be converted to type <typeparamref name="T"/>.</remarks>
        /// <example>
        /// In this example, a class with <see cref="ConfigurationAttribute"/>s that contain lists is defined.
        /// <code title="C#" description="Code that demonstrates list retrieval." lang="C#">
        /// [Configuration("array_ints", 5, 6, 12, 22, 96, 4)]
        /// [Configuration("array_doubles", 5.5, 1.1, 2.2)]
        /// [Configuration("array_single", "hello world", null)]
        /// [Configuration("array_anything", 52, "nine", 5.1, ConsoleColor.Red)]
        /// class Application
        /// {
        ///     static void Main(string args[])
        ///     {
        ///         Configuration.Initialize("C:\\configs");
        ///         List&lt;int&gt; integers = Configuration.Global.GetList&lt;int&gt;("array_ints");
        ///         List&lt;double&gt; doubles = Configuration.Global.GetList&lt;double&gt;("array_doubles");
        ///         List&lt;string&gt; single = Configuration.Global.GetList&lt;string&gt;("array_single");
        ///         List&lt;object&gt; varying = Configuration.Global.GetList&lt;object&gt;("array_anything");
        ///
        ///         Console.WriteLine("Here are all of the configuration integers:");
        ///         foreach(var integer in integers)
        ///         {
        ///             Console.Write("\t{0}", integer);
        ///         }
        ///
        ///         Console.WriteLine("Now here are all of the doubles:");
        ///         foreach(var dbl in doubles)
        ///         {
        ///             Console.Write("\t{0}", dbl);
        ///         }
        ///
        ///         Console.WriteLine("The single item in the singles array...:\n\t{0}", single[0]);
        ///
        ///         Console.WriteLine("Finally, the rest of the items:");
        ///         foreach(var obj in varying)
        ///         {
        ///             Console.Write("\t{0}", obj.ToString());
        ///         }
        ///     }
        /// }
        /// </code>
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
            var writer = new StreamWriter(GetValue<string>("configuration_folder") + "config\\"
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
            var configRootPath = Global.GetValue<string>("configuration_folder") + "config\\";
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