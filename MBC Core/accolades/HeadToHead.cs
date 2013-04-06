using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolades
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

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;

            if (op == a.ibc)
            {
                diff++;
                if (diff > Configuration.GetGlobalDefault().GetConfigValue<int>("accolade_h2h_diff", 4))
                    ResetCounters();
            }
            else
            {
                op = a.ibc;
                diff = 0;
                cnt++;
            }

            if (cnt > Configuration.GetGlobalDefault().GetConfigValue<int>("accolade_h2h_count", 8))
            {
                ResetCounters();
                return RoundLog.RoundAccolade.HeadToHead;
            }

            return RoundLog.RoundAccolade.None;
        }
    }
}
