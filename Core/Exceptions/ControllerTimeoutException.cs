using MBC.Shared;
using System;

namespace MBC.Core.Threading
{
    public class ControllerTimeoutException : Exception
    {
        private IController2 controller;
        private string methodName;

        public ControllerTimeoutException(string methodName, IController2 ctrlTimeout)
            : base(ctrlTimeout.ToString() + " timed out on method call to " + methodName)
        {
            this.methodName = methodName;
        }

        public IController2 Controller
        {
            get
            {
                return controller;
            }
        }

        public string MethodName
        {
            get
            {
                return methodName;
            }
        }
    }
}