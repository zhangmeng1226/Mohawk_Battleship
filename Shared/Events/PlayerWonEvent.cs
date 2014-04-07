using MBC.Shared;
using MBC.Shared.Entities;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had won a <see cref="GameLogic"/>.
    /// </summary>
    [Serializable]
    public class PlayerWonEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event with the player that won.
        /// </summary>
        /// <param name="player"></param>
        public PlayerWonEvent(Player player)
            : base(player)
        {
        }

        protected internal override void PerformOperation()
        {
            if (!Player.Active)
            {
                throw new InvalidEventException(this, "The player is inactive.");
            }
            Player.Wins++;
            Player.Active = false;
        }
    }
}