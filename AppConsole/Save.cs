namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Contains functions that are used for saving data to files.
    /// </summary>
    public static class Save
    {
        /// <summary>
        /// Saves the <see cref="Core.Util.Configuration.Global"/> <see cref="Core.Util.Configuration"/> to a file.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Config(int idx, params string[] param)
        {
        }

        /// <summary>
        /// Saves the <see cref="MatchRun.CurrentMatch"/> to a file.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Match(int idx, params string[] param)
        {
        }
    }
}