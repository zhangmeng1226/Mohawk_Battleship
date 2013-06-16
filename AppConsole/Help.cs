using System;

namespace MBC.App.BattleshipConsole
{
    public static class Help
    {
        public static void Display(int idx, params string[] param)
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
                var descSplit = commandDescPair.Value.Split(' ');
                var targetWidth = Console.WindowWidth - largestLength;
                var currentWidth = 0;
                for (var descIdx = 0; descIdx < descSplit.Length; descIdx++)
                {
                    currentWidth += descSplit[descIdx].Length + 1;
                    if (currentWidth > targetWidth)
                    {
                        Console.WriteLine();
                        Console.SetCursorPosition(largestLength, Console.CursorTop);
                        currentWidth = descSplit[descIdx].Length + 1;
                    }
                    Console.Write(' ');
                    Console.Write(descSplit[descIdx]);
                }
                Console.WriteLine();
            }
        }
    }
}