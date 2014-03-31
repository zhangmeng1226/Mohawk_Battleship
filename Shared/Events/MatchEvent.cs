using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public abstract class MatchEvent : Event
    {
        protected MatchEvent(Match match)
        {
            Match = match;
        }

        public Match Match
        {
            get;
            set;
        }
    }
}