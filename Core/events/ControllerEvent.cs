using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that provide information on the actions
    /// made in rounds from controllers. Provides the <see cref="ControllerID"/> of the <see cref="Controller"/>
    /// that caused the event to be created.
    /// </summary>
    public abstract class ControllerEvent : RoundEvent
    {
        /// <summary>
        /// Copies the <see cref="ControllerRegister"/> object reference.
        /// </summary>
        /// <param name="register"></param>
        public ControllerEvent(ControllerID registerID)
        {
            RegisterID = registerID;
        }

        private ControllerEvent() { }

        /// <summary>
        /// Gets the <see cref="ControllerRegister"/> that caused this event to occur.
        /// </summary>
        public ControllerID RegisterID
        {
            get;
            private set;
        }
    }
}