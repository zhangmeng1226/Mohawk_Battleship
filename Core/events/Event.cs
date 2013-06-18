namespace MBC.Core.Events
{

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
        /// The message describing the occurrence.
        /// </summary>
        protected string message;

        /// <summary>
        /// Gets a string representation of the message generated.
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// Provides a string representation.
        /// </summary>
        /// <returns>The <see cref="Event.Message"/>.</returns>
        public override string ToString()
        {
            return Message;
        }
    }
}