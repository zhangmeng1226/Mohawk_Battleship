using System;
using System.Collections.Generic;
using System.Threading;
using MBC.Core.Util;

namespace MBC.Core.Threading
{
    [Configuration("mbc_thread_pool_amt", 4)]
    public abstract class Threader
    {
        public Threader()
        {
            RunThread = new Thread(ThreadRun);
            IsRunning = false;
        }

        public event Action ThreadStop;

        /// <summary>
        /// Gets a value that indicates the running state.
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        }

        protected Thread RunThread
        {
            get;
            set;
        }

        public void Join()
        {
            RunThread.Join();
        }

        public void Run()
        {
            if (IsRunning)
            {
                return;
            }
            RunThread.Start();
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                if (ThreadStop != null)
                {
                    ThreadStop();
                }
            }
        }

        public bool WaitFor(int milliseconds)
        {
            return RunThread.Join(milliseconds);
        }

        protected abstract void ThreadRun();
    }
}