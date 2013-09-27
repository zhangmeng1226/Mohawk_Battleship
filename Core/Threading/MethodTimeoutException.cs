using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Threading
{
    public class MethodTimeoutException : Exception
    {
        private string methodName;
        private int timeTaken;

        public MethodTimeoutException(string methodName, int timeTaken) 
            : base("Method call to "+methodName+" has exceeded the time limit of "+timeTaken)
        {
            this.methodName = methodName;
            this.timeTaken = timeTaken;
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
