using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using MBC.App.Terminal;
using MBC.App.Terminal.Modules;
using MBC.Core;
using MBC.Core.Util;

namespace MBC.App.Terminal
{
    /// <summary>
    /// Provides an interactive console for the user. Supports multiple terminal buffers.
    /// Use the CTRL + Arrow keys to navigate between buffers. Use CTRL + SHIFT + Arrow keys to
    /// move buffers around.
    ///
    /// This static class is responsible for the management of multiple buffers which are TerminalModule objects.
    /// This class is static because there can only be one console per application.
    ///
    /// Use the static methods AddModule(TerminalModule) and RemoveModule(TerminalModule) to add or remove modules
    /// from the terminal display. If multiple TerminalModule objects are added, this class will ensure they
    /// are displayed on the screen simultaneously. After adding/removing modules, update the state of the
    /// display by using the UpdateDisplay() method.
    ///
    /// Note, the width and height of the terminal window should be a minimum of at least 80 and 25, respectively.
    /// </summary>
    [Configuration("term_max_columns", 3)]
    [Configuration("term_color_selected", ConsoleColor.Green)]
    [Configuration("term_color_grid", ConsoleColor.Yellow)]
    [Configuration("term_color_input_display", ConsoleColor.DarkGreen)]
    [Configuration("term_color_input_text", ConsoleColor.White)]
    [Configuration("term_color_background", ConsoleColor.Black)]
    public static class BattleshipConsole
    {
        private static string inputLine = "";

        private static bool isRunning = true;

        //A string of input given by the user.
        private static long lastTickBeep = 0;

        private static int modRows = 1, modCols = 1;

        private static List<TerminalModule> runningMods;

        //The modules attached to this BattleshipConsole.
        private static int selectedMod = 0;

        //The selected module from the list of runningMods.
        //The number of rows and columns.
        private static int width = 0, height = 0;

        /// <summary>
        /// Gets or sets the running state of this application. Setting this false will cause
        /// the application to quit.
        /// </summary>
        public static bool Running
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        /// <summary>
        /// Adds a TerminalModule object to this BattleshipConsole for displaying.
        /// </summary>
        /// <param name="mod"></param>
        public static void AddModule(TerminalModule mod)
        {
            runningMods.Add(mod);
        }

        /// <summary>
        /// Removes a TerminalModule object from this BattleshipConsole.
        /// </summary>
        /// <param name="mod"></param>
        public static void RemoveModule(TerminalModule mod)
        {
            runningMods.Remove(mod);
        }

        /// <summary>
        /// Refreshes the entire terminal buffer. Use after adding/removing modules.
        /// </summary>
        public static void UpdateDisplay()
        {
            Console.Clear();
            int maxCols = Configuration.Global.GetValue<int>("term_max_columns");
            modRows = (runningMods.Count / (maxCols + 1)) + 1;
            modCols = runningMods.Count > maxCols ? maxCols : runningMods.Count;
            if (modCols == 0)
                modCols = 1;
            RecalculateBounds();
            WriteGridLines();
            UpdateInputLineDisplay(true);
        }

