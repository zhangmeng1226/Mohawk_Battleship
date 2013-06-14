using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    public class ControllerTimeoutException : Exception
    {
        private ControllerRegister controller;
        private string methodName;
        private int timeTaken;

        public ControllerTimeoutException(ControllerRegister register, string methodName, int timeTaken)
            : base(register+" took too long executing "+methodName+" according to the time limit of "+
            register.Match.TimeLimit+"ms. Time taken was "+timeTaken+"ms.")
        {
            this.controller = register;
            this.methodName = methodName;
            this.timeTaken = timeTaken;
        }

        public ControllerRegister Register
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
