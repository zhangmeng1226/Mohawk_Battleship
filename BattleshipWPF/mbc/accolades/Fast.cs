using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public class Fast : AccoladeProcessor
    {
        int diff = 0;
        int absent = 0;

        public MBCRoundLog.RoundAccolade Process(MBCRoundLog.RoundActivity a)
        {
            if (a.action != MBCRoundLog.RoundAction.ShotAndMiss && a.action != MBCRoundLog.RoundAction.ShotAndHit)
                return MBCRoundLog.RoundAccolade.None;
            if (a.action != MBCRoundLog.RoundAction.ShotAndHit)
            {
                if (++absent > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_fast_absent", 4))
                {
                    diff = 0;
                    absent = 0;
                }
            }
            else
            {
                diff++;
                absent = 0;
            }

            if (diff > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_fast_diff", 10))
            {
                diff = 0;
                return MBCRoundLog.RoundAccolade.Fast;
            }
            return MBCRoundLog.RoundAccolade.None;
        }
    }
}
