using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBC.Shared;

namespace MBC.App.FormBattleship.Controls
{
    public enum State { Open, Miss, Hit }
    public enum ShipPiece { None, Carrier, Battleship, Cruiser, Destroyer, Sub }

    public class Cell
    {
        public Coordinates Coordinates { get; set; }
        public ShipPiece ShipPiece { get; set; }
        public ShipOrientation Orientation { get; set; }
        public CellState State { get; set; }
        public int ShipSection { get; set; }
    }
}
