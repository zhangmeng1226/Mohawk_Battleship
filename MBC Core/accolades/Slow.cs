using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolades
{
    public class Slow : AccoladeProcessor
    {
        static Slow()
        {
            Configuration.Default.SetConfigValue<int>("accolade_slow_diff", 4);
            Configuration.Default.SetConfigValue<int>("accolade_slow_cnt", 25);
        }
        int cnt = 0;
        int diff = 0;

        private void ResetCounters()
        {
            cnt = 0;
            diff = 0;
        }

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit && a.action != RoundLog.RoundAction.ShotAndMiss)
                return RoundLog.RoundAccolade.None;

            if (a.action == RoundLog.RoundAction.ShotAndHit)
            {
                diff++;
                if (diff > Configuration.Global.GetConfigValue<int>("accolade_slow_diff"))
                    ResetCounters();
            }
            else
            {
                cnt++;
                diff = 0;
            }

            if (cnt > Configuration.Global.GetConfigValue<int>("accolade_slow_cnt"))
            {
                ResetCounters();
                return RoundLog.RoundAccolade.Slow;
            }

            return RoundLog.RoundAccolade.None;
        }
    }
}
