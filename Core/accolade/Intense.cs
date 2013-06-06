using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    /// <summary>
    /// An AccoladeProcessor that determines if a round has the certain interesting characteristic:
    /// 
    ///     Throughout the round, both controllers have a similar number of ship hits.
    /// </summary>
    public class Intense : AccoladeProcessor
    {
        /// <summary>Sets default configuration values for keys that relate to this class.
        /// Should be called before using the global Configuration.Default object.</summary>
        /// <seealso cref="Configuration"/>
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<int>("accolade_intense_diff", 4);
            Configuration.Default.SetValue<int>("accolade_intense_count", 8);
        }
        int[] hits = new int[2];
        int cnt = 0;

        /// <summary>
        ///  Processes a RoundActivity to determine a certain round characteristic.
        /// </summary>
        /// <param name="a">The Activity generated for a round.</param>
        /// <returns>A RoundAccolade that describes the characteristic earned at the moment.</returns>
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
