using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="ControllerRegister"/> that had won a <see cref="Round"/>.
    /// </summary>
    public class ControllerWonEvent : ControllerEvent
    {
        /// <summary>
        /// Passes the <paramref name="register"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="register">The <see cref="ControllerRegister"/> winning a <see cref="Round"/>.</param>
        public ControllerWonEvent(ControllerRegister register)
            : base(register)
        {
            message = register + " has won the round.";
        }
    }
}