using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class MatchEndEvent : MatchEvent
    {
        public MatchEndEvent(Match match)
            : base(match)
        {
            message = "The match has ended.";
        }
    }
}
