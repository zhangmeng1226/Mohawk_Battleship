using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MBC.Core.util
{
    /**
     * <summary>Used for logging. (SERVER)</summary>
     */
    public class Logger
    {
        /**
         * <summary>Sets default configuration values for keys that relate to this class.
         * Should be called before using the global Configuration.Default object.</summary>
         */
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<string>("logger_def_message_level", "Info");
            Configuration.Default.SetValue<string>("logger_directory", "logs");
        }
        private List<StreamWriter> oStreams;
        private Log logReader;
        private MemoryStream mainStream; //No pun intended
        private FileStream fileStream;

        public Logger(string n)
        {
            oStreams = new List<StreamWriter>();
            mainStream = new MemoryStream();
            StreamWriter mainWriter = new StreamWriter(mainStream);
            fileStream = new FileStream(Util.WorkingDirectory() + Configuration.Global.GetValue<string>("logger_directory"), FileMode.OpenOrCreate);

            logReader = new Log(new StreamReader(mainStream));

            AddOutputStream(new StreamWriter(fileStream)); //Add the file stream
            AddOutputStream(mainWriter);

            //Reading stored contents from file to memory
            StreamReader read = new StreamReader(fileStream);
            while (!read.EndOfStream)
                mainWriter.WriteLine(read.ReadLine());
        }

        ~Logger()
        {
            fileStream.Close();
        }

        /**
         * <summary>Attach a stream to this log, such as a file stream or a network stream.</summary>
         */
        public void AddOutputStream(StreamWriter sOut)
        {
            oStreams.Add(sOut);
        }

        /**
         * <summary>Writes to all streams. The message is formatted as follows:
         * [DATE]\t[[MESSAGELEVEL]]\t[MESSAGE][LINE_TERMINATION]
         * </summary>
         */
        private void BroadcastStreams(string str, LogMessage.Level lvl)
        {
            string date = DateTime.Now.ToString();
            string level = Enum.GetName(typeof(LogMessage.Level), lvl);
            foreach (StreamWriter sw in oStreams)
            {
                lock (sw)
                {
                    sw.WriteLine("%s\t[%s]\t%s", date, level, str);
                    sw.Flush();
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

        public Log GetLog()
        {
            return logReader;
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
