using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class MatchPlayEvent : MatchEvent
    {
        public MatchPlayEvent(Match match)
            : base(match)
        {
            message = "The match is now running.";
        }
    }
}
