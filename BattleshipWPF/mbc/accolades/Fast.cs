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

        public override RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndMiss && a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;
            if (a.action != RoundLog.RoundAction.ShotAndHit)
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
                return RoundLog.RoundAccolade.Fast;
            }
            return RoundLog.RoundAccolade.None;
        }
    }
}
