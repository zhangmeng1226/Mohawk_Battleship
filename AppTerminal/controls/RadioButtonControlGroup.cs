using MBC.App.Terminal.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Controls
{
    public class RadioButtonControlGroup
    {
        private ControlLayout radioLayout;
        private List<RadioButtonControl> radioControls;
        RadioButtonControl selectedControl;

        public RadioButtonControlGroup(ControlLayout layout)
        {
            radioLayout = layout;
            radioControls = new List<RadioButtonControl>();
        }

        public void AddRadioButton(string text)
        {
            RadioButtonControl ctrl = new RadioButtonControl(text, this);
            radioControls.Add(ctrl);
            radioLayout.Add(ctrl);
            if (selectedControl == null)
            {
                selectedControl = ctrl;
                selectedControl.SetSelected(true);
            }
        }

        public void RadioButtonSelected(RadioButtonControl ctrl)
        {
            selectedControl.SetSelected(false);
            ctrl.SetSelected(true);
            selectedControl = ctrl;
        }

    }
}
