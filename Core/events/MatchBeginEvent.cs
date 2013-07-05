using MBC.Core.Matches;
namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has begun.
    /// </summary>
    public class MatchBeginEvent : MatchEvent
    {
        /// <summary>
        /// Copies the <paramref name="match"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> that has started.</param>
        public MatchBeginEvent(Match match)
            : base(match)
        {
            Message = "The match has begun.";
        }
    }
}