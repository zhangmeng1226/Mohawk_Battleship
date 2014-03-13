using MBC.Core;
using MBC.Core.Controllers;
using MBC.Core.Game;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Provides functions for creating certain objects.
    /// </summary>
    [Configuration("mbc_console_create_events", true)]
    public static class Create
    {
        /// <summary>
        /// Creates a new <see cref="Configuration"/> with a specific name and changes
        /// the application <see cref="Configuration.Global"/> <see cref="Configuration"/> with
        /// it.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Config(int idx, params string[] param)
        {
        }

        /// <summary>
        /// Replaces the <see cref="MatchRun.CurrentMatch"/> with a new <see cref="Match"/> with specific indexes
        /// given for <see cref="Shared.Controller"/>s.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Match(int idx, params string[] param)
        {
            var playControllers = new List<ControllerSkeleton>();
            int strParseInt;
            while (idx < param.Length && int.TryParse(param[idx++], out strParseInt))
            {
                if (strParseInt < 0 || strParseInt > Input.Controllers.Count)
                {
                    Console.Write("Invalid controller specified: ");
                    Console.WriteLine(strParseInt);
                }
                else
                {
                    playControllers.Add(Input.Controllers[strParseInt]);
                }
            }
            if (playControllers.Count < 2)
            {
                Console.WriteLine("A match must include a minimum of two controllers.");
                return;
            }
            try
            {
                var newMatch = new MatchServer();
                MatchRun.CurrentMatch = newMatch;

                if (Configuration.Global.GetValue<bool>("mbc_console_create_events"))
                {
                    Input.RunCommand("event enable MatchBeginEvent");
                    Input.RunCommand("event enable MatchEndEvent");
                    Input.RunCommand("event enable PlayerShotEvent");
                    Input.RunCommand("event enable PlayerWonEvent");
                }

                Console.WriteLine("Match created with:");
                foreach (var controller in playControllers)
                {
                    newMatch.AddPlayer(controller);
                    Console.WriteLine(controller);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}