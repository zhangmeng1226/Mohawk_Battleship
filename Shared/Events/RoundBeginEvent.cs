using MBC.Shared;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="GameLogic"/> that has begun.
    /// </summary>
    public class RoundBeginEvent : MatchEvent
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor
        /// based on the <see cref="MBC.Shared.Register"/>s that are involved in it.
        /// </summary>
        /// <param name="round">The associated <see cref="GameLogic"/>.</param>
        public RoundBeginEvent(Match match, IDNumber roundNumber)
            : base(match)
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

        public override bool ApplyBackward()
        {
            if ((Match.CurrentRound - 1) == Round)
            {
                Match.CurrentRound--;
                return true;
            }
            return false;
        }

        public override bool ApplyForward()
        {
            if ((Match.CurrentRound + 1) == Round)
            {
                Match.CurrentRound++;
                foreach (var plr in Match.Players)
                {
                    plr.Active = true;
                }
                return true;
            }
            return false;
        }
    }
}