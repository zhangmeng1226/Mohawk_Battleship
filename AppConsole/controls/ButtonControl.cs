using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{
    public class ButtonControl : UserControl
    {
        private string buttonText;

        public ButtonControl(string text, ButtonSelect selectEvent)
        {
            ButtonSelectEvent += selectEvent;
            ButtonText = text;
        }

        public delegate bool ButtonSelect(string btnText);

        public event ButtonSelect ButtonSelectEvent;

        public string ButtonText
        {
            get
            {
                return buttonText;
            }
            set
            {
                buttonText = value;
                text = string.Format("[{0}]", buttonText);
                RequiresUpdate = true;
            }
        }

        public override void Input(string txt)
        {
        }

        public override bool KeyPress(ConsoleKeyInfo key)
        {
            if (key.Key != ConsoleKey.Enter)
            {
                return false;
            }
            if (ButtonSelectEvent != null)
            {
                return ButtonSelectEvent(Text.Substring(1, Text.Length - 2));
            }
            return false;
        }
    }
}