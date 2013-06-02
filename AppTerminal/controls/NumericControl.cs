using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{
    public class NumericControl : UserControl
    {
        private int value;
        private NumericControlParameters parameters;

        public NumericControl(NumericControlParameters param)
        {
            value = param.initVal;
            parameters = param;
            text = param.initVal.ToString();
            SetDisplayText();
        }

        public int Value
        {
            get { return value; }
        }

        private void SetDisplayText()
        {
            text = "[" + value + "] " + parameters.text;
        }

        public override void Input(string txt)
        {
            int val;
            if (int.TryParse(txt, out val))
            {
                value = val;
                SetDisplayText();
                RequiresUpdate = true;
            }
        }

        public override bool KeyPress(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    if (value + parameters.increment > parameters.maximum)
                        break;
                    value += parameters.increment;
                    SetDisplayText();
                    RequiresUpdate = true;
                    break;
                case ConsoleKey.LeftArrow:
                    if (value - parameters.increment < parameters.minimum)
                        break;
                    if (!parameters.negative && value - parameters.increment < 0)
                        break;
                    value -= parameters.increment;
                    SetDisplayText();
                    RequiresUpdate = true;
                    break;
                default:
                    return false;
            }
            return false;
        }

        public class NumericControlParameters
        {
            public bool negative;
            public int minimum;
            public int maximum;
            public int increment;
            public int initVal;
            public string text;

            public NumericControlParameters(string txt, bool neg, int min, int max, int inc, int init)
            {
                negative = neg;
                minimum = min;
                maximum = max;
                increment = inc;
                initVal = init;
                text = txt;
            }
        }
    }
}
