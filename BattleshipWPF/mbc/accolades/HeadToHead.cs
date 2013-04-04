using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public class HeadToHead : AccoladeProcessor
    {

        int cnt = 0;
        int diff = 0;
        IBattleshipController op = null;

        private void ResetCounters()
        {
            cnt = 0;
            diff = 0;
        }

        public MBCRoundLog.RoundAccolade Process(MBCRoundLog.RoundActivity a)
        {
            if (a.action != MBCRoundLog.RoundAction.ShotAndHit)
                return MBCRoundLog.RoundAccolade.None;

            if (op == a.ibc)
            {
                diff++;
                if (diff > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_h2h_diff", 4))
                    ResetCounters();
            }
            else
            {
                op = a.ibc;
                diff = 0;
                cnt++;
            }

            if (cnt > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_h2h_count", 8))
            {
                ResetCounters();
                return MBCRoundLog.RoundAccolade.HeadToHead;
            }

            return MBCRoundLog.RoundAccolade.None;
        }
    }
}
