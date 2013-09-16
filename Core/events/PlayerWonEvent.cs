using MBC.Core.Rounds;
using MBC.Shared;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="ControllerRegister"/> that had won a <see cref="Round"/>.
    /// </summary>
    public class PlayerWonEvent : PlayerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="ControllerRegister"/> winning a <see cref="Round"/>.</param>
        public PlayerWonEvent(Player player)
            : base(player)
        {
            Message = player + " has won the round.";
        }

        protected internal override void GenerateMessage()
        {
            Message = Player + " has won the round.";
        }
    }
}