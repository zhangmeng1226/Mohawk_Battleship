using System;
using System.Xml.Serialization;
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
        [XmlIgnore]
        private string curMessage = null;

        /// <summary>
        /// Gets a string representation of the message generated.
        /// </summary>
        [XmlIgnore]
        public string Message
        {
            get
            {
                if (curMessage == null)
                {
                    GenerateMessage();
                }
                return curMessage;
            }
            protected set
            {
                curMessage = value;
            }
        }

        protected internal abstract void GenerateMessage();

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