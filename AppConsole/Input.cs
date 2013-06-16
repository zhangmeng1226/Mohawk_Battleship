using MBC.Core;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    public static class Input
    {
        private static List<ControllerInformation> availableControllers;

        public static bool InputRunning { get; set; }

        public static List<ControllerInformation> Controllers
        {
            get
            {
                return availableControllers;
            }
        }

        public static void Parse(string[] commandParams)
        {
            try
            {
                for (var cmdIdx = 0; cmdIdx < commandParams.Length; cmdIdx++)
                {
                    switch (commandParams[cmdIdx++].ToLower())
                    {
                        case "list":
                        case "show":
                            Show.Parse(commandParams, ref cmdIdx);
                            break;
                        case "exit":
                            InputRunning = false;
                            break;
                        case "match":
                            MatchRun.Parse(commandParams, ref cmdIdx);
                            break;
                        case "start":
                        case "begin":
                            MatchRun.Start();
                            break;
                        case "step":
                            MatchRun.Step();
                            break;
                        case "set":
                            Set.Parse(commandParams, ref cmdIdx);
                            break;
                        case "help":
                            Help.Display();
                            break;
                        default:
                            Console.WriteLine("Invalid command.");
                            return;
                    }
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Incorrect number of parameters.");
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "MBC console";
            Console.CancelKeyPress += (a, b) =>
            {
                MatchRun.Running = false;
            };

            Environment.CurrentDirectory = Environment.CurrentDirectory + "\\..";

            Configuration.InitializeConfiguration(Environment.CurrentDirectory + "\\configs");
            ControllerInformation.LoadControllerFolder(Environment.CurrentDirectory + "\\bots");

            availableControllers = ControllerInformation.AvailableControllers;

            if (args.Length != 0)
            {
                Parse(args);
                return;
            }

            Console.WriteLine("Type \"help\" for usage.");

            InputRunning = true;
            while (InputRunning)
            {
                Console.Write("\n> ");

                var consoleInput = Console.ReadLine();
                Console.WriteLine(consoleInput);
                if (consoleInput != null)
                {
                    var inputSplit = consoleInput.Split(' ');

                    Parse(inputSplit);
                }
            }
        }
    }
}
