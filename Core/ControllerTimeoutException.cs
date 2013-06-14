using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    public class ControllerTimeoutException : Exception
    {
        private ControllerUser controller;
        private string methodName;
        private int timeTaken;

        public ControllerTimeoutException(ControllerUser controller, string methodName, int timeTaken)
            : base(controller+" took too long executing "+methodName+" according to the time limit of "+
            controller.Register.Match.TimeLimit+"ms. Time taken was "+timeTaken+"ms.")
        {
            this.controller = controller;
            this.methodName = methodName;
            this.timeTaken = timeTaken;
        }

        public ControllerUser Controller
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

        public int TimeTaken
        {
            get
            {
                return timeTaken;
            }
        }
    }
}
