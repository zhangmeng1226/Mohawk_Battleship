using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.BattleshipConsole.NewStuff
{
    public class Label : Element
    {
        public Label(string text)
        {
            Text = text;
        }

        public Label()
            : this("")
        {
        }

        public string Text
        {
            get;
            set;
        }
    }
}