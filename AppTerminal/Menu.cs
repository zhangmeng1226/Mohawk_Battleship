using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Core;

namespace MBC.App.Terminal
{
    public class Menu
    {
        static Menu()
        {
            Configuration.Default.SetValue<string>("term_menu_select_background_color", "White");
            Configuration.Global.SetValue<string>("term_menu_select_foreground_color", "Black");
        }
        private TerminalModule mod;
        private int selected;
        private int line;
        private List<string> items;
        private bool centered;

        public delegate void MenuSelect(string s);
        public event MenuSelect MenuSelectEvent;

        public Menu(TerminalModule m, int ln, bool c, params string[] i)
        {
            init(m, ln, c, i);
        }
        public Menu(TerminalModule m, int ln, params string[] i)
        {
            init(m, ln, false, i);
        }

        private void init(TerminalModule m, int ln, bool c, params string[] i)
        {
            mod = m;
            selected = 0;
            line = ln;
            items = new List<string>(i);
            centered = c;

            mod.KeyPressEvent += new TerminalModule.KeyPress(mod_KeyPressEvent);
        }

        private void WriteMenuItem(int l, bool col)
        {
            mod.AlignToLine(line + l);
            if (col)
            {
                Util.StoreConsoleColors();
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                    Configuration.Global.GetValue<string>("term_menu_select_foreground_color"));
                Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                    Configuration.Global.GetValue<string>("term_menu_select_background_color"));
            }
            if (centered)
                mod.WriteCenteredText(items[l]);
            else
                mod.WriteText(items[l]);
            if (col)
                Util.RestoreConsoleColors();
        }

        void mod_KeyPressEvent(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    WriteMenuItem(selected, false);
                    selected -= 1;
                    if (selected < 0)
                        selected = 0;
                    WriteMenuItem(selected, true);
                    break;
                case ConsoleKey.DownArrow:
                    WriteMenuItem(selected, false);
                    selected += 1;
                    if (selected > items.Count() - 1)
                        selected = items.Count() - 1;
                    WriteMenuItem(selected, true);
                    break;
                case ConsoleKey.Enter:
                    if (MenuSelectEvent != null)
                        MenuSelectEvent(items[selected]);
                    break;
                default:
                    return;
            }
        }

        public void Display()
        {
            for (int i = 0; i < items.Count; i++)
                WriteMenuItem(i, i == selected);
        }
    }
}
