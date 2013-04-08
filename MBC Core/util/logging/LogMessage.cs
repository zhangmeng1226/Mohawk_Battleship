using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.util
{
    /**
     * <summary>The LogMessage class is used to describe a single log message to a Log object.</summary>
     */
    public class LogMessage
    {
        private static Dictionary<string, string> prefixLevels;

        static LogMessage()
        {
            prefixLevels = new Dictionary<string, string>();
            prefixLevels.Add("!v", "Verbose");
            prefixLevels.Add("!i", "Info");
            prefixLevels.Add("!d", "Debug");
            prefixLevels.Add("!w", "Warning");
            prefixLevels.Add("!e", "Error");
            prefixLevels.Add("!c", "Critical");
            prefixLevels.Add("!f", "Fatal");
        }

        /**
         * <summary>Translates a prefix string into the corresponding LogMessage Level. The string
         * specified will be made into lowercase.</summary>
         */
        public static LogMessage.Level GetPrefixLevel(string pre)
        {
            return (LogMessage.Level) Enum.Parse(typeof(Level), prefixLevels[pre]);
        }

        /**
         * <summary>Translates a name string into the corresponding LogMessage Level. The string
         * specified will be made into lowercase.</summary>
         */
        public static LogMessage.Level GetNameLevel(string name)
        {
            return (LogMessage.Level) Enum.Parse(typeof(Level), name);
        }

        private string message;
        private Level level;

        public enum Level
        {
            Verbose,
            Info,
            Debug,
            Warning,
            Error,
            Critical,
            Fatal
        }
    }
}
