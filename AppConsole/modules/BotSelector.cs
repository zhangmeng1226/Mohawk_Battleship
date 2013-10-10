using System;
using System.Collections.Generic;
using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.Core;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;

namespace MBC.App.Terminal.Modules
{
    public class BotSelector : TerminalModule
    {
        private RadioButtonControlGroup blueGroup;
        private VerticalLayout blueList;
        private VerticalLayout buttonConfirmLayout;
        private RadioButtonControlGroup redGroup;
        private VerticalLayout redList;
        private List<ControllerSkeleton> selectableBots;

        public BotSelector()
        {
            LoadControllers();

            redList = new VerticalLayout(VerticalLayout.VerticalAlign.Left);
            redList.SetDisplayLine(5);
            redGroup = new RadioButtonControlGroup(redList);
            foreach (var ctrl in selectableBots)
            {
                redGroup.AddRadioButton(ctrl.GetAttribute<NameAttribute>().Name);
            }

            blueList = new VerticalLayout(VerticalLayout.VerticalAlign.Right);
            blueList.SetDisplayLine(5);
            blueGroup = new RadioButtonControlGroup(blueList);
            foreach (var ctrl in selectableBots)
            {
                blueGroup.AddRadioButton(ctrl.GetAttribute<NameAttribute>().Name);
            }

            buttonConfirmLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            buttonConfirmLayout.SetDisplayLine(6 + selectableBots.Count);
            buttonConfirmLayout.Add(new ButtonControl("Confirm", SelectionConfirm));

            AddControlLayout(redList);
            AddControlLayout(blueList);
            AddControlLayout(buttonConfirmLayout);
        }

        protected override void Display()
        {
            WriteCenteredText("=====BOT SELECTION=====");
            NewLine(2);
            WriteCenteredText("Select two controllers from the list below.");
            NewLine();
            Console.ForegroundColor = ConsoleColor.Red;
            WriteText("Red Controller:");
            AlignToCoord(Width - "Blue Controller:".Length, CurrentY);
            Console.ForegroundColor = ConsoleColor.Blue;
            WriteText("Blue Controller:");
            redList.Display();
            blueList.Display();
            buttonConfirmLayout.Display();
        }

        private void LoadControllers()
        {
            selectableBots = ControllerSkeleton.LoadControllerFolder(
                Configuration.Global.GetValue<string>("app_data_root") + "controllers");
            selectableBots.AddRange(ControllerSkeleton.LoadControllerFolder(Environment.CurrentDirectory + "\\..\\bots"));
        }

        private bool SelectionConfirm(string btn)
        {
            IController red = null, blue = null;
            foreach (var bot in selectableBots)
            {
                var botName = bot.GetAttribute<NameAttribute>().Name;
                if (red == null && redGroup.GetSelected() == botName)
                {
                    red = bot.CreateInstance();
                }
                if (blue == null && blueGroup.GetSelected() == botName)
                {
                    blue = bot.CreateInstance();
                }
                if (red != null && blue != null)
                {
                    break;
                }
            }
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.AddModule(new CompetitionOptionsDisplay(red, blue));
            BattleshipConsole.UpdateDisplay();
            return true;
        }
    }
}