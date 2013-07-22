using MBC.Core.Rounds;
using MBC.Shared;
using System;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="ControllerRegister"/> that had lost a <see cref="Round"/>.
    /// </summary>
    public class ControllerLostEvent : ControllerEvent
    {
        public ControllerLostEvent(ControllerID register) : base(register) { }

        protected internal override void GenerateMessage()
        {
            Message = RegisterID + " has lost the round.";
        }

        internal override void ProcBackward(Round round)
        {
            round.Remaining.Add(RegisterID);
        }

        internal override void ProcForward(Round round)
        {
            round.Remaining.Remove(RegisterID);
        }
    }
}