using MBC.Core.Rounds;
using MBC.Shared;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has begun.
    /// </summary>
    public class RoundBeginEvent : Event
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor
        /// based on the <see cref="MBC.Shared.Register"/>s that are involved in it.
        /// </summary>
        /// <param name="round">The associated <see cref="GameLogic"/>.</param>
        public RoundBeginEvent(int roundNumber)
        {
            Round = roundNumber;
        }

        /// <summary>
        /// Constructs the event from serialization data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public RoundBeginEvent(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the round number.
        /// </summary>
        public int Round
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