using MBC.Core;
using MBC.Core.Events;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    [Configuration("mbc_console_start_show_events", true)]
    public static class MatchRun
    {
        public static Match CurrentMatch { get; set; }
        public static bool Running { get; set; }

        public static void Step()
        {
            if (CurrentMatch == null)
            {
                Console.WriteLine("No match has been created.");
                return;
            }

            if (!CurrentMatch.Step())
            {
                Console.WriteLine("The match is over.");
            }
        }

        public static void Start()
        {
            if (CurrentMatch == null)
            {
                Console.WriteLine("No match has been created.");
                return;
            }
            Running = true;
            while (Running && CurrentMatch.Step()) ;
            Console.WriteLine("The match has stopped.");
        }

        public static void Stop()
        {
            CurrentMatch.End();
            Console.WriteLine("The match has been stopped and removed.");
        }

        public static void Parse(string[] commandParams, ref int idx)
        {
            var playControllers = new List<ControllerInformation>();
            int strParseInt;
            while (idx < commandParams.Length && int.TryParse(commandParams[idx++], out strParseInt))
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
                CurrentMatch = new Match(Input.Configuration, playControllers.ToArray());

                if (Input.Configuration.GetValue<bool>("mbc_console_start_show_events"))
                {
                    CurrentMatch.RoundEvent += (ev) =>
                        {
                            Console.WriteLine(ev);
                        };
                    CurrentMatch.MatchEvent += (ev) =>
                        {
                            Console.WriteLine(ev);
                        };
                    CurrentMatch.ControllerEvent += (ev) =>
                        {
                            Console.WriteLine(ev);
                        };
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
