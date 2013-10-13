using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Modules;
using MBC.Core;
using MBC.Core.Util;

namespace MBC.App.Terminal.Layouts
{
    /// <summary>
    /// A ControlLayout is the base for a container of UserControl objects. This class is responsible
    /// for displaying the UserControl objects contained, in a certain way.
    /// </summary>
    [Configuration("term_color_control_selected_background", ConsoleColor.DarkGreen)]
    [Configuration("term_color_control_selected_foreground", ConsoleColor.Black)]
    [Configuration("term_color_control_unselected_background", ConsoleColor.Black)]
    [Configuration("term_color_control_unselected_foreground", ConsoleColor.White)]
    public abstract class ControlLayout
    {
        protected List<UserControl> controls = new List<UserControl>();
        protected int displayLine = -1;
        protected TerminalModule module;
        private int currentControl = 0;

        public void Add(UserControl ctrl)
        {
            controls.Add(ctrl);
        }

        public void Display()
        {
            if (displayLine == -1)
            {
                displayLine = module.CurrentY;
            }
            DrawAllUnselected();
            if (module.IsLayoutSelected(this))
            {
                DrawSelected(controls[currentControl]);
            }
        }

        public virtual void Input(string input)
        {
            controls[currentControl].Input(input);
        }

        public virtual bool KeyPressed(ConsoleKeyInfo key)
        {
            if (controls.Count == 0)
            {
                return false;
            }
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DrawUnselected(controls[currentControl]);
                    currentControl--;
                    if (currentControl < 0)
                    {
                        currentControl = controls.Count - 1;
                        module.PreviousLayout();
                        break;
                    }
                    DrawSelected(controls[currentControl]);
                    break;

                case ConsoleKey.DownArrow:
                    DrawUnselected(controls[currentControl]);
                    currentControl++;
                    if (currentControl >= controls.Count)
                    {
                        currentControl = 0;
                        module.NextLayout();
                        break;
                    }
                    DrawSelected(controls[currentControl]);
                    break;

                default:
                    bool result = controls[currentControl].KeyPress(key);
                    if (controls[currentControl].RequiresUpdate)
                    {
                        DrawSelected(controls[currentControl]);
                    }
                    return result;
            }
            return true;
        }

        public virtual void Select()
        {
            DrawSelected(controls[currentControl]);
        }

        public void SetDisplayLine(int ln)
        {
            displayLine = ln;
        }

        public void SetModule(TerminalModule mod)
        {
            module = mod;
        }

        protected virtual void DrawAllUnselected()
        {
            module.AlignToLine(displayLine);
        }

        protected virtual void DrawSelected(UserControl ctrl)
        {
            module.AlignToLine(displayLine + controls.IndexOf(ctrl));
        }

        protected virtual void DrawUnselected(UserControl ctrl)
        {
            module.AlignToLine(displayLine + controls.IndexOf(ctrl));
        }
    }
}