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
        IBattleshipOpponent op = null;

        private void ResetCounters()
        {
            cnt = 0;
            diff = 0;
        }

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;

            if (op == a.opponent)
            {
                diff++;
                if (diff > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_h2h_diff", 4))
                    ResetCounters();
            }
            else
            {
                op = a.opponent;
                diff = 0;
                cnt++;
            }

            if (cnt > BattleshipConfig.GetGlobalDefault().GetConfigValue<int>("accolade_h2h_count", 8))
            {
                ResetCounters();
                return RoundLog.RoundAccolade.HeadToHead;
            }

            return RoundLog.RoundAccolade.None;
        }
    }
}
