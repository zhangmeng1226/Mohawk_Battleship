using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class ControllerTimeoutEvent : RoundControllerEvent
    {
        private int time;
        private string methodName;

        public ControllerTimeoutEvent(Round round, ControllerTimeoutException exception)
            : base(exception.Controller, round)
        {
            Init(exception.Controller, round, exception.TimeTaken, exception.MethodName);
        }

        public ControllerTimeoutEvent(Controller controller, Round round, int time, string method)
            : base(controller, round)
        {
            Init(controller, round, time, method);
        }

        private void Init(Controller controller, Round round, int time, string method)
        {
            this.time = time;
            this.methodName = method;

            message = controller + " went over the time limit of " + round.MatchInfo.TimeLimit +
                "ms by " + (round.MatchInfo.TimeLimit - time) + "ms on the invoke of " + method + ".";
        }

        public int TimeTaken
        {
            get
            {
                return time;
            }
        }

        public string Method
        {
            get
            {
                return methodName;
            }
        }
    }
}
