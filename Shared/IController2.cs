using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public interface IController2
    {
        public Match match;

        event StringOutputHandler ControllerMessageEvent;
    }
}