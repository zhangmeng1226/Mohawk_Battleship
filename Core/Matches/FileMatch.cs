using MBC.Core.Rounds;
using MBC.Core.Util;
using System;

namespace MBC.Core.Matches
{
    /// <summary>
    /// A <see cref="Match"/> that loads a previously generated <see cref="Match"/> from a resource.
    /// </summary>
    public class FileMatch : Match
    {
        /// <summary>
        /// Initializes to the <see cref="Match"/> saved in the <paramref name="fileName"/>specified.
        /// </summary>
        /// <param name="fileName"></param>
        public FileMatch(string fileName)
            : base(Configuration.Global)
        {
        }

        /// <summary>
        /// Does not create a new <see cref="Round"/> and returns null if ever invoked.
        /// </summary>
        /// <returns>Returns null.</returns>
        internal override Round CreateNewRound()
        {
            return null;
        }
    }
}