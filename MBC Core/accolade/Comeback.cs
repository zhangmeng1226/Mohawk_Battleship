using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    public class Comeback : AccoladeProcessor
    {
        static Comeback()
        {
            Configuration.Default.SetValue<int>("accolade_comeback_diff", 8);
        }
        int diff = 0;
        int backDiff = 0;
        IBattleshipController last = null;

        private void StartNewCount(IBattleshipController op)
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
