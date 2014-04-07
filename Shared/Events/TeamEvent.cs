using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class TeamEvent : Event
    {
        public TeamEvent(Team team)
            : base(team)
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