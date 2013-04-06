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
        public string message;
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
