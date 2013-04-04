using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public class Comeback : AccoladeProcessor
    {
        int diff = 0;
        int backDiff = 0;
        IBattleshipController last = null;

        private void StartNewCount(IBattleshipController op)
        {
            diff = 0;
            backDiff = 0;
            last = op;
        }

        public MBCRoundLog.RoundAccolade Process(MBCRoundLog.RoundActivity a)
        {
            if (a.action != MBCRoundLog.RoundAction.ShotAndHit)
                return MBCRoundLog.RoundAccolade.None;

            if (last != a.ibc)
            {
                if (diff > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_comeback_diff", 8))
                    backDiff++;
                else
                    StartNewCount(a.ibc);
            }
            else
            {
                diff = diff + 1 - backDiff;
                backDiff = 0;
            }
            if (backDiff > diff+1)
            {
                StartNewCount(a.ibc);
                return MBCRoundLog.RoundAccolade.Comeback;
            }
            return MBCRoundLog.RoundAccolade.None;
        }
    }
}
