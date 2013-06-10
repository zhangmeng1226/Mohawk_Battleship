using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal
{
    public class Size2D
    {
        private int width;
        private int height;

        public Size2D(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

    }
}
