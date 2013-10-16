using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Core;

namespace MBC.App.Terminal.Controls
{
    public class StringInputControl : UserControl
    {
        private string inputText;
        private string placeholderText;

        public StringInputControl(string placeholder)
        {
            placeholderText = placeholder;
            inputText = "";
        }

        public string InputText
        {
            get
            {
                return inputText;
            }
        }

        public string PlaceholderText
        {
            get
            {
                return placeholderText;
            }
        }

        public override void Input(string txt)
        {
            inputText = txt;
            Update();
        }

        public override bool KeyPress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.Backspace:
                    if (inputText.Length > 0)
                    {
                        inputText = inputText.Substring(0, inputText.Length - 1);
                        Update();
                        return true;
                    }
                    break;

                default:
                    if (!Char.IsControl(key.KeyChar))
                    {
                        inputText += key.KeyChar;
                        Update();
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void Update()
        {
            text = string.Format("{0} [{1}]", placeholderText, inputText);
            RequiresUpdate = true;
        }
    }
}