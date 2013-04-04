using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public class Intense : AccoladeProcessor
    {
        Dictionary<IBattleshipController, int> hits = new Dictionary<IBattleshipController,int>();
        int cnt = 0;

        public MBCRoundLog.RoundAccolade Process(MBCRoundLog.RoundActivity a)
        {
            if (a.action != MBCRoundLog.RoundAction.ShotAndHit)
                return MBCRoundLog.RoundAccolade.None;
            hits[a.ibc]++;

            if (Math.Abs(hits.Values.ElementAt(0) - hits.Values.ElementAt(1)) >
                BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_intense_diff", 4))
                cnt = 0;
            else
                cnt++;

            if (cnt > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_intense_count", 8))
            {
                cnt = 0;
                return MBCRoundLog.RoundAccolade.Intense;
            }

            return MBCRoundLog.RoundAccolade.None;
        }
    }
}
