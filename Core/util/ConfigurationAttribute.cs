using System;

namespace MBC.Core.Util
{
    /// <summary>
    /// Essentially provides a value associated with a unique key that will be loaded by a call to
    /// <see cref="Configuration.InitializeConfiguration(string)"/> to <see cref="Configuration.Default"/>.
    /// Optionally provides a <see cref="ConfigurationAttribute.Description"/> that explains the purpose
    /// of the existence of the key and value pair.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ConfigurationAttribute : Attribute
    {
        private string key;
        private object value;

        /// <summary>
        /// Initializes the <see cref="Key"/> and <see cref="Value"/> with the ones provided.
        /// </summary>
        /// <param name="key">The key to set.</param>
        /// <param name="value">The value to set the key.</param>
        public ConfigurationAttribute(string key, object value)
        {
            this.key = key;
            this.value = value;
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        public string Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Gets the value of the key.
        /// </summary>
        public object Value
        {
            get
            {
                return value;
            }
        }

        /// <summary>
        /// Gets or sets the description of the key and value pair.
        /// </summary>
        public string Description { get; set; }
    }
}