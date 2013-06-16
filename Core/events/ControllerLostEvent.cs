using MBC.Shared;

namespace MBC.Core.Events
{
    public class ControllerLostEvent : ControllerEvent
    {
        public ControllerLostEvent(ControllerRegister controller)
            : base(controller)
        {
            message = controller + " has lost the round.";
        }
    }
}