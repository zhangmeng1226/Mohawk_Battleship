using MBC.Core;
using MBC.Core.Util;
using System;
using System.Collections.Generic;

namespace MBC.App.BattleshipConsole
{
    [Configuration("mbc_console_create_events", true)]
    public static class Create
    {
        public static void Config(int idx, params string[] param)
        {
        }

        public static void Match(int idx, params string[] param)
        {
            var playControllers = new List<ControllerInformation>();
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
                MatchRun.CurrentMatch = new Match(Configuration.Global, playControllers.ToArray());

                if (Configuration.Global.GetValue<bool>("mbc_console_create_events"))
                {
                    Input.RunCommand("event enable match");
                    Input.RunCommand("event enable controller");
                    Input.RunCommand("event enable round");
                }

                Console.WriteLine("Match created with:");
                foreach (var controller in playControllers)
                {
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