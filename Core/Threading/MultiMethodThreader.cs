using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MBC.Core.Threading
{
    public class MultiMethodThreader : Threader
    {
        private Dictionary<string, ThreadStart> methodStore;

        public MultiMethodThreader()
        {
            RunThread = new Thread(RunMethod);
            methodStore = new Dictionary<string, ThreadStart>();
        }

        public ThreadStart this[string key]
        {
            get
            {
                return methodStore[key];
            }
            set
            {
                methodStore[key] = value;
            }
        }

        public void InvokeMethod(string name)
        {
            MethodToRun = name;
            Run();
        }

        public string MethodToRun
        {
            get;
            set;
        }

        private void RunMethod()
        {
            methodStore[MethodToRun].Invoke();
        }
    }
}
