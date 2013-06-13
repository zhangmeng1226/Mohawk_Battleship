using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal
{
    public class Coordinates2D
    {

        private int x;
        private int y;

        public Coordinates2D(int coordX, int coordY)
        {
            x = coordX;
            y = coordY;
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
    }
}
