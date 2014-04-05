using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class TeamEvent : Event
    {
        public TeamEvent(Team team)
        {
            Team = team;
        }

        public Team Team
        {
            get;
            private set;
        }
    }
}