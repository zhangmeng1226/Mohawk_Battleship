using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class TeamRemovePlayerEvent : TeamEvent
    {
        public TeamRemovePlayerEvent(Team team, Player plr)
            : base(team)
        {
        }

        public Player Player
        {
            get;
            private set;
        }

        protected internal override void PerformOperation()
        {
            if (!Team.MembersPlr.Contains(Player))
            {
                throw new InvalidEventException(this, String.Format("Player {0} is not a part of team {1}", Player, Team));
            }
            Team.MembersPlr.Remove(Player);
        }
    }
}