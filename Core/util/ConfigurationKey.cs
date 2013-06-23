using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Util
{
    public struct ConfigurationKey
    {
        private string value = "";

        public static implicit operator string(ConfigurationKey key)
        {
            return key.value;
        }

        public static implicit operator ConfigurationKey(string key)
        {
            return new ConfigurationKey { value = key };
        }
    }
}
