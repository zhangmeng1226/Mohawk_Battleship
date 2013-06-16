namespace MBC.Core.Events
{
    public class MatchBeginEvent : MatchEvent
    {
        public MatchBeginEvent(Match match)
            : base(match)
        {
            message = "The match has begun.";
        }
    }
}