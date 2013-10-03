using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MBC.Core.Threading
{
    public abstract class Threader
    {

        public event Action ThreadStop;

        public Threader()
        {
            RunThread = new Thread(ThreadRun);
            IsRunning = false;
        }

        protected Thread RunThread
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value that indicates the running state.
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        }

        protected abstract void ThreadRun();

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
                ThreadStop(this);
            }
        }

        public void Join()
        {
            RunThread.Join();
        }

        public bool WaitFor(int milliseconds)
        {
            return RunThread.Join(milliseconds);
        }
    }
}
