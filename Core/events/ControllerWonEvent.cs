using MBC.Shared;

namespace MBC.Core.Events
{
    public class ControllerWonEvent : ControllerEvent
    {
        public ControllerWonEvent(ControllerRegister register)
            : base(register)
        {
            message = register + " has won the round.";
        }
    }
}