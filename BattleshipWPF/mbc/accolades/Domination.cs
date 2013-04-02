using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public class Domination : AccoladeProcessor
    {
        int diff = 0;
        IBattleshipOpponent last = null;

        public override RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;

            if (last != a.opponent)
            {
                diff = 0;
                last = a.opponent;
            }
            diff++;
            if (diff > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_dom_diff", 9))
            {
                diff = 0;
                return RoundLog.RoundAccolade.Domination;
            }
            return RoundLog.RoundAccolade.None;
        }
    }
}
