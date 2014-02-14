using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had won a <see cref="GameLogic"/>.
    /// </summary>
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

        public override bool ApplyBackward()
        {
            Player.Wins--;
            return false;
        }

        public override bool ApplyForward()
        {
            Player.Wins++;
            return true;
        }
    }
}