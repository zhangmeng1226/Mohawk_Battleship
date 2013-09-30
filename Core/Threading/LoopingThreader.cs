using System;
using System.Threading;

namespace MBC.Core.Threading
{
    public class LoopingThreader : FuncThreader
    {
        public LoopingThreader()
        {

        }

        public LoopingThreader(Delegate invoke, params object[] parameters)
        {
            SetMethod(invoke, parameters);
        }

        protected override void ThreadRun()
        {
            while (IsRunning)
            {
                base.ThreadRun();
            }
        }
    }
}