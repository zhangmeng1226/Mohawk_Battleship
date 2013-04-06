using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MBC.Core.util
{
    /**
     * <summary>Used for logging. Thread safe</summary>
     */
    public class Logger
    {
        static Logger()
        {
            Configuration.Default.SetConfigValue<string>("logger_default_message_level", "info");
        }
        private List<StreamWriter> oStreams;
        private Log log;
        private Configuration config;
        private string name;

        public Logger(string n) 
        {
            name = n;
            config = Configuration.Global;
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
            switch (message.Substring(0, 1).ToLower())
            {
                case "!v":
                    Log(message.Substring(2, message.Length-1).Trim(), LogMessage.Level.Verbose);
                    break;
                case "!i":
                    Log(message.Substring(2, message.Length - 1).Trim(), LogMessage.Level.Info);
                    break;
                case "!d":
                    Log(message.Substring(2, message.Length - 1).Trim(), LogMessage.Level.Debug);
                    break;
                case "!w":
                    Log(message.Substring(2, message.Length - 1).Trim(), LogMessage.Level.Warning);
                    break;
                case "!e":
                    Log(message.Substring(2, message.Length - 1).Trim(), LogMessage.Level.Error);
                    break;
                case "!c":
                    Log(message.Substring(2, message.Length - 1).Trim(), LogMessage.Level.Critical);
                    break;
                case "!f":
                    Log(message.Substring(2, message.Length - 1).Trim(), LogMessage.Level.Fatal);
                    break;
                default:
                    switch (config.GetConfigValue<string>("logger_default_message_level"))
                    {
                        case "verbose":
                            Log(message, LogMessage.Level.Verbose);
                            break;
                        case "info":
                            Log(message, LogMessage.Level.Info);
                            break;
                        case "debug":
                            Log(message, LogMessage.Level.Debug);
                            break;
                        case "warning":
                            Log(message, LogMessage.Level.Warning);
                            break;
                        case "error":
                            Log(message, LogMessage.Level.Error);
                            break;
                        case "critical":
                            Log(message, LogMessage.Level.Critical);
                            break;
                        case "fatal":
                            Log(message, LogMessage.Level.Fatal);
                            break;
                    }
                    Log(message, LogMessage.Level.Info);
                    break;
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
