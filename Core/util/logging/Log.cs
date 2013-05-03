using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace MBC.Core.util
{
    /**
     * <summary>The Log class is used to monitor a stream or a file for log changes, and provides events
     * for log monitoring. (CLIENT)</summary>
     */
    public class Log
    {
        private StreamReader iStream;
        private Thread readThread;

        public delegate void LogMessageReceived(LogMessage msg);
        public event LogMessageReceived LogMessageReceivedEvent;

        /**
         * <summary>This Log class constructor requires a stream to read from. Then, the constructor
         * will begin creating a new thread to monitor the stream.</summary>
         */
        public Log(StreamReader readFrom)
        {
            iStream = readFrom;
            readThread = new Thread(new ThreadStart(ThreadLoop));
            readThread.Start();
        }

        ~Log()
        {
            readThread.Abort();
            iStream.Close();
        }

        private void ThreadLoop()
        {
            while (true)
                try
                {
                    string[] line = iStream.ReadLine().Split('\t');
                    if (LogMessageReceivedEvent != null)
                        LogMessageReceivedEvent(new LogMessage(line[2], LogMessage.GetNameLevel(line[1]), DateTime.Parse(line[0])));
                }
                catch (Exception) { Util.PrintDebugMessage("Log parsing error occurred."); }
        }
    }
}
