using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.util
{
    public class LoggerManager
    {

        private LoggerManager() { }

        private static Dictionary<string, Logger> logStore = new Dictionary<string,Logger>();

        /**
         * <summary>Use this function to select the Logger class attached with a string</summary>
         * <remarks>Use as LoggerManager["ANY_NAME"] to get Logger objects. It will generate non-existing Loggers
         * automatically.</remarks>
         */
        public static Logger GetLogger(string name)
        {
                Logger res = null;
                logStore.TryGetValue(name, out res);
                if (res == null)
                    res = new Logger(name);
                return res;
        }
    }
}
