using MBC.Core;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.App.BattleshipConsole
{
    public delegate void MBCShellCommandHandler(int idx, params string[] param);

    public static class Input
    {
        private static List<ControllerInformation> availableControllers;
        private static SortedDictionary<string, MBCShellCommandHandler> availableCommands;
        private static SortedDictionary<string, string> availableCommandDescriptions;
        private static bool inputRunning = false;

        static Input()
        {
            availableCommands = new SortedDictionary<string, MBCShellCommandHandler>();
            availableCommandDescriptions = new SortedDictionary<string, string>();

            AddCommand(Show.Config, "show", "config", "Displays the current configuration");
            AddCommand(Show.Controllers, "show", "controllers", "Displays the currently loaded controllers and their usage IDs");

            AddCommand(Create.Match, "create", "match", "[controllerIDs...] Discards the current match (if any) and creates a new match with the specified controllers by ID as shown by \"show controllers\".");
            AddCommand(Create.Config, "create", "config", "[configname] Creates a new configuration with the specified name.");

            AddCommand(Load.Match, "load", "match", "[filename] Loads a match from a file.");
            AddCommand(Load.Config, "load", "config", "[filename] Loads a configuration from a file.");

            AddCommand(Save.Match, "save", "match", "[filename] Saves the current match to a file.");
            AddCommand(Save.Config, "save", "config", "[filename] Saves the configuration to a file.");

            AddCommand(MatchRun.Step, "match", "step", "Steps through the current match.");
            AddCommand(MatchRun.Start, "match", "start", "Plays through a match until it ends.");
            AddCommand(MatchRun.Start, "match", "stop", "Ends the current match.");

            AddCommand(EventOutput.Enable, "event", "enable", "[\"match/\"controller\"/\"round\"] Enables console display of the specified event.");
            AddCommand(EventOutput.Disable, "event", "disable", "[\"match/\"controller\"/\"round\"] Disables console display of the specified event.");

            AddCommand(Help.Display, "help", "Shows this help display.");

            AddCommand(Stop, "exit", "Exits the console application.");
        }

        public static SortedDictionary<string, string> CommandDescriptions
        {
            get
            {
                return availableCommandDescriptions;
            }
        }

        public static List<ControllerInformation> Controllers
        {
            get
            {
                return availableControllers;
            }
        }

        public static void AddCommand(MBCShellCommandHandler handler, params string[] newCmd)
        {
            if (newCmd.Length < 2)
            {
                throw new ArgumentException("AddCommand() must be provided with at least a command and a description.");
            }

            var concatCmd = new StringBuilder();
            concatCmd.Append(newCmd[0]);
            for (int i = 1; i < newCmd.Length - 1; i++)
            {
                concatCmd.Append(' ');
                concatCmd.Append(newCmd[i]);
            }
            availableCommands.Add(concatCmd.ToString(), handler);
            availableCommandDescriptions.Add(concatCmd.ToString(), newCmd[newCmd.Length - 1]);
        }

        public static bool RunCommand(params string[] input)
        {
            if (input == null || input.Length == 0)
            {
                return false;
            }
            var cmdFind = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                cmdFind.Append(input[i]);
                MBCShellCommandHandler invoke;
                if (availableCommands.TryGetValue(cmdFind.ToString(), out invoke))
                {
                    string[] parameters = new string[input.Length - i - 1];
                    while (++i < input.Length)
                    {
                        if (input[i] == "|")
                        {
                            break;
                        }
                        parameters[parameters.Length - i] = input[i];
                    }
                    invoke(0, parameters);
                    if (i == input.Length)
                    {
                        return true;
                    }
                    cmdFind.Clear();
                    continue;
                }
                cmdFind.Append(' ');
            }
            return false;
        }

        public static bool RunCommand(string input)
        {
            return RunCommand(input.Split(' '));
        }

        private static void Stop(int idx, params string[] param)
        {
            inputRunning = false;
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
                if (!RunCommand(args))
                {
                    Console.WriteLine("Invalid command specified.");
                }
                return;
            }

            Console.WriteLine("Type \"help\" for usage.");

            inputRunning = true;
            while (inputRunning)
            {
                Console.Write("\n> ");

                var consoleInput = Console.ReadLine();
                if (consoleInput != null && !RunCommand(consoleInput))
                {
                    Console.WriteLine("Invalid command specified.");
                }
            }
        }
    }
}
