using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class MatchEvent : Event
    {
        private Match match;

        public MatchEvent(Match match)
        {

        }

        public Match Match
        {
            get
            {
                return match;
            }
        }
    }
}
