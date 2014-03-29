using MBC.Shared;
using System;

namespace MBC.Core.Threading
{
    public class ControllerTimeoutException : Exception
    {
        private string methodName;
        private IController controller;

        public ControllerTimeoutException(string methodName, IController ctrlTimeout)
            : base(ctrlTimeout.ToString()+" timed out on method call to " + methodName)
        {
            this.methodName = methodName;
        }

        public string MethodName
        {
            get
            {
                return methodName;
            }
        }

        public IController Controller
        {
            get
            {
                return controller;
            }
        }
    }
}