using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{
    /**
     * <summary>The UserControl class is used as a base for a user control located on the terminal window.
     * Each UserControl is linked with a ControlLayout and displays are made with the layout this UserControl
     * is linked to. Generally, all UserControl objects have text, whether they are buttons, checkboxes, progress bars,
     * string input, etc.
     * 
     * When extending this class, note that the enter key is used in the BattleshipConsole for whole string
     * inputs. Do not handle an enter key press (return false in KeyPress(ConsoleKeyInfo) if the input string 
     * is required for this control.
     * </summary>
     */
    public abstract class UserControl
    {
        protected string text; //The string of text this UseControl will display.
        private bool requiresUpdate;

        /**
         * <summary>Gets the text that this UserControl will display in the terminal.</summary>
         */
        public string Text
        {
            get { return text; }
        }

        public UserControl(string text)
        {
            this.text = text;
        }

        /**
         * <summary>Gets or sets a boolean value that determines whether or not this UserControl requires
         * a redisplay on the terminal window. Getting the value will cause this boolean value to become false.</summary>
         */
        public bool RequiresUpdate
        {
            get
            {
                bool update = requiresUpdate;
                requiresUpdate = false;
                return update;
            }
            set
            {
                requiresUpdate = value;
            }
        }

        /**
         * <summary>Invoked when a string of text from the BattleshipConsole input is entered,
         * and the TerminalModule this UserControl is contained in is selected.</summary>
         */
        public abstract void Input(string txt);

        /**
         * <summary>Invoked when a key has been pressed, and the TerminalModule this UserControl is contained
         * in is selected.</summary>
         */
        public abstract bool KeyPress(ConsoleKeyInfo key);
    }
}
