using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class MatchAddPlayerEvent : MatchEvent
    {

        public MatchAddPlayerEvent(Player newPlayer)
        {
            Player = newPlayer;
        }

        public Player Player
        {
            get;
            private set;
        }

        protected internal override string GenerateMessage()
        {
            return "Added player " + Player;
        }
    }
}
