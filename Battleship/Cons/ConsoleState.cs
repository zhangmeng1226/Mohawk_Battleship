using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship.Cons
{
    /**
     * <summary>The base class for console states</summary>
     */
    public abstract class ConsoleState
    {
        protected BattleshipConsole main; //Reference to the 'parent'
        protected string extraMenu = ""; //Extra key options available to the user.
        protected const string headerEnds = "====================="; //Styling used for header ends

        public ConsoleState(BattleshipConsole main)
        {
            this.main = main;
        }

        /**
         * <summary>Writes a specific character for a number of times.</summary>
         * <param name="c">The character to print to the console.</param>
         * <param name="n">The number of times to print the character</param>
         */
        protected void WriteChars(char c, int n)
        {
            for (int i = 0; i < n; i++)
                Console.Write(c);
        }

        /**
         * <summary>Called in the loop, used for input interactivity.</summary>
         * <param name="input">The string entered by the user into the console</param>
         */
        public ConsoleState GetInput(string input)
        {
            switch (input)
            {
                case "E": //Exit key
                    return null;
                case "C": //Quit key
                    return this;
            }
            return Response(input);
        }

        /**
         * <summary>Prints the specified text, with the specified ends, centered, to the console.</summary>
         * <param name="text">The text to print centered.</param>
         * <param name="ends">The ends used for styling.</param>
         */
        public void WriteCenteredText(string text, string ends)
        {
            int maxChars = Console.WindowWidth - ends.Length * 2;
            int count = 0;
            while (count < text.Length)
            {
                int printLen = text.Length - count > maxChars ? maxChars : text.Length - count;
                int cen = maxChars / 2 - printLen / 2;
                //Program.PrintDebugMessage("" + cen);

                Console.Write(ends);
                WriteChars(' ', cen);

                Console.Write(text.Substring(count, printLen));

                WriteChars(' ', Console.WindowWidth - (ends.Length * 2 + cen + printLen));
                Console.Write(ends);
                count += printLen;
            }
        }

        /**
         * <summary>Called in the loop, this provides a standard way for user interactivity and display.</summary>
         */
        public void Display()
        {
            Console.Clear();
            WriteCenteredText("BATTLESHIP COMPETITION FRAMEWORK", headerEnds);
            StateDisplay();
            WriteCenteredText("[E]xit, [C]onfigure" + (extraMenu == "" ? "" : ", ") + extraMenu, "===");
            Console.Write("Input : ");
        }

        /**
         * <summary>Overridden to provide specific printing related to the subclass.</summary>
         */
        protected abstract void StateDisplay();

        /**
         * <summary>Called when input is not handled by the base class.</summary>
         * <param name="input">The string entered by the user</param>
         * <returns>The implementing class, null, or a new ConsoleState object. null will end the program.</returns>
         */
        protected abstract ConsoleState Response(string input);
    }
}
