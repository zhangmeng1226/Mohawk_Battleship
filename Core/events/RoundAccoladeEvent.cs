using MBC.Core.Accolades;

namespace MBC.Core.Events
{
    public class RoundAccoladeEvent : RoundEvent
    {
        public RoundAccoladeEvent(Round round, Accolade accolade)
            : base(round)
        {
        }
    }
}