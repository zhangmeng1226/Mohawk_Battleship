using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    /// <summary>
    /// An AccoladeProcessor that determines if a round has the certain interesting characteristic:
    /// 
    ///     Both controllers are making ship hits one after another with little difference.
    /// </summary>
    public class HeadToHead : AccoladeProcessor
    {
        /// <summary>Sets default configuration values for keys that relate to this class.
        /// Should be called before using the global Configuration.Default object.</summary>
        /// <seealso cref="Configuration"/>
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<int>("accolade_h2h_diff", 4);
            Configuration.Default.SetValue<int>("accolade_h2h_count", 8);
        }
        int cnt = 0;
        int diff = 0;
        int op = Controller.None;

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
            if (a.action != RoundLog.RoundAction.ShotAndHit)
                return RoundLog.RoundAccolade.None;

            if (op == a.ibc)
            {
                diff++;
                if (diff > Configuration.Global.GetValue<int>("accolade_h2h_diff"))
                    ResetCounters();
            }
            else
            {
                op = a.ibc;
                diff = 0;
                cnt++;
            }

            if (cnt > Configuration.Global.GetValue<int>("accolade_h2h_count"))
            {
                ResetCounters();
                return RoundLog.RoundAccolade.HeadToHead;
            }

            return RoundLog.RoundAccolade.None;
        }
    }
}
