using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MBC.Shared;

namespace MBC.App.FormBattleship.Controls
{

    class Cell : Button
    {
        public Coordinates Coordinate{ get; set; }
    }
}
