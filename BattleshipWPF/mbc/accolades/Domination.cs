using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public class Domination : AccoladeProcessor
    {
        int diff = 0;
        IBattleshipController last = null;

        public MBCRoundLog.RoundAccolade Process(MBCRoundLog.RoundActivity a)
        {
            if (a.action != MBCRoundLog.RoundAction.ShotAndHit)
                return MBCRoundLog.RoundAccolade.None;

            if (last != a.ibc)
            {
                diff = 0;
                last = a.ibc;
            }
            diff++;
            if (diff > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_dom_diff", 9))
            {
                diff = 0;
                return MBCRoundLog.RoundAccolade.Domination;
            }
            return MBCRoundLog.RoundAccolade.None;
        }
    }
}
