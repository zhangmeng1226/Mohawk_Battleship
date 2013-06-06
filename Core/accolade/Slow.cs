using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    /// <summary>
    /// An AccoladeProcessor that determines if a round has the certain interesting characteristic:
    /// 
    ///     The number of misses each controller is making is large, thus making the round long.
    /// </summary>
    public class Slow : AccoladeProcessor
    {
        /// <summary>Sets default configuration values for keys that relate to this class.
        /// Should be called before using the global Configuration.Default object.</summary>
        /// <seealso cref="Configuration"/>
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<int>("accolade_slow_diff", 4);
            Configuration.Default.SetValue<int>("accolade_slow_cnt", 25);
        }
        int cnt = 0;
        int diff = 0;

        private void ResetCounters()
        {
            cnt = 0;
            diff = 0;
        }

        /// <summary>
        ///  Processes a RoundActivity to determine a certain round characteristic.
        /// </summary>
        /// <param name="a">The Activity generated for a round.</param>
        /// <returns>A RoundAccolade that describes the characteristic earned at the moment.</returns>
        public RoundLog.RoundAccolade Process(RoundLog.RoundActivity a)
        {
            if (a.action != RoundLog.RoundAction.ShotAndHit && a.action != RoundLog.RoundAction.ShotAndMiss)
                return RoundLog.RoundAccolade.None;

            if (a.action == RoundLog.RoundAction.ShotAndHit)
            {
                diff++;
                if (diff > Configuration.Global.GetValue<int>("accolade_slow_diff"))
                    ResetCounters();
            }
            else
            {
                cnt++;
                diff = 0;
            }

            if (cnt > Configuration.Global.GetValue<int>("accolade_slow_cnt"))
            {
                ResetCounters();
                return RoundLog.RoundAccolade.Slow;
            }

            return RoundLog.RoundAccolade.None;
        }
    }
}
