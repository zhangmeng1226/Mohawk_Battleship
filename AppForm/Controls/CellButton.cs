using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MBC.Shared;

namespace MBC.App.FormBattleship.Controls
{
    enum State { Open, Miss, Hit}

    class CellButton : Button
    {

        private State state;

        public CellButton(Coordinates coord)
        {
            Coordinate = coord;
            State = State.Open;
            Width = 60;
            Height = 60;
            Padding = new Padding(0);
            Margin = new Padding(0);
            ForeColor = System.Drawing.Color.White;
        }

        public Coordinates Coordinate { get; set; }

        public State State 
        {
            get
            {
                return state;
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
                }
                state = value;
            } 
        }

    }
}
