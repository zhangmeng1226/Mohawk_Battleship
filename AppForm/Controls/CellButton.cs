using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MBC.Shared;

namespace MBC.App.FormBattleship.Controls
{

    class CellButton : Button
    {
        public CellButton()
        {
            Text = "Open";
            Width = 60;
            Height = 60;
            Padding = new Padding(0);
            Margin = new Padding(0);
            BackColor = System.Drawing.Color.Black;
            ForeColor = System.Drawing.Color.White;
        }

    }
}
