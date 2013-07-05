using MBC.Core.Matches;
namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has ended.
    /// </summary>
    public class MatchEndEvent : MatchEvent
    {
        /// <summary>
        /// Passes the <paramref name="match"/> to the base constructor and generates a <see cref="Event.Message"/>.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> that has ended.</param>
        public MatchEndEvent(Match match)
            : base(match)
        {
            Message = "The match has ended.";
        }
    }
}