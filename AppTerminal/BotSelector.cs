using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using MBC.Core.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal
{
    public class BotSelector : TerminalModule
    {
        VerticalLayout redList;
        VerticalLayout blueList;
        VerticalLayout buttonConfirmLayout;

        public BotSelector()
        {
            ControllerFactory.LoadControllerFolder();
            redList = new VerticalLayout(VerticalLayout.VerticalAlign.Left);
            redList.SetDisplayLine(5);
            RadioButtonControlGroup redGroup = new RadioButtonControlGroup(redList);
            foreach (string ctrlName in ControllerFactory.Names)
            {
                redGroup.AddRadioButton(ctrlName);
            }


            blueList = new VerticalLayout(VerticalLayout.VerticalAlign.Right);
            blueList.SetDisplayLine(5);
            RadioButtonControlGroup blueGroup = new RadioButtonControlGroup(blueList);
            foreach (string ctrlName in ControllerFactory.Names)
            {
                blueGroup.AddRadioButton(ctrlName);
            }

            buttonConfirmLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            buttonConfirmLayout.SetDisplayLine(6 + ControllerFactory.Names.Count);
            buttonConfirmLayout.Add(new ButtonControl("Confirm", SelectionConfirm));

            AddControlLayout(redList);
            AddControlLayout(blueList);
            AddControlLayout(buttonConfirmLayout);
        }

        private void SelectionConfirm(string btn)
        {

        }

        protected override void Display()
        {
            WriteCenteredText("=====BOT SELECTION=====");
            NewLine(2);
            WriteCenteredText("Select two controllers from the list below.");
            NewLine();
            Util.SetConsoleForegroundColor("Red");
            WriteText("Red Controller:");
            AlignToCoord(Width - "Blue Controller:".Length, CurrentY);
            Util.SetConsoleForegroundColor("Blue");
            WriteText("Blue Controller:");
            redList.Display();
            blueList.Display();
            buttonConfirmLayout.Display();
        }
    }
}
