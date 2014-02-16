using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had lost a <see cref="GameLogic"/>.
    /// </summary>
    public class PlayerLostEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event with the player that lost.
        /// </summary>
        /// <param name="loser"></param>
        public PlayerLostEvent(Player loser)
            : base(loser)
        {
        }

        public override bool ApplyBackward()
        {
            if (!Player.Active)
            {
                Player.Active = true;
                Player.Losses--;
                return true;
            }
            return false;
        }

        public override bool ApplyForward()
        {
            if (Player.Active)
            {
                Player.Active = false;
                Player.Losses++;
                return true;
            }
            return false;
        }
    }
}