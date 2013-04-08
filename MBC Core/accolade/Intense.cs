using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    public class Intense : AccoladeProcessor
    {
        static Intense()
        {
            Configuration.Default.SetValue<int>("accolade_intense_diff", 4);
            Configuration.Default.SetValue<int>("accolade_intense_count", 8);
        }
        Dictionary<IBattleshipController, int> hits = new Dictionary<IBattleshipController,int>();
        int cnt = 0;

        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;
            hits[a.ibc]++;

            if (Math.Abs(hits.Values.ElementAt(0) - hits.Values.ElementAt(1)) > 
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
