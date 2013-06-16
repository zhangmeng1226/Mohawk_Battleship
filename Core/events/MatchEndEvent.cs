namespace MBC.Core.Events
{
    public class MatchEndEvent : MatchEvent
    {
        public MatchEndEvent(Match match)
            : base(match)
        {
            message = "The match has ended.";
        }
    }
}