using System;

namespace MBC.App.BattleshipConsole
{
    /// <summary>
    /// Provides functions to display help information to the console.
    /// </summary>
    public static class Help
    {
        /// <summary>
        /// Displays the <see cref="Input.CommandDescriptions"/> of all existing commands.
        /// </summary>
        /// <param name="idx">The current index of the parameter stream.</param>
        /// <param name="param">The string of parameters made by the user.</param>
        public static void Commands(int idx, params string[] param)
        {
            var largestLength = 0;
            foreach (var command in Input.CommandDescriptions.Keys)
            {
                if (command.Length > largestLength)
                {
                    largestLength = command.Length;
                }
            }

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Command usage for the MBC console application:");
            Console.BackgroundColor = ConsoleColor.Black;

            foreach (var commandDescPair in Input.CommandDescriptions)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(commandDescPair.Key);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(largestLength, Console.CursorTop);
                Show.ProperLine(largestLength, commandDescPair.Value);
                Console.WriteLine();
            }
        }
    }
}