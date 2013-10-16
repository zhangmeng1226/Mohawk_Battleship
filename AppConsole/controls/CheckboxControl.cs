using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{
    public class CheckboxControl : UserControl
    {
        private bool isChecked = false;

        public CheckboxControl(string text)
        {
            this.text = "[ ] " + text;
        }

        public CheckboxControl(string text, bool chk)
        {
            this.text = "[" + (chk ? "*" : " ") + "] " + text;
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
        }

        public override void Input(string txt)
        {
        }

        public override bool KeyPress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    isChecked = !isChecked;
                    SetCheckbox();
                    RequiresUpdate = true;
                    return true;
            }
            return false;
        }

        private void SetCheckbox()
        {
            StringBuilder builder = new StringBuilder(Text);
            builder[1] = isChecked ? '*' : ' ';
            text = builder.ToString();
        }
    }
}