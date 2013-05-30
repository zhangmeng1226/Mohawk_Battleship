using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Core;

using MBC.App.Terminal.Layouts;
using MBC.App.Terminal.Controls;

namespace MBC.App.Terminal
{
    /**
     * <summary>The base class for a TerminalModule. Used for providing different states of the terminal
     * application.
     * 
     * Provides various methods to display information in a TerminalModule buffer. Direct output via the
     * Console class is not to be used.
     * 
     * 
     * </summary>
     */
    public abstract class TerminalModule
    {
        private int writePosX = 0, writePosY = 0, writeWidth = 0, writeHeight = 0;
        private int curX = 0, curY = 0;
        private List<ControlLayout> controls = new List<ControlLayout>();
        private ControlLayout currentControlLayout = new NoLayout();

        protected void AddControlLayout(ControlLayout ctrl)
        {
            ctrl.SetModule(this);
            controls.Add(ctrl);
            currentControlLayout = controls[0];
        }

        protected void RemoveControlLayout(ControlLayout ctrl)
        {
            PreviousLayout();
            controls.Remove(ctrl);
        }

        public void NextLayout()
        {
            if (controls == null)
            {
                return;
            }
            int next = controls.IndexOf(currentControlLayout) + 1;
            if (next >= controls.Count)
            {
                next = 0;
            }
            currentControlLayout = controls[next];
            currentControlLayout.Select();
        }

        public void PreviousLayout()
        {
            if (controls == null)
            {
                return;
            }
            int prev = controls.IndexOf(currentControlLayout) - 1;
            if (prev < 0)
            {
                prev = controls.Count - 1;
            }
            currentControlLayout = controls[prev];
            currentControlLayout.Select();
        }

        public bool IsLayoutSelected(ControlLayout layout)
        {
            return currentControlLayout == layout;
        }

        /**
         * <summary>Gets the width of characters this ConsoleModule can write</summary>
         */
        public int Width
        {
            get { return writeWidth; }
        }

        /**
         * <summary>Gets the height of characters this ConsoleModule can write</summary>
         */
        public int Height
        {
            get { return writeHeight; }
        }

        public int WriteX
        {
            get { return writePosX; }
        }

        public int WriteY
        {
            get { return writePosY; }
        }

        public int CurrentX
        {
            get { return curX; }
        }

        public int CurrentY
        {
            get { return curY; }
        }

        /**
         * <summary>Aligns the cursor to the specified coords for this buffer</summary>
         * <returns>True if the cursor position is out of bounds</returns>
         */
        public bool AlignToCoord(int x, int y)
        {
            if (x > writeWidth)
            {
                curX = 0;
                curY = y + 1;
            }
            else
            {
                curX = x;
                curY = y;
            }
            Align();
            return curY > writeHeight;
        }

        /**
         * <summary>Aligns the cursor to the specified line for this buffer</summary>
         * <returns>True if the cursor position is out of bounds</returns>
         */
        public bool AlignToLine(int line)
        {
            curX = 0;
            curY = line;
            Align();
            return curY > writeHeight;
        }

        /**
         * <summary>Aligns the cursor to the last position used.</summary>
         */
        public void Align()
        {
            Console.SetCursorPosition(writePosX + curX, writePosY + curY);
        }

        /**
         * <summary>Moves the cursor down a line. Optionally provide a number to move down that many lines.</summary>
         */
        public void NewLine(int n = 1)
        {
            curY += n;
            curX = 0;
            Align();
        }
        /**
         * <summary>Writes characters to the terminal. Wraps text around if the buffer is too small.</summary>
         */
        public void WriteChars(char[] c)
        {
            int a = 0;
            while (a < c.Length)
            {
                if (curY > writeHeight)
                    return;
                int avail = writeWidth - curX;
                if (avail - (c.Length-a) <= 0)
                {
                    Console.Write(c, a, avail);
                    a += avail;
                    NewLine();
                }
                else
                {
                    Console.Write(c, a, c.Length - a);
                    a += c.Length;
                    curX += c.Length;
                }
            }
        }

        /**
         * <summary>Called when a key is pressed in the console.</summary>
         */
        public bool KeyPressed(ConsoleKeyInfo key)
        {
            Util.StoreConsoleColors();
            bool result = currentControlLayout.KeyPressed(key);
            Util.RestoreConsoleColors();
            return result;
        }

        /**
         * <summary>Called when a string has been entered in the console.</summary>
         */
        public void InputEntered(string input)
        {
            Util.StoreConsoleColors();
            currentControlLayout.Input(input);
            Util.RestoreConsoleColors();
        }

        /**
         * <summary>Writes a specific character for a number of times.</summary>
         * <param name="c">The character to print to the console.</param>
         * <param name="n">The number of times to print the character</param>
         */
        public void WriteCharRepeat(char c, int n)
        {
            char[] cs = new char[n];
            for (int i = 0; i < n; i++)
                cs[i] = c;
            WriteChars(cs);
        }

        /**
         * <summary>Prints the specified text centered to the console.</summary>
         * <param name="text">The text to print centered.</param>
         * <param name="ends">The ends used for styling.</param>
         */
        public void WriteCenteredText(string text)
        {
            int maxChars = writeWidth;
            int count = 0;
            while (count < text.Length)
            {
                int printLen = text.Length - count > maxChars ? maxChars : text.Length - count;
                int cen = maxChars / 2 - printLen / 2;

                AlignToCoord(cen, curY);

                WriteChars(text.Substring(count, printLen).ToCharArray());

                count += printLen;
            }
        }

        /**
         * <summary>Writes text to the console.</summary>
         */
        public void WriteText(string txt, params object[] rst)
        {
            WriteChars(String.Format(txt, rst).ToCharArray());
        }

        /**
         * <summary>Refreshes the display of this module while updating the bounds of the writing area.</summary>
         */
        public void RefreshDisplay(int x, int y, int w, int h)
        {
            Util.StoreConsoleColors();
            writePosX = x;
            writePosY = y;
            writeWidth = w;
            writeHeight = h;
            curX = 0;
            curY = 0;
            Align();
            Display();
            Util.RestoreConsoleColors();
        }

        /**
         * <summary>Refresh the display of this module.</summary>
         */
        protected abstract void Display();
    }
}
