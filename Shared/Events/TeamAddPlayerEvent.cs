using MBC.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class TeamAddPlayerEvent : TeamEvent
    {
        public TeamAddPlayerEvent(Team team, Player plr)
            : base(team)
        {
            Player = plr;
        }

        public Player Player
        {
            get;
            private set;
        }

        protected internal override void PerformOperation()
        {
            if (Team.MembersPlr.Contains(Player))
            {
                throw new InvalidEventException(this, String.Format("Player {0} is already a member of team {1}", Player, Team));
            }
            Team.MembersPlr.Add(Player);
        }
    }
}