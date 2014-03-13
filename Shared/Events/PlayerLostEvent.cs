using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had lost a <see cref="GameLogic"/>.
    /// </summary>
    [Serializable]
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
    }
}