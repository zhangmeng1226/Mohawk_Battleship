using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{
    public class RadioButtonControl : UserControl
    {
        private bool isSelected;
        private RadioButtonControlGroup radioGroup;

        public RadioButtonControl(string text, RadioButtonControlGroup group)
            : base(text)
        {
            this.text = "( ) " + text;
            isSelected = false;
            radioGroup = group;
        }

        public RadioButtonControl(string text, bool select, RadioButtonControlGroup group)
            : base(text)
        {
            this.text = "(" + (select ? "*" : " ") + ") " + text; 
            isSelected = select;
            radioGroup = group;
        }

        public void SetSelected(bool selected)
        {
            isSelected = selected;
            StringBuilder builder = new StringBuilder(text);
            builder[1] = isSelected ? '*' : ' ';
            text = builder.ToString();
            RequiresUpdate = true;
        }

        public override void Input(string txt) { }

        public override bool KeyPress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    radioGroup.RadioButtonSelected(this);
                    return true;
            }
            return false;
        }
    }
}
