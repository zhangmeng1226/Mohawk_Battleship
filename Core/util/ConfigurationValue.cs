using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Util
{
    public struct ConfigurationValue
    {

        private object value = null;

        public static implicit operator object(ConfigurationValue val)
        {
            return val.value;
        }

        public static implicit operator ConfigurationValue(object val)
        {
            return new ConfigurationValue { value = val };
        }
    }
}
