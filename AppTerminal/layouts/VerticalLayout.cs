using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MBC.App.Terminal.Controls;
using MBC.Core;

namespace MBC.App.Terminal.Layouts
{
    public class VerticalLayout : ControlLayout
    {
        private VerticalAlign align;

        public VerticalLayout(VerticalAlign alignment)
        {
            align = alignment;
        }

        public enum VerticalAlign
        {
            Left,
            Center,
            Right
        }

        protected override void DrawAllUnselected()
        {
            base.DrawAllUnselected();
            foreach (UserControl ctrl in controls)
            {
                DrawUnselected(ctrl);
            }
        }

        protected override void DrawSelected(Controls.UserControl ctrl)
        {
            base.DrawSelected(ctrl);
            Utility.StoreConsoleColors();
            Utility.SetConsoleBackgroundColor(Configuration.Global.GetValue<string>("term_color_control_selected_background"));
            Utility.SetConsoleForegroundColor(Configuration.Global.GetValue<string>("term_color_control_selected_foreground"));
            DrawControl(ctrl);
            Utility.RestoreConsoleColors();
        }

        protected override void DrawUnselected(Controls.UserControl ctrl)
        {
            base.DrawUnselected(ctrl);
            Utility.StoreConsoleColors();
            Utility.SetConsoleBackgroundColor(Configuration.Global.GetValue<string>("term_color_control_unselected_background"));
            Utility.SetConsoleForegroundColor(Configuration.Global.GetValue<string>("term_color_control_unselected_foreground"));
            DrawControl(ctrl);
            Utility.RestoreConsoleColors();
        }

        private void DrawControl(UserControl ctrl)
        {
            switch (align)
            {
                case VerticalAlign.Left:
                    module.AlignToLine(module.CurrentY);
                    module.WriteText(ctrl.Text);
                    module.NewLine();
                    break;
                case VerticalAlign.Center:
                    module.WriteCenteredText(ctrl.Text);
                    module.NewLine();
                    break;
                case VerticalAlign.Right:
                    module.AlignToCoord(module.Width - ctrl.Text.Length - 1, module.CurrentY);
                    module.WriteText(ctrl.Text);
                    module.NewLine();
                    break;
            }
        }
    }
}
