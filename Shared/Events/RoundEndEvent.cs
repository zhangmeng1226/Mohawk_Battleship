using MBC.Shared;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has ended.
    /// </summary>
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

        public override bool ApplyBackward()
        {
            return Match.CurrentRound > 0;
        }

        public override bool ApplyForward()
        {
            return Match.CurrentRound > -1;
        }
    }
}