using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;

namespace MBC.Core.Matches
{
    public class AllRoundsMatch : ActiveMatch
    {
        public AllRoundsMatch()
        {
        }

        public AllRoundsMatch(Configuration conf)
            : base(conf)
        {
        }

        protected override GameLogic CreateNewRound(IDNumber roundID)
        {
            return new ClassicRound();
        }
    }
}