using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class PlayerBeginTurnEvent : PlayerEvent
    {
        public PlayerBeginTurnEvent(Player plr)
            : base(plr)
        {
        }
    }
}