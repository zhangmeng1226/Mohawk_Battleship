using System;
using MBC.Core.Util;

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
            Configuration.Global.SaveConfigFile();
            Console.WriteLine("Saved the current configuration (" + Configuration.Global.Name + ")");
        }

        /// <summary>
        /// Saves the <see cref="MatchRun.CurrentMatch"/> to a file.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Match(int idx, params string[] param)
        {
            var fileName = param[idx++];
            MatchRun.CurrentMatch.SaveToFile(fileName);
            Console.WriteLine("Saved the current match as " + fileName);
        }
    }
}