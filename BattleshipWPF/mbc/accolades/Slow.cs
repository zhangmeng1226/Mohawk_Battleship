using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public class Slow : AccoladeProcessor
    {
        int cnt = 0;
        int diff = 0;

        private void ResetCounters()
        {
            cnt = 0;
            diff = 0;
        }

        public MBCRoundLog.RoundAccolade Process(MBCRoundLog.RoundActivity a)
        {
            if (a.action != MBCRoundLog.RoundAction.ShotAndHit && a.action != MBCRoundLog.RoundAction.ShotAndMiss)
                return MBCRoundLog.RoundAccolade.None;

            if (a.action == MBCRoundLog.RoundAction.ShotAndHit)
            {
                diff++;
                if (diff > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_slow_diff", 4))
                    ResetCounters();
            }
            else
            {
                cnt++;
                diff = 0;
            }

            if (cnt > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_slow_cnt", 25))
            {
                ResetCounters();
                return MBCRoundLog.RoundAccolade.Slow;
            }

            return MBCRoundLog.RoundAccolade.None;
        }
    }
}
