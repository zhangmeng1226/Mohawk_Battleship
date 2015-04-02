using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MBC.App.FormBattleship.Properties;
using MBC.Shared;

namespace MBC.App.FormBattleship.Controls
{


    public class CellButton : Control
    {

        private static Image hitImage;
        private static Image missImage;
        private static Image[] carrierImages;
        private static Image[] battleshipImages;
        private static Image[] cruiserImages;
        private static Image[] destroyerImages;
        private static Image[] subImages;

        private Rectangle size;

        static CellButton()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            hitImage = LoadImage(assembly, "Missile_Hit.png");
            missImage = LoadImage(assembly, "Missile_Miss.png");
            carrierImages = new Image[5];
            battleshipImages = new Image[4];
            cruiserImages = new Image[3];
            subImages = new Image[3];
            destroyerImages = new Image[2];
            for (int i = 0; i < 5; i++)
            {
                carrierImages[i] = LoadImage(assembly, string.Format("Carrier{0}.png", i + 1));
            }
            for (int i = 0; i < 4; i++)
            {
                battleshipImages[i] = LoadImage(assembly, string.Format("Battleship{0}.png", i + 1));
            }
            for (int i = 0; i < 3; i++)
            {
                cruiserImages[i] = LoadImage(assembly, string.Format("Cruiser{0}.png", i + 1));
                subImages[i] = LoadImage(assembly, string.Format("Sub{0}.png", i + 1));
            }
            for (int i = 0; i < 2; i++)
            {
                destroyerImages[i] = LoadImage(assembly, string.Format("Destroyer{0}.png", i + 1));
            }
        }

        private static Image LoadImage(Assembly assembly, string img)
        {
            Stream imageStream = assembly.GetManifestResourceStream("MBC.App.FormBattleship.resources." + img);
            return Image.FromStream(imageStream);
        }

        public CellButton()
        {
            ShipState = State.Open;
            ShipPiece = ShipPiece.None;
            Width = 60;
            Height = 60;
            Padding = new Padding(0);
            Margin = new Padding(0);
            ForeColor = System.Drawing.Color.White;

            size = new Rectangle(0, 0, Width, Height);
        }
        public CellButton(Coordinates coord) :
            this()
        {
            Coordinate = coord;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Color reducedColor = Color.FromArgb(128, BackColor);
            g.FillRectangle(new SolidBrush(reducedColor), size);

            if (Orientation == ShipOrientation.Vertical)
            {
                g.RotateTransform(90.0f);
            }
            switch (ShipPiece)
            {
                case ShipPiece.Carrier:
                    g.DrawImage(carrierImages[Section], size);
                    break;
                case ShipPiece.Battleship:
                    g.DrawImage(battleshipImages[Section], size);
                    break;
                case ShipPiece.Cruiser:
                    g.DrawImage(cruiserImages[Section], size);
                    break;
                case ShipPiece.Destroyer:
                    g.DrawImage(destroyerImages[Section], size);
                    break;
                case ShipPiece.Sub:
                    g.DrawImage(subImages[Section], size);
                    break;
            }

            switch (ShipState)
            {
                case State.Open:
                    break;
                case State.Miss:
                    g.DrawImage(missImage, size);
                    break;
                case State.Hit:
                    g.DrawImage(hitImage, size);
                    break;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x000F)
            {
                DoPaint();
            }
        }

        protected void DoPaint()
        {
            Graphics g = CreateGraphics();

            Color reducedColor = Color.FromArgb(128, BackColor);
            g.FillRectangle(new SolidBrush(reducedColor), size);

            if (Orientation == ShipOrientation.Vertical)
            {
                g.RotateTransform(90.0f);
            }
            switch (ShipPiece)
            {
                case ShipPiece.Carrier:
                    g.DrawImage(carrierImages[Section], size);
                    break;
                case ShipPiece.Battleship:
                    g.DrawImage(battleshipImages[Section], size);
                    break;
                case ShipPiece.Cruiser:
                    g.DrawImage(cruiserImages[Section], size);
                    break;
                case ShipPiece.Destroyer:
                    g.DrawImage(destroyerImages[Section], size);
                    break;
                case ShipPiece.Sub:
                    g.DrawImage(subImages[Section], size);
                    break;
            }

            switch (ShipState)
            {
                case State.Open:
                    break;
                case State.Miss:
                    g.DrawImage(missImage, size);
                    break;
                case State.Hit:
                    g.DrawImage(hitImage, size);
                    break;
            }
        }

        public Coordinates Coordinate { get; set; }

        public State ShipState { get; set; }

        public int Section { get; set; }

        public ShipPiece ShipPiece { get; set; }

        public ShipOrientation Orientation
        {
            get;
            set;
        }
    }
}
