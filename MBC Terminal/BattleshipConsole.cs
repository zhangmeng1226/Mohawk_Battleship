using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Drawing;
using MBC.Core;
using MBC.Terminal;

namespace MBC.Terminal
{
    /**
     * <summary>Provides an interactive console for the user. Supports multiple terminal buffers.
     * Use the CTRL + Arrow keys to navigate between buffers. Use CTRL + SHIFT + Arrow keys to
     * move buffers around.</summary>
     */
    public class BattleshipConsole
    {
        static BattleshipConsole()
        {
            Configuration.Default.SetValue<int>("term_max_columns", 3);
            Configuration.Default.SetValue<string>("term_color_selected", "Green");
            Configuration.Default.SetValue<string>("term_color_grid", "Yellow");
        }
        private static List<TerminalModule> runningMods; //The first element is the TerminalInputDisplay module.
        private static int selectedMod = 1;
        private static int modRows = 1, modCols = 1;
        private static int width = 0, height = 0;
        private static bool isRunning = true;

        public static bool Running
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        private static void WriteGridLines()
        {
            Util.StoreConsoleColors();
            Console.BackgroundColor = Util.EnumKey<ConsoleColor>("term_color_grid");
            char[] c;
            for (int i = 0; i <= modRows; i++)
            {
                Console.SetCursorPosition(0, i == modRows ? height - 3 : (height / modRows) * i - i);
                c = new char[width];
                for (int j = 0; j < c.Length; j++)
                    c[j] = ' ';
                Console.Write(c);
            }
            for (int i = 0; i <= modCols; i++)
            {
                int left = i == modCols ? width - 1 : i * (width / modCols);
                for (int j = 0; j < height - 3; j++)
                {
                    Console.SetCursorPosition(left, j);
                    Console.Write(' ');
                }
            }


            //Writing the selected border
            Console.BackgroundColor = Util.EnumKey<ConsoleColor>("term_color_selected");
            c = new char[width / modCols];
            for (int i = 0; i < c.Length; i++)
                c[i] = ' ';
            int modLeft = runningMods[selectedMod].WriteX - 1;
            int modTop = runningMods[selectedMod].WriteY - 1;
            int modHeight = runningMods[selectedMod].Height + 1;
            int modWidth = runningMods[selectedMod].Width + 1;

            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(modLeft, modTop + (modHeight * i));
                Console.Write(c);
            }
            for (int i = 0; i < 2; i++)
            {
                int x = modLeft + modWidth * i;
                x = x >= width ? width - 1 : x;
                for (int j = 0; j < modHeight; j++)
                {
                    Console.SetCursorPosition(x, modTop + j);
                    Console.Write(' ');
                }
            }
            Util.RestoreConsoleColors();
        }

        private static void SetGrid()
        {
            int maxCols = Configuration.Global.GetValue<int>("term_max_columns");
            modRows = ((runningMods.Count - 1) / (maxCols + 1)) + 1;
            modCols = runningMods.Count - 1 > maxCols ? maxCols : runningMods.Count - 1;
            if (modCols == 0)
                modCols = 1;
            RecalculateBounds();
            WriteGridLines();
        }

        public static void AddModule(TerminalModule mod)
        {
            runningMods.Add(mod);
            SetGrid();
        }

        public static void RemoveModule(TerminalModule mod)
        {
            runningMods.Remove(mod);
            SetGrid();
        }

        private static void RecalculateBounds()
        {
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            runningMods[0].RefreshDisplay(0, height - 1, width, 2);
            int bufferWidth = width / modCols;
            int bufferHeight = (height - 2) / modRows;
            for (int i = 1; i < runningMods.Count(); i++)
            {
                int row = (i - 1) / Configuration.Global.GetValue<int>("term_max_columns");
                runningMods[i].RefreshDisplay(((i - 1) % (modCols)) * bufferWidth + 1,
                    row * bufferHeight + 1, bufferWidth - 1, bufferHeight - 2);
            }
        }

        private static bool HandleKey(ConsoleKeyInfo cki)
        {
            if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                int lastSel = selectedMod;
                bool arrow = true;
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedMod -= modCols;
                        if (selectedMod < 1)
                            selectedMod = 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedMod += modCols;
                        if (selectedMod > runningMods.Count-1)
                            selectedMod = runningMods.Count-1;
                        break;
                    case ConsoleKey.LeftArrow:
                        selectedMod--;
                        if (selectedMod < 1)
                            selectedMod = 1;
                        break;
                    case ConsoleKey.RightArrow:
                        selectedMod++;
                        if (selectedMod > runningMods.Count-1)
                            selectedMod = runningMods.Count-1;
                        break;
                    default:
                        arrow = false;
                        break;
                }
                if (arrow)
                {
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                        Util.ListSwap<TerminalModule>(runningMods, lastSel, selectedMod);
                    RecalculateBounds();
                    WriteGridLines();
                    return true;
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            Console.Title = "Mohawk Battleship Competition";
            Console.Clear();

            runningMods = new List<TerminalModule>();
            runningMods.Add(new TerminalInputDisplay());
            runningMods.Add(new MainMenu());
            AddModule(new MainMenu());

            string line = "";
            while (isRunning)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (!HandleKey(key))
                    runningMods[selectedMod].KeyPressed(key);
                runningMods[0].KeyPressed(key);

                if (key.Key == ConsoleKey.Enter)
                {
                    foreach (TerminalModule mod in runningMods)
                        mod.InputEntered(line);
                    line = "";
                }
                else
                    line += key.KeyChar;

                if (width != Console.WindowWidth || height != Console.WindowHeight)
                {
                    Console.Clear();
                    SetGrid();
                }
            }
        }
    }
}
