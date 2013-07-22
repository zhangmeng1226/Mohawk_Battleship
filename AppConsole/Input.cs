using MBC.Core;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Defines a method that is given the current <paramref name="idx"/> in the <paramref name="param"/> stream.
    /// </summary>
    /// <param name="idx">The current index in the <paramref name="param"/> array.</param>
    /// <param name="param">An array of strings that make up a command.</param>
    public delegate void MBCShellCommandHandler(int idx, params string[] param);
    
    /// <summary>
    /// <para>The main part of the AppConsole that stores command strings that are associated
    /// with <see cref="MBCShellCommandHandler"/>s, which perform specific tasks. The
    /// strings that are input are then parsed to invoke the stored commands.</para>
    /// <para>The commands modify the internal state of this particular class,
    /// specifically storing the controllers that are available.</para>
    /// </summary>
    public static class Input
    {
        private static SortedDictionary<string, string> availableCommandDescriptions;
        private static SortedDictionary<string, MBCShellCommandHandler> availableCommands;
        private static List<ControllerInformation> availableControllers;
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

            AddCommand(Set.Config, "set", "config", "[key] [value] Sets a configuration key value to the one specified.");

            AddCommand(EventOutput.Enable, "event", "enable", "[\"match\"/\"controller\"/\"round\"] Enables console display of the specified event.");
            AddCommand(EventOutput.Disable, "event", "disable", "[\"match\"/\"controller\"/\"round\"] Disables console display of the specified event.");

            AddCommand(Help.Commands, "help", "Shows this help display.");

            AddCommand(Stop, "exit", "Exits the console application.");
        }

        /// <summary>
        /// Gets the dictionary of command strings associated with a description.
        /// </summary>
        public static SortedDictionary<string, string> CommandDescriptions
        {
            get
            {
                return availableCommandDescriptions;
            }
        }

        /// <summary>
        /// Gets a list of <see cref="ControllerInformation"/> loaded by commands.
        /// </summary>
        public static List<ControllerInformation> Controllers
        {
            get
            {
                return availableControllers;
            }
        }

        /// <summary>
        /// Associates a number of <paramref name="newCmd"/>s to a <paramref name="handler"/>.
        /// </summary>
        /// <param name="handler">The method to invoke from <paramref name="newCmd"/></param>
        /// <param name="newCmd">The strings that invoke the <paramref name="handler"/>.</param>
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

        /// <summary>
        /// Runs a command, or a series of commands separated by a pipe "|" and passes the
        /// current index of the array of strings to the command handler to parse parameters
        /// when needed.
        /// </summary>
        /// <param name="input">The array of strings that make up commands.</param>
        /// <returns>A value indicating the successful completion of the
        /// command(s).</returns>
        public static bool RunCommand(params string[] input)
        {
            if (input == null || input.Length == 0)
            {
                return false;
            }
            var cmdFind = new StringBuilder();

            try
            {
                for (var i = 0; i < input.Length; i++)
                {
                    cmdFind.Append(input[i]);
                    MBCShellCommandHandler invoke;
                    if (availableCommands.TryGetValue(cmdFind.ToString(), out invoke))
                    {
                        var parameters = new List<string>();
                        while (++i < input.Length)
                        {
                            if (input[i] == "|")
                            {
                                break;
                            }
                            parameters.Add(input[i]);
                        }
                        invoke(0, parameters.ToArray());
                        if (i == input.Length)
                        {
                            return true;
                        }
                        cmdFind.Clear();
                        continue;
                    }
                    cmdFind.Append(' ');
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Invalid command(s) specified.");
            }
            return false;
        }

        /// <summary>
        /// Splits a string by whitespaces without splitting the strings encapsulated with
        /// double-quotes. Runs the <see cref="Input.RunCommand(string[])"/> method with
        /// the parsed <paramref name="input"/>.
        /// </summary>
        /// <param name="input">A string of input.</param>
        /// <returns>A value indicating whether or not the command(s) entered were
        /// successfully completed.</returns>
        public static bool RunCommand(string input)
        {
            var quoteSplits = input.Split('\"');
            var compiledSplitsList = new List<string>();
            for (var i = 0; i < quoteSplits.Length; i++)
            {
                if (i % 2 == 0)
                {
                    compiledSplitsList.AddRange(quoteSplits[i].Trim().Split(' '));
                }
                else
                {
                    compiledSplitsList.Add(quoteSplits[i]);
                }
            }
            return RunCommand(compiledSplitsList.ToArray());
        }

        /// <summary>
        /// AppConsole entry point
        /// </summary>
        /// <param name="args">Arguments to be specified for running commands as batch mode.</param>
        private static void Main(string[] args)
        {
            //Set up the console
            Console.Title = "MBC console";
            Console.CancelKeyPress += (a, b) =>
            {
                MatchRun.Running = false;
            };
            Configuration.Initialize(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"\\MBC Data");

            //Load controllers from the application data directory.
            availableControllers = ControllerInformation.LoadControllerFolder(
                Configuration.Global.GetValue<string>("app_data_root") + "controllers");
            //Load controllers from the running application root directory.
            availableControllers.AddRange(ControllerInformation.LoadControllerFolder(Environment.CurrentDirectory + "\\..\\bots"));

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

        /// <summary>
        /// Stops input.
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="param"></param>
        private static void Stop(int idx, params string[] param)
        {
            inputRunning = false;
        }
    }
}