using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MBC.Core.util
{
    /**
     * <summary>Used for logging.</summary>
     */
    public class Logger
    {
        static Logger()
        {
            Configuration.Default.SetValue<string>("logger_def_message_level", "Info");
        }
        private List<StreamWriter> oStreams;
        private Log log;
        private string name;

        public Logger(string n) 
        {
            name = n;
        }

        /**
         * <summary>Attach a stream to this log, such as a file stream or a network stream.</summary>
         */
        public void AddOutputStream(StreamWriter sOut)
        {
            oStreams.Add(sOut);
        }

        private void BroadcastStreams(string str, LogMessage.Level lvl)
        {
            foreach(StreamWriter sw in oStreams) 
            {
                lock (sw)
                {
                    sw.Write(str);
                    sw.Write((int)lvl);
                }
            }
        }

        /**
         * <summary>Prefix-enabled logging method. Use the prefixes beginning with an exclaimation mark (!)
         * to specify a log message level. The message can be entered directly after the two characters.<br>
         * This is only a simple logging method.<br>
         * <ul>Level prefixes:
         * <li>!v  - Verbose</li>
         * <li>!i  - Info</li>
         * <li>!d  - Debug</li>
         * <li>!w  - Warning</li>
         * <li>!e  - Error</li>
         * <li>!c  - Critical</li>
         * <li>!f  - Fatal</li></ul>
         */
        public void Log(string message)
        {
            try
            {
                Log(message.Substring(2, message.Length - 1).Trim(), LogMessage.GetPrefixLevel(message));
            }
            catch (KeyNotFoundException)
            {
                Log(message.Trim(), LogMessage.GetNameLevel(Configuration.Global.GetValue<string>("logger_def_message_level")));
            }
        }

        /**
         * <summary>Log a message with the specified level. This will also immediately output
         * to the attached streams within the same thread.</summary>
         */
        public void Log(string message, LogMessage.Level lvl)
        {
            BroadcastStreams(message, lvl);
        }

    }
}
