using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{

    /// <summary>The UserControl class is used as a base for a user control located on the terminal window.
    /// Each UserControl is linked with a ControlLayout and displays are made with the layout this UserControl
    /// is linked to. Generally, all UserControl objects have text, whether they are buttons, checkboxes, progress bars,
    /// string input, etc.
    /// 
    /// When extending this class, note that the enter key is used in the BattleshipConsole for whole string
    /// inputs. Do not handle an enter key press (return false in KeyPress(ConsoleKeyInfo) if the input string 
    /// is required for this control.
    /// </summary>

    public abstract class UserControl : Component
    {
    }
}
