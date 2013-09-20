using MBC.Core.Rounds;
using MBC.Shared;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had lost a <see cref="Round"/>.
    /// </summary>
    public class PlayerLostEvent : PlayerEvent
    {
        public PlayerLostEvent(Player loser) : base(loser) { }

        protected internal override void GenerateMessage()
        {
            Message = Player + " has lost the round.";
        }

        internal override void ProcBackward(Round round)
        {
            round.Remaining.Add(Player);
        }

        internal override void ProcForward(Round round)
        {
            round.Remaining.Remove(Player);
        }
    }
}