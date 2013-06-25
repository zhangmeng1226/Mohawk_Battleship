using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="ControllerRegister"/> that had lost a <see cref="Round"/>.
    /// </summary>
    public class ControllerLostEvent : ControllerEvent
    {
        /// <summary>
        /// Passes the <paramref name="controller"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="controller"></param>
        public ControllerLostEvent(ControllerRegister controller)
            : base(controller)
        {
            message = controller + " has lost the round.";
        }
    }
}