using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public class Intense : AccoladeProcessor
    {
        Dictionary<IBattleshipOpponent, int> hits = new Dictionary<IBattleshipOpponent,int>();
        int cnt = 0;

        public override RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;
            hits[a.opponent]++;

            if (Math.Abs(hits.Values.ElementAt(0) - hits.Values.ElementAt(1)) >
                BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_intense_diff", 4))
                cnt = 0;
            else
                cnt++;

            if (cnt > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_intense_count", 8))
            {
                cnt = 0;
                return RoundLog.RoundAccolade.Intense;
            }

            return RoundLog.RoundAccolade.None;
        }
    }
}
