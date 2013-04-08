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
