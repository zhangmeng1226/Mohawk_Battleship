using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    public class Comeback : AccoladeProcessor
    {
        /**
         * <summary>Sets default configuration values for keys that relate to this class.
         * Should be called before using the global Configuration.Default object.</summary>
         */
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<int>("accolade_comeback_diff", 8);
        }
        int diff = 0;
        int backDiff = 0;
        int last = Controller.None;

        private void StartNewCount(int op)
        {
            diff = 0;
            backDiff = 0;
            last = op;
        }

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;

            if (last != a.ibc)
            {
                if (diff > Configuration.Global.GetValue<int>("accolade_comeback_diff"))
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
                return RoundLog.RoundAccolade.Comeback;
            }
            return RoundLog.RoundAccolade.None;
        }
    }
}
