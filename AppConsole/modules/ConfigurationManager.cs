using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.App.Terminal.Controls;
using MBC.App.Terminal.Layouts;
using MBC.App.Terminal.Modules;
using MBC.Core.Util;

namespace MBC.App.Terminal.Modules
{
    public class ConfigurationManager : TerminalModule
    {
        private ButtonControl applyButton;
        private string error;
        private VerticalLayout leftLayout, rightLayout, centerLayout;
        private List<StringInputControl> stringControls;

        public ConfigurationManager()
        {
            stringControls = new List<StringInputControl>();
            leftLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Left);
            rightLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Right);
            centerLayout = new VerticalLayout(VerticalLayout.VerticalAlign.Center);
            error = null;

            var configKeys = Configuration.GetAllKnownKeys();
            for (int i = 0; i < configKeys.Count / 2; i++)
            {
                var stringControl = CreateConfigInput(configKeys[i]);
                leftLayout.Add(stringControl);
                stringControls.Add(stringControl);
            }
            for (int i = configKeys.Count / 2; i < configKeys.Count; i++)
            {
                var stringControl = CreateConfigInput(configKeys[i]);
                rightLayout.Add(stringControl);
                stringControls.Add(stringControl);
            }

            applyButton = new ButtonControl("Apply changes", ApplyChanges);
            centerLayout.Add(applyButton);
            centerLayout.Add(new ButtonControl("Save changes to file", SaveChanges));
            centerLayout.Add(new ButtonControl("Exit", Exit));
            AddControlLayout(leftLayout);
            AddControlLayout(rightLayout);
            AddControlLayout(centerLayout);
        }

        protected override void Display()
        {
            Console.ForegroundColor = ConsoleColor.White;
            WriteCenteredText("=====CONFIGURATION MANAGER=====");
            NewLine(2);

            if (error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                WriteCenteredText("There was an error while setting the value for " + error);
            }
            else
            {
                WriteCenteredText("Review and modify configuration associations to determine application behaviour.");
            }
            Console.ForegroundColor = ConsoleColor.White;
            NewLine();
            leftLayout.Display();
            rightLayout.Display();
            centerLayout.Display();
        }

        private bool ApplyChanges(string input)
        {
            foreach (var control in stringControls)
            {
                try
                {
                    Configuration.Global.SetValue(control.PlaceholderText, control.InputText);
                }
                catch
                {
                    error = control.PlaceholderText;
                    BattleshipConsole.UpdateDisplay();
                    return false;
                }
            }
            return true;
        }

        private StringInputControl CreateConfigInput(string key)
        {
            StringInputControl control;
            if (key.Length > 8)
            {
                control = new StringInputControl(key.Substring(0, 8) + "...");
            }
            else
            {
                control = new StringInputControl(key);
            }
            control.Input(Configuration.Global.GetValue<object>(key).ToString());
            return control;
        }

        private bool Exit(string input)
        {
            BattleshipConsole.RemoveModule(this);
            BattleshipConsole.AddModule(new MainMenu());
            BattleshipConsole.UpdateDisplay();
            return true;
        }

        private bool SaveChanges(string input)
        {
            if (!ApplyChanges(input))
            {
                return false;
            }
            Configuration.Global.SaveConfigFile();
            return true;
        }
    }
}