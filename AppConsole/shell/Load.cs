using MBC.Core.Util;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Provides functions for loading data from files.
    /// </summary>
    public static class Load
    {
        /// <summary>
        /// Loads a <see cref="Configuration"/> file with a specific name to the <see cref="Configuration.Global"/> <see cref="Configuration"/>.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Config(int idx, params string[] param)
        {
            Configuration loadConf = new Configuration(param[idx++]);
        }

        /// <summary>
        /// Loads a <see cref="Match"/> from a specific file name into the <see cref="MatchRun.CurrentMatch"/>.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Match(int idx, params string[] param)
        {
        }
    }
}