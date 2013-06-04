using MBC.App.Terminal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Layouts
{
    /**
     * <summary>The NoLayout class is created to provide an empty layout for TerminalModules that
     * do not contain UserControls.</summary>
     */
    public class NoLayout : ControlLayout
    {

        protected override void DrawSelected(UserControl ctrl)
        {
            //Do not draw anything.
        }

        protected override void DrawUnselected(UserControl ctrl)
        {
            //Do not draw anything.
        }

        protected override void DrawAllUnselected()
        {
            //Do not draw anything.
        }

        public override void Input(string input)
        {
            //Do not do anything.
        }

        public override bool KeyPressed(ConsoleKeyInfo key)
        {
            //Do not do anything.
            return false;
        }
    }
}
