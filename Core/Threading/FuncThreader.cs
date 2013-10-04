using System;
using System.Reflection;

namespace MBC.Core.Threading
{
    public class FuncThreader : Threader
    {
        protected Delegate invokeMethod;
        protected object[] invokeParams;
        protected object invokeReturn;

        public FuncThreader()
        {
        }

        public FuncThreader(Delegate invoke, params object[] parameters)
        {
            SetMethod(invoke, parameters);
        }

        public MethodInfo Method
        {
            get
            {
                return invokeMethod.Method;
            }
        }

        public T GetReturnValue<T>()
        {
            return (T)invokeReturn;
        }

        public void RunMethod(Delegate invoke, params object[] parameters)
        {
            invokeParams = parameters;
            invokeMethod = invoke;
            Run();
        }

        public void SetMethod(Delegate invoke, params object[] parameters)
        {
            invokeParams = parameters;
            invokeMethod = invoke;
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