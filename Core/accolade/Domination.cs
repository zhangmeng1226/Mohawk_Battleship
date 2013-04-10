using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    public class Domination : AccoladeProcessor
    {
        static Domination()
        {
            Configuration.Default.SetValue<int>("accolade_dom_diff", 9);
        }
        int diff = 0;
        IBattleshipController last = null;

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;

            if (last != a.ibc)
            {
                diff = 0;
                last = a.ibc;
            }
            diff++;
            if (diff > Configuration.Global.GetValue<int>("accolade_dom_diff"))
            {
                diff = 0;
                return RoundLog.RoundAccolade.Domination;
            }
            return RoundLog.RoundAccolade.None;
        }
    }
}
