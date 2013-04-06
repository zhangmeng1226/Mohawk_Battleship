using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolades
{
    public class Fast : AccoladeProcessor
    {
        static Fast()
        {
            Configuration def = Configuration.Default;
            def.SetConfigValue<int>("accolade_fast_absent", 4);
            def.SetConfigValue<int>("accolade_fast_diff", 10);
        }
        int diff = 0;
        int absent = 0;

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndMiss && a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;
            if (a.action != RoundLog.RoundAction.ShotAndHit)
            {
                if (++absent > Configuration.Global.GetConfigValue<int>("accolade_fast_absent"))
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

            if (diff > Configuration.Global.GetConfigValue<int>("accolade_fast_diff"))
            {
                diff = 0;
                return RoundLog.RoundAccolade.Fast;
            }
            return RoundLog.RoundAccolade.None;
        }
    }
}
