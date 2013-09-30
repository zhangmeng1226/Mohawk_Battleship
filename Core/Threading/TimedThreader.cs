using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

        public void TimeMethod(Delegate invoke, params object[] parameters)
        {
            SetMethod(invoke, parameters);
            TimeMethod();
        }

        public int MaxTime
        {
            get;
            set;
        }

        public void TimeMethod()
        {
            Run();
            if (WaitFor(MaxTime))
            {
                throw new MethodTimeoutException(Method.Name, MaxTime);
            }
        }

        public int TimeElapsed
        {
            get
            {
                return (int)watcher.ElapsedMilliseconds;
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
