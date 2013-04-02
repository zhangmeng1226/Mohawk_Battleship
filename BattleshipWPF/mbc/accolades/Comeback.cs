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
        IBattleshipOpponent last = null;

        private void StartNewCount(IBattleshipOpponent op)
        {
            diff = 0;
            backDiff = 0;
            last = op;
        }

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;

            if (last != a.opponent)
            {
                if (diff > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_comeback_diff", 8))
                    backDiff++;
                else
                    StartNewCount(a.opponent);
            }
            else
            {
                diff = diff + 1 - backDiff;
                backDiff = 0;
            }
            if (backDiff > diff+1)
            {
                StartNewCount(a.opponent);
                return RoundLog.RoundAccolade.Comeback;
            }
            return RoundLog.RoundAccolade.None;
        }
    }
}
