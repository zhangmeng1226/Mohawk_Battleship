﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MBC.Shared;
namespace MBC.App.FormBattleship.Controls
{
    class ShipButton : Button
    {

        public ShipButton(int size)
        {
            ShipOrientation = ShipOrientation.Horizontal;
            ShipSize = size;
        }

        public ShipOrientation ShipOrientation { get; set; }
        public int ShipSize { get; set; }

        /// <summary>
        /// Toggles the ships orientation.
        /// </summary>
        /// <returns>Its new current Orientation</returns>
        public ShipOrientation toggleOrientation()
        {
            if (ShipOrientation == ShipOrientation.Horizontal)
                ShipOrientation = ShipOrientation.Vertical;
            else
                ShipOrientation = ShipOrientation.Horizontal;
            return ShipOrientation;
        }
    }
}
