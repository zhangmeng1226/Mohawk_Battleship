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
     * for log monitoring.</summary>
     */
    public class Log
    {
        private StreamReader iStream;
        private FileSystemWatcher fileWatcher;

        /**
         * <summary>This Log class constructor requires a stream to read from. Then, the constructor
         * will begin creating a new thread to monitor the stream.</summary>
         */
        public Log(StreamReader readFrom)
        {
            iStream = readFrom;
            ThreadPool.QueueUserWorkItem(ThreadLoop, 0);
        }

        public Log(string name)
        {
            CreateFileWatcher(name);
        }

        public Log()
        {
            CreateFileWatcher("default");
        }

        private void CreateFileWatcher(string name)
        {
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = Util.WorkingDirectory() + "logs\\";
            fileWatcher.Filter = name + ".txt";
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess;
            fileWatcher.Changed += FileChangeEvent;
            fileWatcher.EnableRaisingEvents = true;
        }

        private void FileChangeEvent(object source, FileSystemEventArgs e)
        {

        }

        private void ThreadLoop(Object tn)
        {

        }
    }
}
