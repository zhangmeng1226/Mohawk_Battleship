using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MBC.App.Terminal.Controls;
using System.Drawing;

namespace MBC.App.Terminal.Layouts
{
    /**
     * <summary>A ControlLayout is the base for a container of UserControl objects. This class is responsible
     * for displaying the UserControl objects contained, in a certain way.
     * </summary>
     */
    public abstract class ControlLayout
    {
        protected List<UserControl> controls = new List<UserControl>();
        private int currentControl = 0;
        protected TerminalModule module;
        protected int displayLine = -1;

        public static void SetConfigDefaults()
        {
            Configuration.Default.SetValue<string>("term_color_control_selected_background", "DarkGreen");
            Configuration.Default.SetValue<string>("term_color_control_selected_foreground", "Black");
            Configuration.Default.SetValue<string>("term_color_control_unselected_background", "Black");
            Configuration.Default.SetValue<string>("term_color_control_unselected_foreground", "White");
        }

        /**
         * <summary>Called when setting the TerminalModule this layout is contained in.</summary>
         */
        public void SetModule(TerminalModule mod)
        {
            module = mod;
        }

        /**
         * <summary>Sets the line (or offset from the top of the TerminalModule), to begin displaying
         * the controls.</summary>
         */
        public void SetDisplayLine(int ln)
        {
            displayLine = ln;
        }

        /**
         * <summary>Adds a UserControl object that will be managed by this ControlLayout.</summary>
         */
        public void Add(UserControl ctrl)
        {
            controls.Add(ctrl);
        }

        /**
         * <summary>Invoked when a key has been pressed, the TerminalModule is selected in the
         * BattleshipConsole, and this ControlLayout is in focus within the TerminalModule.
         * 
         * Through this method, this ControlLayout will iterate through all of the UserControl objects contained
         * with the up and down arrow keys. If these keys have not been pressed, the selected UserControl
         * KeyPress(ConsoleKeyInfo) method will be called.
         * 
         * Changing this default behaviour may be desireable in some cases such as the NoLayout class.
         * </summary>
         */
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

        /**
         * <summary>Usually invoked by the TerminalModule, this will display the selected UserControl
         * within this ControlLayout as being selected.</summary.
         */
        public virtual void Select()
        {
            DrawSelected(controls[currentControl]);
        }

        /**
         * <summary>Invoked when an input string has been entered into the BattleshipConsole, while this
         * ControlLayout is in focus. By default, this will invoke the selected UserControl object's
         * Input(string) method.</summary>
         */
        public virtual void Input(string input)
        {
            controls[currentControl].Input(input);
        }

        /**
         * <summary>Called when this ControlLayout is to completely draw the contents into the terminal
         * window. If the line this ControlLayout is to be displayed at is not specified, then the current
         * line being drawn in the TerminalModule will be used for future references.</summary>
         */
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

        /**
         * <summary>Draws the provided UserControl as being selected in position.
         * 
         * Call this base method to make the alignment.
         * </summary>
         */
        protected virtual void DrawSelected(UserControl ctrl)
        {
            module.AlignToLine(displayLine + controls.IndexOf(ctrl));
        }

        /**
         * <summary>Draws the provided UserControl as being unselected in position.
         * 
         * Call this base method to make the alignment.
         * </summary>
         */
        protected virtual void DrawUnselected(UserControl ctrl)
        {
            module.AlignToLine(displayLine + controls.IndexOf(ctrl));
        }

        /**
         * <summary>Draws all UserControls as unselected.
         * 
         * Call this base method to make the alignment.
         * </summary>
         */
        protected virtual void DrawAllUnselected()
        {
            module.AlignToLine(displayLine);
        }
    }
}
