using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace MBC.Core.Events
{
    /// <summary>
    /// Defines a method that retrieves and handles an <see cref="Event"/>.
    /// </summary>
    /// <param name="ev">The generated <see cref="Event"/></param>
    public delegate void MBCEventHandler(Event ev);

    /// <summary>
    /// The base class for any event created in the MBC core framework. Provides a message string that
    /// describes the generated event.
    /// </summary>
    /// <seealso cref="MatchEvent"/>
    /// <seealso cref="RoundEvent"/>
    /// <seealso cref="PlayerEvent"/>
    public abstract class Event
    {
        private List<Event> subEvents;

        protected internal virtual string GenerateMessage()
        {
            return "";
        }

        /// <summary>
        /// Provides a string representation.
        /// </summary>
        /// <returns>The <see cref="Event.Message"/>.</returns>
        public override string ToString()
        {
            return GenerateMessage();
        }
    }
}