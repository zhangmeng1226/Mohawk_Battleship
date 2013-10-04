using MBC.Core.Rounds;

namespace MBC.Core.Accolades
{
    /// <summary>
    /// Analyses the <see cref="MBC.Core.Events.Event"/>s that are generated from within a <see cref="Round"/>, generates
    /// <see cref="Accolade"/>s based on the data collected through event occurrences through <see cref="Events.RoundEvent"/>
    /// and <see cref="Events.PlayerEvent"/>, and adds these <see cref="Accolade"/>s to the <see cref="Round"/>.
    /// </summary>
    public class AccoladeGenerator
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="round"></param>
        public AccoladeGenerator(Round round)
        {
        }
    }
}