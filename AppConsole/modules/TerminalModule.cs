using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;

namespace MBC.App.Terminal.Modules
{
    /// <summary>
    /// The base class for a TerminalModule. Used for providing different states of the terminal
    /// application.
    ///
    /// Provides various methods to display information in a TerminalModule buffer. Direct output via the
    /// Console class is not to be used.
    /// </summary>
    public abstract class TerminalModule
    {
        private List<ControlLayout> controls = new List<ControlLayout>();
        private ControlLayout currentControlLayout = new NoLayout();
        private int curX = 0, curY = 0;
        private int writePosX = 0, writePosY = 0, writeWidth = 0, writeHeight = 0;

        public int CurrentX
        {
            get { return curX; }
        }

        public int CurrentY
        {
            get { return curY; }
        }

        public int Height
        {
            get { return writeHeight; }
        }

        public int Width
        {
            get { return writeWidth; }
        }

        public int WriteX
        {
            get { return writePosX; }
        }

        public int WriteY
        {
            get { return writePosY; }
        }

        public void Align()
        {
            Console.SetCursorPosition(writePosX + curX, writePosY + curY);
        }

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

        public bool AlignToLine(int line)
        {
            curX = 0;
            curY = line;
            Align();
            return curY > writeHeight;
        }

        public void InputEntered(string input)
        {
            var storedColors = ColorStore.StoreCurrentColors();
            currentControlLayout.Input(input);
            storedColors.Restore();
        }

        public bool IsLayoutSelected(ControlLayout layout)
        {
            return currentControlLayout == layout;
        }

        public bool KeyPressed(ConsoleKeyInfo key)
        {
            var storedColors = ColorStore.StoreCurrentColors();
            bool result = currentControlLayout.KeyPressed(key);
            storedColors.Restore();
            return result;
        }

        public void NewLine(int n = 1)
        {
            curY += n;
            curX = 0;
            Align();
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

        public void RefreshDisplay(int x, int y, int w, int h)
        {
            var storedColors = ColorStore.StoreCurrentColors();
            writePosX = x;
            writePosY = y;
            writeWidth = w;
            writeHeight = h;
            curX = 0;
            curY = 0;
            Align();
            Display();
            storedColors.Restore();
        }

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

        public void WriteCharRepeat(char c, int n)
        {
            char[] cs = new char[n];
            for (int i = 0; i < n; i++)
                cs[i] = c;
            WriteChars(cs);
        }

        public void WriteChars(char[] c)
        {
            int a = 0;
            while (a < c.Length)
            {
                if (curY > writeHeight)
                    return;
                int avail = writeWidth - curX;
                if (avail - (c.Length - a) <= 0)
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

        public void WriteText(string txt, params object[] rst)
        {
            WriteChars(String.Format(txt, rst).ToCharArray());
        }

        protected void AddControlLayout(ControlLayout ctrl)
        {
            ctrl.SetModule(this);
            controls.Add(ctrl);
            currentControlLayout = controls[0];
        }

        protected abstract void Display();

        protected void RemoveControlLayout(ControlLayout ctrl)
        {
            PreviousLayout();
            controls.Remove(ctrl);
        }
    }
}