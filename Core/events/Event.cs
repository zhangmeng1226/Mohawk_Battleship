namespace MBC.Core.Events
{
    /// <summary>
    /// Defines a method that retrieves and handles an <see cref="Event"/>.
    /// </summary>
    /// <param name="ev">The generated <see cref="Event"/></param>
    public delegate void MBCEventHandler(Event ev, bool backward);

    /// <summary>
    /// The base class for any event created in the MBC core framework. Provides a message string that
    /// describes the generated event.
    /// </summary>
    /// <seealso cref="MatchEvent"/>
    /// <seealso cref="RoundEvent"/>
    /// <seealso cref="ControllerEvent"/>
    public abstract class Event
    {
        /// <summary>
        /// Gets a string representation of the message generated.
        /// </summary>
        public string Message
        {
            get;
            protected set;
        }

        /// <summary>
        /// Provides a string representation.
        /// </summary>
        /// <returns>The <see cref="Event.Message"/>.</returns>
        public override string ToString()
        {
            return Message;
        }

        internal virtual void ProcBackward()
        {
        }

        internal virtual void ProcForward()
        {
        }
    }
}