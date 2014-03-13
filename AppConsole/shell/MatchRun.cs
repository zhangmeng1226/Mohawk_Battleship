using MBC.Core.Game;
using MBC.Core.Util;
using MBC.Shared;
using System;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Provides functions and variables for running a <see cref="Match"/>.
    /// </summary>
    [Configuration("mbc_console_match_show_events", true)]
    public static class MatchRun
    {
        /// <summary>
        /// Gets or sets <see cref="Match"/> that is currently being used in the application.
        /// </summary>
        public static MatchServer CurrentMatch { get; set; }

        /// <summary>
        /// Begins running the <see cref="MatchRun.CurrentMatch"/> continuously until it ends, or
        /// <see cref="MatchRun.Running"/> is made false.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Start(int idx, params string[] param)
        {
            if (CurrentMatch == null)
            {
                Console.WriteLine("No match has been created.");
                return;
            }
            Console.CancelKeyPress += (a, b) =>
            {
                CurrentMatch.Stop();
                Console.WriteLine("The match has been stopped.");
            };
            CurrentMatch.Play();
            Console.WriteLine("The match has been stopped.");
        }

        /// <summary>
        /// Stops and ends the <see cref="MatchRun.CurrentMatch"/>.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Stop(int idx, params string[] param)
        {
            CurrentMatch.Stop();
            Console.WriteLine("The match has been stopped.");
        }
    }
}