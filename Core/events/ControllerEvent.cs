using MBC.Core.Rounds;
using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that are specific to the actions made by
    /// <see cref="Controller"/>s. Provides the <see cref="ControllerRegister"/> of the <see cref="Controller"/>
    /// that caused the event to be created. Most if not all events contain references to the objects that
    /// provide information, so the information may be irrelevent if more events are created after.
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