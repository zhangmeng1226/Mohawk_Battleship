using System;
using System.Threading;

namespace MBC.Core.Threading
{
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
                ThreadStop(this);
            }
        }

        public bool WaitFor(int milliseconds)
        {
            return RunThread.Join(milliseconds);
        }

        protected abstract void ThreadRun();
    }
}