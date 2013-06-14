using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class MatchBeginEvent : MatchEvent
    {
        public MatchBeginEvent(Match match) : base(match)
        {

        }
    }
}
