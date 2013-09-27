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
        private int maxTime;

        public TimedThreader()
        {
            watcher = new Stopwatch();
        }

        public TimedThreader(int timeout)
        {
            maxTime = timeout;
        }

        public TimedThreader(Delegate invoke)
            : base(invoke)
        {
            watcher = new Stopwatch();
        }

        public TimedThreader(Delegate invoke, int timeout)
            : base(invoke)
        {
            watcher = new Stopwatch();
            maxTime = timeout;
        }

        public void TimeMethod(Delegate invoke, params object[] parameters)
        {
            SetMethod(invoke, parameters);
            TimeMethod();
        }

        public void TimeMethod()
        {
            Run();
            if (WaitFor(maxTime))
            {
                throw new MethodTimeoutException(Method.Name, maxTime);
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
