using MBC.Shared;

namespace MBC.Core.Events
{
    /// <summary>
    /// Defines a method that retrieves and handles a <see cref="ControllerEvent"/>.
    /// </summary>
    /// <param name="ev">The generated <see cref="ControllerEvent"/>.</param>
    public delegate void MBCControllerEventHandler(ControllerEvent ev);

    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that are specific to the actions made by
    /// <see cref="Controller"/>s. Provides the <see cref="ControllerRegister"/> of the <see cref="Controller"/>
    /// that caused the event to be created. Most if not all events contain references to the objects that
    /// provide information, so the information may be irrelevent if more events are created after.
    /// </summary>
    public abstract class ControllerEvent : Event
    {
        private ControllerRegister register;

        /// <summary>
        /// Copies the <see cref="ControllerRegister"/> object reference.
        /// </summary>
        /// <param name="register"></param>
        public ControllerEvent(ControllerRegister register)
        {
            this.register = register;
        }

        /// <summary>
        /// Gets the <see cref="ControllerRegister"/> that caused this event to occur.
        /// </summary>
        public ControllerRegister Register
        {
            get
            {
                return register;
            }
        }
    }
}