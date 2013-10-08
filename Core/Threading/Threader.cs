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

        public ThreadPriority Priority
        {
            get
            {
                return RunThread.Priority;
            }
            set
            {
                RunThread.Priority = value;
            }
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