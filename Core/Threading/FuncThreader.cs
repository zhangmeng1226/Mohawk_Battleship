using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MBC.Core.Threading
{
    public class FuncThreader : Threader
    {
        protected Delegate invokeMethod;
        protected object invokeReturn;
        protected object[] invokeParams;

        public FuncThreader() 
        {
        }

        public FuncThreader(Delegate invoke)
        {
            invokeMethod = invoke;
        }

        public MethodInfo Method
        {
            get
            {
                return invokeMethod.Method;
            }
        }

        public void SetMethod(Delegate invoke, params object[] parameters)
        {
            invokeParams = parameters;
            invokeMethod = invoke;
        }

        public void RunMethod(Delegate invoke, params object[] parameters)
        {
            invokeParams = parameters;
            invokeMethod = invoke;
            Run();
        }

        public T GetReturnValue<T>()
        {
            return (T)invokeReturn;
        }

        protected override void ThreadRun()
        {
            if (invokeMethod == null)
            {
                return;
            }
            invokeReturn = invokeMethod.DynamicInvoke(invokeParams);
        }
    }
}