        /// <summary>
        /// Used for selecting a buffer via arrow keys + CTRL. Also moves
        /// the order of TerminalModules in this BattleshipConsole with arrow keys + CTRL + SHIFT.
        /// Does not do anything and returns false if CTRL is not pressed.
        /// </summary>
        /// <param name="cki"></param>
        /// <returns></returns>
        private static bool HandleKeyControl(ConsoleKeyInfo cki)
        {
            if (cki.Modifiers.HasFlag(ConsoleModifiers.Control))
            {
                int lastSel = selectedMod;
                bool arrow = true;
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedMod -= modCols;
                        if (selectedMod < 0)
                            selectedMod = 0;
                        break;

                    case ConsoleKey.DownArrow:
                        selectedMod += modCols;
                        if (selectedMod > runningMods.Count - 1)
                            selectedMod = runningMods.Count - 1;
                        break;

                    case ConsoleKey.LeftArrow:
                        selectedMod--;
                        if (selectedMod < 0)
                            selectedMod = 0;
                        break;

                    case ConsoleKey.RightArrow:
                        selectedMod++;
                        if (selectedMod > runningMods.Count - 1)
                            selectedMod = runningMods.Count - 1;
                        break;

                    default:
                        arrow = false;
                        break;
                }
                if (arrow)
                {
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        var temp = runningMods[lastSel];
                        runningMods[lastSel] = runningMods[selectedMod];
                        runningMods[selectedMod] = temp;
                    }
                    RecalculateBounds();
                    WriteGridLines();
                    return true;
                }
            }
            return false;
        }

        private static void Main(string[] args)
        {
            Configuration.Initialize(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MBC Data");
            Console.Title = "Mohawk Battleship Competition";
            Console.Clear();

            runningMods = new List<TerminalModule>();
            AddModule(new MainMenu());
            UpdateDisplay();

            while (isRunning)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (!HandleKeyControl(key) && !runningMods[selectedMod].KeyPressed(key))
                {
                    if (ProcessInputLine(key))
                    {
                        runningMods[selectedMod].InputEntered(inputLine);
                    }
                    UpdateInputLineDisplay(false);
                }
                else
                {
                    Console.SetCursorPosition(10 + inputLine.Length, height - 2);
                }

                if (width != Console.WindowWidth || height != Console.WindowHeight)
                {
                    Console.Clear();
                    UpdateDisplay();
                }
            }
        }

        /// <summary>
        /// Processes a key press and modifies the input string with valid characters.
        /// </summary>
        /// <returns>
        /// True if the enter key was pressed and processed, False otherwise.
        /// </returns>
        private static bool ProcessInputLine(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    if (inputLine.Length != 0)
                    {
                        string input = inputLine;
                        inputLine = "";
                        switch (input.ToLower())
                        {
                            case "exit":
                                Running = false;
                                return false;
                        }
                        return true;
                    }
                    break;

                case ConsoleKey.Backspace:
                    if (inputLine.Length > 0)
                    {
                        inputLine = inputLine.Substring(0, inputLine.Length - 1);
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
                        inputLine += key.KeyChar;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Calculates the boundaries of the generated number of rows and columns used
        /// for displaying the TerminalModules based on the size of the terminal window.
        /// </summary>
        private static void RecalculateBounds()
        {
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            int bufferWidth = width / modCols;
            int bufferHeight = (height - 2) / modRows;
            for (int i = 0; i < runningMods.Count(); i++)
            {
                int row = i / Configuration.Global.GetValue<int>("term_max_columns");
                runningMods[i].RefreshDisplay((i % (modCols)) * bufferWidth + 1,
                    row * bufferHeight + 1, bufferWidth - 1, bufferHeight - 2);
            }
        }

        /// <summary>
        /// Updates the display of the input line on the terminal window. Optionally updates
        /// just the input string that the user has entered instead of the entire line.
        /// </summary>
        private static void UpdateInputLineDisplay(bool complete)
        {
            var storedColors = ColorStore.StoreCurrentColors();
            if (complete)
            {
                Console.ForegroundColor = Configuration.Global.GetValue<ConsoleColor>("term_color_input_display");
                Console.SetCursorPosition(0, height - 2);
                Console.Write("[Input]:> ");
            }
            else
            {
                Console.SetCursorPosition(10, height - 2);
            }
            Console.ForegroundColor = Configuration.Global.GetValue<ConsoleColor>("term_color_input_text");
            Console.Write(new String(' ', width - 10));
            Console.SetCursorPosition(10, height - 2);
            Console.Write(inputLine);
            storedColors.Restore();
        }

        /// <summary>
        /// Makes the grid lines that identify each module displayed on the terminal.
        /// </summary>
        private static void WriteGridLines()
        {
            var storedColors = ColorStore.StoreCurrentColors();
            Console.BackgroundColor = Configuration.Global.GetValue<ConsoleColor>("term_color_grid");
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
            Console.BackgroundColor = Configuration.Global.GetValue<ConsoleColor>("term_color_selected");
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
            storedColors.Restore();
        }
    }
}