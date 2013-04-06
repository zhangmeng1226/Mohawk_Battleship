using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MBC.Core
{
    /**
     * <summary>This is the framework's utility class. It has various useful functions. Use them whenever
     * possible in order to remove code redundancy.</summary>
     */
    public static class Util
    {
        /**
         * <summary>Used for printing verbose messages while in Debug mode only.</summary>
         * <param name="message">The message to print to the console</param>
         */
        public static void PrintDebugMessage(string message)
        {
#if DEBUG
            string[] mLineArray = message.Split('\n');
            foreach (string line in mLineArray)
                Console.WriteLine("DEBUG: " + line);
#endif
        }

        /**
         * <summary>Used to convert a string into an array of bytes</summary>
         * <seealso cref="BytesToString"/>
         */
        public static byte[] StringToBytes(string s)
        {
            byte[] res = new byte[s.Length * sizeof(char)];
            System.Buffer.BlockCopy(s.ToCharArray(), 0, res, 0, res.Length);
            return res;
        }

        /**
         * <summary>Used to convert an array of bytes into a string</summary>
         * <seealso cref="StringToBytes"/>
         */
        public static string BytesToString(byte[] b)
        {
            char[] res = new char[b.Length / sizeof(char)];
            System.Buffer.BlockCopy(b, 0, res, 0, b.Length);
            return new string(res);
        }

        /**
         * <summary>Used as a prefix to a path accessor to get to the working directory, or root.</summary>
         */
        public static string WorkingDirectory()
        {
            return Assembly.GetExecutingAssembly().Location+"\\..\\";
        }

        /**
         * <returns>A string that contains the name and version of the specified controller</returns>
         */
        public static string ControllerToString(IBattleshipController c)
        {
            return c.Name + " (v" + c.Version + ")";
        }
    }
}
