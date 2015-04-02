using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBC.Shared;

namespace MBC.App.FormBattleship.Controls
{
    public class FieldHover
    {

        public int MouseX { get; set; }
        public int MouseY { get; set; }
        public Coordinates Coordinates { get; set; }
        public ShipOrientation Orientation { get; set; }
        public ShipPiece ShipPiece { get; set; }
        public int HoverSize { get; set; }
    }
}
