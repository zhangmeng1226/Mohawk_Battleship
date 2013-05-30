using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{
    public class StringInputControl : UserControl
    {
        private string placeholderText;
        private string inputText;

        public StringInputControl(string placeholder)
            : base(placeholder)
        {
            placeholderText = placeholder;
            inputText = "";
        }

        public override void Input(string txt)
        {
            inputText = txt;
            text = inputText;
            RequiresUpdate = true;
        }

        public override bool KeyPress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (inputText.Length > 0)
                    {
                        inputText = inputText.Substring(0, inputText.Length - 1);
                        if (inputText.Length == 0)
                        {
                            RequiresUpdate = true;
                            text = placeholderText;
                        }
                    }
                    break;
                default:
                    if (!Char.IsControl(key.KeyChar))
                    {
                        inputText += key.KeyChar;
                        text = inputText;
                        RequiresUpdate = true;
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
