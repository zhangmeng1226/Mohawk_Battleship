using MBC.Core.Rounds;
using MBC.Shared;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// A <see cref="RoundEvent"/> that provides information on a turn change.
    /// </summary>
    public class RoundTurnChangeEvent : RoundEvent
    {
        /// <summary>
        /// Sets the previous and next <see cref="IDNumber"/>s in the switch.
        /// </summary>
        /// <param name="last">The previous <see cref="IDNumber"/> that had the turn.</param>
        /// <param name="next">The next <see cref="IDNumber"/> to take the turn.</param>
        public RoundTurnChangeEvent(Player last, Player next)
        {
            NextTurn = next;
            PreviousTurn = last;
        }

        /// <summary>
        /// Gets the <see cref="IDNumber"/> of the next turn.
        /// </summary>
        public Player NextTurn
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="IDNumber"/> of the previous turn.
        /// </summary>
        public Player PreviousTurn
        {
            get;
            private set;
        }

        /// <summary>
        /// Generates the message that describes this <see cref="Event"/>.
        /// </summary>
        protected internal override void GenerateMessage()
        {
            Message = "Turn switch from " + PreviousTurn + " to " + NextTurn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="round"></param>
        internal override void ProcBackward(Round round)
        {
            round.CurrentTurn = PreviousTurn;
        }

        internal override void ProcForward(Round round)
        {
            round.CurrentTurn = NextTurn;
        }
    }
}