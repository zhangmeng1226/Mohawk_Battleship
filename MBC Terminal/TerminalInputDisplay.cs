using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using MBC.Core;

namespace MBC.Terminal
{
    public class TerminalInputDisplay : TerminalModule
    {
        static TerminalInputDisplay()
        {
            Configuration.Default.SetValue<string>("term_color_input_display", "DarkGreen");
            Configuration.Default.SetValue<string>("term_color_input_text", "White");
        }
        private string inputStr = "";
        private long lastTickBeep = 0;

        public TerminalInputDisplay()
        {
            KeyPressEvent += new KeyPress(TerminalInputDisplay_KeyPressEvent);
        }

        void TerminalInputDisplay_KeyPressEvent(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    inputStr = "";
                    AlignToCoord(9, 0);
                    WriteCharRepeat(' ', Width - 10);
                    AlignToCoord(10, 0);
                    break;
                case ConsoleKey.Backspace:
                    if (inputStr.Length > 0)
                    {
                        AlignToCoord(10 + inputStr.Length - 1, 0);
                        WriteText(" ");
                        AlignToCoord(10 + inputStr.Length - 1, 0);
                        inputStr = inputStr.Substring(0, inputStr.Length - 1);
                    }
                    else if (Environment.TickCount - lastTickBeep > 500)
                    {
                        lastTickBeep = Environment.TickCount;
                        Console.Beep();
                    }
                    break;
                default:
                    if (!Char.IsControl(key.KeyChar))
                    {
                        Align();
                        Console.ForegroundColor = Util.EnumKey<ConsoleColor>("term_color_input_text");
                        WriteText("" + key.KeyChar);
                        inputStr += key.KeyChar;
                    }
                    break;
            }
        }

        protected override void Display()
        {
            Console.ForegroundColor = Util.EnumKey<ConsoleColor>("term_color_input_display");
            WriteText("[Input]:> ");

            Console.ForegroundColor = Util.EnumKey<ConsoleColor>("term_color_input_text");
            WriteText(inputStr);
        }
    }
}
