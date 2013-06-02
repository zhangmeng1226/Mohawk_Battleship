using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using MBC.Core.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal.Modules
{
    public class BotSelector : TerminalModule
    {
        VerticalLayout redList;
        VerticalLayout blueList;
        RadioButtonControlGroup redGroup;
        RadioButtonControlGroup blueGroup;

        VerticalLayout buttonConfirmLayout;

        public BotSelector()
        {
            ControllerFactory.LoadControllerFolder();
            redList = new VerticalLayout(VerticalLayout.VerticalAlign.Left);
            redList.SetDisplayLine(5);
            redGroup = new RadioButtonControlGroup(redList);
            foreach (string ctrlName in ControllerFactory.Names)
            {
                redGroup.AddRadioButton(ctrlName);
            }


            blueList = new VerticalLayout(VerticalLayout.VerticalAlign.Right);
            blueList.SetDisplayLine(5);
            blueGroup = new RadioButtonControlGroup(blueList);
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

        private bool SelectionConfirm(string btn)
        {
            var red = ControllerFactory.CreateController(redGroup.GetSelected());
            var blue = ControllerFactory.CreateController(blueGroup.GetSelected());
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.AddModule(new CompetitionOptionsDisplay(red, blue));
            BattleshipConsole.UpdateDisplay();
            return true;
        }

        protected override void Display()
        {
            WriteCenteredText("=====BOT SELECTION=====");
            NewLine(2);
            WriteCenteredText("Select two controllers from the list below.");
            NewLine();
            Utility.SetConsoleForegroundColor("Red");
            WriteText("Red Controller:");
            AlignToCoord(Width - "Blue Controller:".Length, CurrentY);
            Utility.SetConsoleForegroundColor("Blue");
            WriteText("Blue Controller:");
            redList.Display();
            blueList.Display();
            buttonConfirmLayout.Display();
        }
    }
}
