using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    public class Fast : AccoladeProcessor
    {
        /**
         * <summary>Sets default configuration values for keys that relate to this class.
         * Should be called before using the global Configuration.Default object.</summary>
         */
        public static void SetConfigDefaults()
        {
            Configuration def = Configuration.Default;
            def.SetValue<int>("accolade_fast_absent", 4);
            def.SetValue<int>("accolade_fast_diff", 10);
        }
        int diff = 0;
        int absent = 0;

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndMiss && a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;
            if (a.action != RoundLog.RoundAction.ShotAndHit)
            {
                if (++absent > Configuration.Global.GetValue<int>("accolade_fast_absent"))
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

            if (diff > Configuration.Global.GetValue<int>("accolade_fast_diff"))
            {
                diff = 0;
                return RoundLog.RoundAccolade.Fast;
            }
            return RoundLog.RoundAccolade.None;
        }
    }
}
