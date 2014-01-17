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
        /// Deprecated. Do not use.
        /// </summary>
        /// <param name="config"></param>
        [Obsolete("MatchConfig is deprecated. Match parameters located in the match object itself.")]
        public MatchConfigChangedEvent(MatchConfig config)
        {
            Config = config;
        }

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
        /// Deprecated. Do not use
        /// </summary>
        [Obsolete("MatchConfig is deprecated.")]
        public MatchConfig Config
        {
            get;
            private set;
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