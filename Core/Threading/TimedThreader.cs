using System;
using System.Diagnostics;

namespace MBC.Core.Threading
{
    public class TimedThreader : FuncThreader
    {
        private Stopwatch watcher;

        public TimedThreader()
        {
            watcher = new Stopwatch();
        }

        public TimedThreader(int timeout)
        {
            watcher = new Stopwatch();
            MaxTime = timeout;
        }

        public TimedThreader(int timeout, Delegate invoke, params object[] parameters)
            : base(invoke, parameters)
        {
        }

        public int MaxTime
        {
            get;
            set;
        }

        public int TimeElapsed
        {
            get
            {
                return (int)watcher.ElapsedMilliseconds;
            }
        }

        public void TimeMethod(Delegate invoke, params object[] parameters)
        {
            SetMethod(invoke, parameters);
            TimeMethod();
        }

        public void TimeMethod()
        {
            Run();
            if (WaitFor(MaxTime))
            {
                throw new MethodTimeoutException(Method.Name, MaxTime);
            }
        }

        protected override void ThreadRun()
        {
            watcher.Restart();
            base.ThreadRun();
            watcher.Stop();
        }
    }
}