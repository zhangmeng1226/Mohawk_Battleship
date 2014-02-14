using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Match"/> that has ended.
    /// </summary>
    public class MatchEndEvent : MatchEvent
    {
        /// <summary>
        /// Constructs the event.
        /// </summary>
        public MatchEndEvent(Match match)
            : base(match)
        {
        }

        public override bool ApplyBackward()
        {
            return true;
        }

        public override bool ApplyForward()
        {
            return true;
        }
    }
}