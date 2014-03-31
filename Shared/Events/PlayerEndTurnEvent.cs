using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared.Events
{
    public class PlayerEndTurnEvent : PlayerEvent
    {
        public PlayerEndTurnEvent(Player plr)
            : base(plr)
        {
        }
    }
}