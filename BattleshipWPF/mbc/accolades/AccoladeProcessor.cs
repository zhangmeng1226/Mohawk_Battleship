using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.mbc.accolades
{
    public interface AccoladeProcessor
    {
        /**
         * <summary>Process an activity</summary>
         */
        MBCRoundLog.RoundAccolade Process(MBCRoundLog.RoundActivity a);
    }
}
