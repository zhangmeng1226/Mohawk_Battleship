using System;
using System.Collections.Generic;

namespace MBC.Core.Util
{
    /// <summary>
    /// Essentially provides value(s) associated with a unique key that will be loaded by a call to
    /// <see cref="Configuration.Initialize(string)"/>.
    /// Optionally provides a <see cref="ConfigurationAttribute.Description"/> that explains the purpose
    /// of the existence of the key and value pair. If the values have the possibility to be an array,
    /// more than one value must be given in the constructor handling many objects. Values that are
    /// not equal to the first object type will be ignored, but can effectively set the <see cref="Configuration"/>
    /// key to allow a list of objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ConfigurationAttribute : Attribute
    {
        private string key;
        private object value;

        /// <summary>
        /// Stores a single <paramref name="value"/> as <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The string that represents the <paramref name="value"/></param>
        /// <param name="value">The value to be stored to the <paramref name="key"/></param>
        public ConfigurationAttribute(string key, object value)
        {
            this.value = value;
            this.key = key;
        }

        /// <summary>
        /// Stores the <paramref name="values"/> as a list in the <paramref name="key"/>. Ignores
        /// objects with types different from the first.
        /// </summary>
        /// <param name="key">The key to set.</param>
        /// <param name="values">The value(s) to set the key.</param>
        public ConfigurationAttribute(string key, params object[] values)
        {
            if (values.Length == 0)
            {
                throw new ArgumentException("Must provide at least one value for " + key);
            }
            this.key = key;

            var list = new List<object>();
            var firstType = values[0].GetType();
            foreach (var valObj in values)
            {
                if (valObj != null && valObj.GetType() == firstType)
                {
                    list.Add(valObj);
                }
            }
            this.value = list;
        }

        /// <summary>
        /// Gets or sets the description of the key and value pair.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string that will be used to display the key to the user.
        /// </summary>
        public string DisplayName
        {
            get;
            set;
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
    }
}