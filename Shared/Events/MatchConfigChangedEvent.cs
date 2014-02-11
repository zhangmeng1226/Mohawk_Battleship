using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Event that is created when the match parameters have changed.
    /// </summary>
    public class MatchConfigChangedEvent : Event
    {
        /// <summary>
        /// Constructs this event.
        /// </summary>
        public MatchConfigChangedEvent()
        {
        }

        /// <summary>
        /// Constructs this event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public MatchConfigChangedEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the serialization data from the event.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}