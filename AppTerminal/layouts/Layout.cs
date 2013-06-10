using MBC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MBC.App.Terminal.Controls;
using System.Drawing;
using MBC.App.Terminal.Modules;

namespace MBC.App.Terminal.Layouts
{
    
    /// <summary>A ControlLayout is the base for a container of UserControl objects. This class is responsible
    /// for displaying the UserControl objects contained, in a certain way.
    /// </summary>
    public abstract class Layout
    {

        private Component layoutComp;

        public Component LayoutComponent
        {
            get
            {
                return layoutComp;
            }
        }

        public void SetLayoutComponent(Component comp)
        {
            layoutComp = comp;
        }

        public abstract void DoLayout();
    }
}
