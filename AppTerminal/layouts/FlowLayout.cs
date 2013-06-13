using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MBC.App.Terminal.Controls;
using MBC.Core;

namespace MBC.App.Terminal.Layouts
{
    /// <summary>
    /// A layout that organizes components horizontally, with the specified alignment.
    /// </summary>
    public class FlowLayout : Layout
    {
        private Alignment align;
        private int paddingX;
        private int paddingY;

        public FlowLayout(Alignment alignment)
        {
            align = alignment;
            paddingX = 1;
        }

        public int PaddingHorizontal
        {
            get
            {
                return paddingX;
            }
            set
            {
                paddingX = value;
            }
        }

        public int PaddingVertical
        {

            get
            {
                return paddingY;
            }
            set
            {
                paddingY = value;
            }
        }

        public enum Alignment
        {
            Left,
            Right,
            Centered,
            Spread
        }

        public override void DoLayout()
        {
            switch (align)
            {
                case Alignment.Left:
                    AlignLeft();
                    break;
                case Alignment.Right:
                    AlignLeft();
                    foreach (var comp in LayoutComponent.Children)
                    {
                        comp.Location.X = LayoutComponent.Size.Width - comp.Size.Width - comp.Location.X;
                    }
                    break;
                case Alignment.Centered:
                    AlignLeft();
                    break;
                case Alignment.Spread:
                    break;
            }
        }

        private void AlignLeft()
        {
            Size2D tSize = new Size2D(0, 0);
            Coordinates2D coordPlacement = new Coordinates2D(0, 0);
            int curHeight = 0;
            int afterPlacementX;
            foreach (var comp in LayoutComponent.Children)
            {
                afterPlacementX = comp.Size.Width + coordPlacement.X;
                if (afterPlacementX > LayoutComponent.Size.Width)
                {
                    tSize.Height += curHeight;
                    curHeight = 0;
                    coordPlacement.X = 0;
                    coordPlacement.Y = tSize.Height + paddingY;
                }
                if (afterPlacementX > tSize.Width)
                {
                    tSize.Width = afterPlacementX;
                }
                if (comp.Size.Height > curHeight)
                {
                    curHeight = comp.Size.Height;
                }

                comp.Location.X = coordPlacement.X;
                comp.Location.Y = coordPlacement.Y;

                coordPlacement.X = afterPlacementX + paddingX;
            }
            tSize.Height += curHeight;
            LayoutComponent.Size = tSize;
        }
    }
}
