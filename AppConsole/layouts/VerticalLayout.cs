using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MBC.App.Terminal.Controls;
using MBC.Core;
using MBC.Core.Util;

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
            var storedColors = ColorStore.StoreCurrentColors();
            Console.BackgroundColor = Configuration.Global.GetValue<ConsoleColor>("term_color_control_selected_background");
            Console.ForegroundColor = Configuration.Global.GetValue<ConsoleColor>("term_color_control_selected_foreground");
            DrawControl(ctrl);
            storedColors.Restore();
        }

        protected override void DrawUnselected(Controls.UserControl ctrl)
        {
            base.DrawUnselected(ctrl);
            var storedColors = ColorStore.StoreCurrentColors();
            Console.BackgroundColor = Configuration.Global.GetValue<ConsoleColor>("term_color_control_unselected_background");
            Console.ForegroundColor = Configuration.Global.GetValue<ConsoleColor>("term_color_control_unselected_foreground");
            DrawControl(ctrl);
            storedColors.Restore();
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