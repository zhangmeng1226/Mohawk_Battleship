using MBC.Core.Rounds;
using MBC.Shared;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has ended.
    /// </summary>
    public class RoundEndEvent : Event
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor
        /// </summary>
        /// <param name="round"></param>
        public RoundEndEvent(IDNumber roundNumber)
        {
            RoundNumber = roundNumber;
        }

        /// <summary>
        /// Constructs the event from the serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public RoundEndEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the round number of this event.
        /// </summary>
        public IDNumber RoundNumber
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