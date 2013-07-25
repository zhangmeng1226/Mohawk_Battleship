using MBC.Core.Rounds;
using MBC.Shared;
using System;

namespace MBC.Core.Events
{
    public class RoundTurnChangeEvent : RoundEvent
    {
        public RoundTurnChangeEvent(ControllerID last, ControllerID next)
        {
            NextTurn = next;
            PreviousTurn = last;
        }

        public ControllerID NextTurn
        {
            get;
            private set;
        }

        public ControllerID PreviousTurn
        {
            get;
            private set;
        }
        protected internal override void GenerateMessage()
        {
            Message = "Turn switch from " + PreviousTurn + " to " + NextTurn;
        }

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