using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has ended.
    /// </summary>
    [Serializable]
    public class RoundEndEvent : Event
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor
        /// </summary>
        /// <param name="round"></param>
        public RoundEndEvent(IDNumber roundNumber)
        {
            Round = roundNumber;
        }

        /// <summary>
        /// Gets the round number of this event.
        /// </summary>
        public IDNumber Round
        {
            get;
            private set;
        }
    }
}