using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace MBC.Core.Controllers
{
    /// <summary>
    /// Times a running thread and aborts it if it exceeds a timeout value
    /// </summary>
    public class ThreadTimeoutAborter
    {
        private Thread abortThread;
        private int timeout;
        private Timer timer;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="th">The thread to abort at timeout</param>
        /// <param name="time">The timeout value</param>
        public ThreadTimeoutAborter(int time)
        {
            timer = new Timer(new TimerCallback(ThreadTimeout));
            timeout = time;
        }

        /// The time (in milliseconds) to wait before aborting a thread once monitoring begins
        /// </summary>
        public int TimeoutMillis
        {
            get
            {
                return timeout;
            }
            set
            {
                if (timeout < 0)
                {
                    throw new ArgumentOutOfRangeException("Cannot provide a timeout value less than 0");
                }
                timeout = value;
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void MonBegin()
        {
            timer.Change(TimeoutMillis, Timeout.Infinite);
            abortThread = Thread.CurrentThread;
        }

        /// <summary>
        /// Ends the timer and prevents the Thread from aborting.
        /// </summary>
        /// <returns>The elapsed time between beginning and ending monitoring in milliseconds</returns>
        public void MonEnd()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Stops the timer and aborts the thread.
        /// </summary>
        /// <param name="stateObject">Unused</param>
        private void ThreadTimeout(object stateObject)
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            abortThread.Abort();
        }
    }
}