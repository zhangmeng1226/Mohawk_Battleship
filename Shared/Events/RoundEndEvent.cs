using MBC.Shared;
using MBC.Shared.Entities;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has ended.
    /// </summary>
    [Serializable]
    public class RoundEndEvent : MatchEvent
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor
        /// </summary>
        /// <param name="round"></param>
        public RoundEndEvent(Match match, IDNumber roundNumber)
            : base(match)
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

        protected internal override void PerformOperation()
        {
            if (!(Match.CurrentRound > -1))
            {
                throw new InvalidEventException(this, "There is no round that has started.");
            }
        }
    }
}