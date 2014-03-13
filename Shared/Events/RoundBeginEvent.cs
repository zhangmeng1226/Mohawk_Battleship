using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has begun.
    /// </summary>
    [Serializable]
    public class RoundBeginEvent : Event
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor
        /// based on the <see cref="MBC.Shared.Register"/>s that are involved in it.
        /// </summary>
        /// <param name="round">The associated <see cref="GameLogic"/>.</param>
        public RoundBeginEvent(IDNumber roundNumber)
        {
            Round = roundNumber;
        }

        /// <summary>
        /// Gets the round number.
        /// </summary>
        public IDNumber Round
        {
            get;
            private set;
        }
    }
}