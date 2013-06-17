using System;

namespace MBC.Core.Util
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ConfigurationAttribute : Attribute
    {
        private string key;
        private object value;

        public ConfigurationAttribute(string key, object value)
        {
            this.key = key;
            this.value = value;
        }

        public string Key
        {
            get
            {
                return key;
            }
        }

        public object Value
        {
            get
            {
                return value;
            }
        }

        public string Description { get; set; }
    }
}