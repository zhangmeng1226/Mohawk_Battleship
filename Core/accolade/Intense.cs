using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    public class Intense : AccoladeProcessor
    {
        /**
         * <summary>Sets default configuration values for keys that relate to this class.
         * Should be called before using the global Configuration.Default object.</summary>
         */
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<int>("accolade_intense_diff", 4);
            Configuration.Default.SetValue<int>("accolade_intense_count", 8);
        }
        int[] hits = new int[2];
        int cnt = 0;

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;
            hits[a.ibc]++;

            if (Math.Abs(hits[0] - hits[1]) > 
                Configuration.Global.GetValue<int>("accolade_intense_diff"))
                cnt = 0;
            else
                cnt++;

            if (cnt > Configuration.Global.GetValue<int>("accolade_intense_count"))
            {
                cnt = 0;
                return RoundLog.RoundAccolade.Intense;
            }

            return RoundLog.RoundAccolade.None;
        }
    }
}
