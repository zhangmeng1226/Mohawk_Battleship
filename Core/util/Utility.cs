using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MBC.Core
{
    
    /// <summary>This is the framework's utility class. It has various useful functions. Use them whenever
    /// possible in order to remove code redundancy.</summary>
    public static class Utility
    {
        
        /// <summary>Used for printing verbose messages to the console while in Debug mode only.</summary>
        /// <param name="message">The message to print to the console</param>
        public static void PrintDebugMessage(string message)
        {
#if DEBUG
            string[] mLineArray = message.Split('\n');
            foreach (string line in mLineArray)
            {
                Console.WriteLine("DEBUG: " + line);
            }
#endif
        }

        
        /// <summary>Used to convert a string into an array of bytes. BytesToString() is used to reverse the
        /// process.</summary>
        /// <seealso cref="BytesToString"/>
        public static byte[] StringToBytes(string s)
        {
            var res = new byte[s.Length * sizeof(char)];
            System.Buffer.BlockCopy(s.ToCharArray(), 0, res, 0, res.Length);
            return res;
        }

        
        /// <summary>Used to convert an array of bytes into a string.</summary>
        /// <seealso cref="StringToBytes"/>
        public static string BytesToString(byte[] b)
        {
            var res = new char[b.Length / sizeof(char)];
            System.Buffer.BlockCopy(b, 0, res, 0, b.Length);
            return new string(res);
        }

        
        /// <summary>Used as a prefix to a path accessor to get to the working directory, or root.</summary>
        public static string WorkingDirectory()
        {
            string result = "";
            string[] locParts = Assembly.GetExecutingAssembly().Location.Split('\\');
            foreach(var locPart in locParts)
            {
                result += locPart + "\\";
            }
            return result + "..\\";
        }

        
        /// <summary>Generates a string that identifies a controller by its name and version.</summary>
        /// <param name="c">The class implementing the IBattleshipController interface to get the name and
        /// version from.</param>
        /// <returns>A string that contains the name and version of the specified Controller</returns>
        public static string ControllerToString(IBattleshipController c)
        {
            return c.Name + " (v" + c.Version + ")";
        }

        
        /// <summary>Generates a string that identifies a controller by its name and version.</summary>
        /// <param name="field">The field to get the controller from.</param>
        /// <param name="idx">The index within the field to get the name and version of the controller from.</param>
        /// <returns>A string that contains the name and version of the specified controller in the given Field object.</returns>
        /// <seealso cref="Field"/>
        public static string ControllerToString(Field field, int idx)
        {
            return field[idx].name + " (v" + field[idx].version + ")";
        }

        
        /// <summary>Swaps two elements in a list</summary>
        public static void ListSwap<T>(IList<T> lst, int a, int b)
        {
            T tmp = lst[a];
            lst[a] = lst[b];
            lst[b] = tmp;
        }

        private static ConsoleColor consForeground;
        private static ConsoleColor consBackground;

        
        /// <summary>Stores the current state of the console colors.</summary>
        public static void StoreConsoleColors()
        {
            consForeground = Console.ForegroundColor;
            consBackground = Console.BackgroundColor;
        }

        
        /// <summary>Restores the stored state of console colors</summary>
        public static void RestoreConsoleColors()
        {
            Console.ForegroundColor = consForeground;
            Console.BackgroundColor = consBackground;
        }

        
        /// <summary>Sets the foreground color of the Console to the one specified in the given string.</summary>
        /// <param name="colName">The name of the enum in ConsoleColor, as a string</param>
        public static void SetConsoleForegroundColor(string colName)
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colName);
        }

        
        /// <summary>Sets the background color of the Console to the one specified in the given string.</summary>
        /// <param name="colName">The name of the enum in ConsoleColor, as a string</param>
        public static void SetConsoleBackgroundColor(string colName)
        {
            Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colName);
        }
    }
}
