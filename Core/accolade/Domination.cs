using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    /// <summary>
    /// An AccoladeProcessor that determines if a round has the certain interesting characteristic:
    /// 
    ///     A controller has a significantly larger number of ship hits than the opposing controller.
    /// </summary>
    public class Domination : AccoladeProcessor
    {
        /// <summary>Sets default configuration values for keys that relate to this class.
        /// Should be called before using the global Configuration.Default object.</summary>
        /// <seealso cref="Configuration"/>
        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<int>("accolade_dom_diff", 9);
        }
        int diff = 0;
        int last = Controller.None;

        /// <summary>
        ///  Processes a RoundActivity to determine a certain round characteristic.
        /// </summary>
        /// <param name="a">The Activity generated for a round.</param>
        /// <returns>A RoundAccolade that describes the characteristic earned at the moment.</returns>
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
