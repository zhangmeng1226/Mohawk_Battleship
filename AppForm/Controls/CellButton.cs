using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MBC.Shared;

namespace MBC.App.FormBattleship.Controls
{
    public enum State { Open, Miss, Hit, Sunk}

    public class CellButton : Button
    {

        private State shipState;

        public CellButton()
        {
            ShipState = State.Open;
            Width = 60;
            Height = 60;
            Padding = new Padding(0);
            Margin = new Padding(0);
            ForeColor = System.Drawing.Color.White;
        }
        public CellButton(Coordinates coord) : 
            this()
        {
            Coordinate = coord;
        }

        public Coordinates Coordinate { get; set; }

        public State ShipState 
        {
            get
            {
                return shipState;
            }
            set 
            {
                switch (value)
                {
                    case State.Open:
                        Text = "Open";
                        BackColor = System.Drawing.Color.Black;
                        break;
                    case State.Miss:
                        Text = "Miss";
                        BackColor = System.Drawing.Color.SkyBlue;
                        break;
                    case State.Hit:
                        Text = "Hit";
                        BackColor = System.Drawing.Color.Tomato;
                        break;
                    case State.Sunk:
                        Text = "SUNK";
                        BackColor = System.Drawing.Color.Crimson;
                        break;
                }
                shipState = value;
            } 
        }

    }
}
