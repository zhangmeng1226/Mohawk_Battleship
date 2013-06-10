using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{
    
    /// <summary>A ButtonControl is a UserControl that fires an event when the enter key has been pressed
    /// while this ButtonControl has been selected.</summary>
     
    public class ButtonControl : UserControl
    {
        /*
        public delegate bool ButtonSelect(string btnText);
        public event ButtonSelect ButtonSelectEvent;

        
        /// <summary>Constructs a new ButtonControl object with the specified event to add to this control.</summary>
         
        public ButtonControl(string text, ButtonSelect selectEvent)
        {
            ButtonSelectEvent += selectEvent;
            Text = "[" + text + "]";
        }

        
        /// <summary>This UserControl does not deal with string input.</summary>
         
        public override void Input(string txt)
        {

        }

        
        /// <summary>Fires events that are associated with this ButtonControl when the enter key has been pressed.
        /// Returns false if the enter key was not pressed.</summary>
         
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
        }*/
    }
}
