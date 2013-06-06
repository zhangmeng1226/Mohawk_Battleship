using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.mbc.accolade
{
    /**
     * <summary>An accolade processor is used to process RoundActivity's in a running round.
     * Its purpose is to collect information that allows an AccoladeProcessor to determine
     * interesting moments in a competition.</summary>
     */
    public interface AccoladeProcessor
    {
        /**
         * <summary>Process an activity</summary>
         */
        RoundLog.RoundAccolade Process(RoundLog.RoundActivity a);
    }
}
