using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Battleship
{
    public partial class Battlefield2D : UserControl
    {
        private Font[] textFonts;
        private Brush[] brushes;
        private Pen[] pens;
        private PointF[] textPoints;
        private Rectangle[] rectangles;

        private Battlefield fieldInfo;

        public Battlefield2D(Size size)
        {
            InitializeComponent();
            DoubleBuffered = true;
            Paint += new PaintEventHandler(Battlefield2D_Paint);
            Size = size;

            textFonts = new Font[4];
            textFonts[0] = new Font("Trebuchet MS", 9, FontStyle.Bold);
            textFonts[1] = new Font("Trebuchet MS", 9, FontStyle.Regular);
            textFonts[2] = new Font("Comic Sans MS", 8, FontStyle.Italic | FontStyle.Bold);
            textFonts[3] = new Font("Comic Sans MS", 8, FontStyle.Italic);

            brushes = new Brush[6];
            brushes[0] = new SolidBrush(Color.Black);
            brushes[1] = new SolidBrush(Color.Red);
            brushes[2] = new SolidBrush(Color.FromArgb(56, Color.Red));
            brushes[3] = new SolidBrush(Color.Blue);
            brushes[4] = new SolidBrush(Color.FromArgb(56, Color.Blue));
            brushes[5] = new SolidBrush(Color.LightGray);

            pens = new Pen[2];
            pens[0] = new Pen(brushes[0]);
            pens[1] = new Pen(brushes[0], 3);

            rectangles = new Rectangle[6];
            rectangles[0] = new Rectangle(0, 0, Width - 1, Height - 1);
            rectangles[1] = new Rectangle(98, 35, 10, 10);
            rectangles[2] = new Rectangle(98, 55, 10, 10);
            rectangles[3] = new Rectangle(278, 35, 10, 10);
            rectangles[4] = new Rectangle(278, 55, 10, 10);
            rectangles[5] = new Rectangle(28, 70, Width - 49, Height - 92);

            textPoints = new PointF[10];
            textPoints[0] = new PointF(7, 7);
            textPoints[1] = new PointF(7, 30);
            textPoints[2] = new PointF(110, 30);
            textPoints[3] = new PointF(110, 50);
            textPoints[4] = new PointF(190, 30);
            textPoints[5] = new PointF(290, 30);
            textPoints[6] = new PointF(290, 50);
            textPoints[7] = new PointF(21, 50);
            textPoints[8] = new PointF(Width - 21, Height - 35);
            textPoints[9] = new PointF(10, Height - 22);
        }

        public void SetBattlefield(Battlefield field)
        {
            fieldInfo = field;
            Refresh();
        }

        void DrawLines(Graphics draw, float cellWidth, float cellHeight)
        {
            for (int x = 1; x <= fieldInfo.gameSize.Width; x++)
            {
                float xOffset = (x * cellWidth);
                draw.DrawLine(pens[0],
                    rectangles[5].X + xOffset,
                    rectangles[5].Y + rectangles[5].Height,
                    rectangles[5].X + xOffset,
                    rectangles[5].Y);
                draw.DrawString("" + (x-1), textFonts[3], brushes[0], textPoints[9].X + xOffset, textPoints[9].Y);
            }
            for (int y = 1; y <= fieldInfo.gameSize.Height; y++)
            {
                float yOffset = (y * cellHeight);
                draw.DrawLine(pens[0],
                    rectangles[5].X,
                    rectangles[5].Y + yOffset,
                    rectangles[5].X + rectangles[5].Width,
                    rectangles[5].Y + yOffset);
                draw.DrawString("" + (y-1), textFonts[3], brushes[0], textPoints[9].X, textPoints[9].Y - yOffset);
            }
        }

        void DrawShips(Graphics draw, float cellWidth, float cellHeight)
        {
            Battlefield.OpponentInfo[] ops = fieldInfo.GetInfo();
            for (int i = 0; i < ops.Count(); i++)
            {
                int colOffset = i * 2;
                int tOffset;
                if (i == 0)
                    tOffset = 4;
                else
                    tOffset = 8;
                foreach (Ship ship in ops[i].ships)
                {
                    float xStartCalc = cellWidth * ship.Location.X;
                    float yStartCalc = cellHeight * (ship.Location.Y + 1);
                    float xStartGrid = rectangles[5].X + xStartCalc + tOffset;
                    float yStartGrid = rectangles[5].Y + rectangles[5].Height - yStartCalc + tOffset;
                    float rectWidth = cellWidth - 14;
                    float rectHeight = cellHeight - 14;
                    float xCenter = xStartGrid + rectWidth / 2;
                    float yCenter = yStartGrid + rectHeight / 2;
                    float oXCenter = rectWidth;
                    float oYCenter = rectHeight;

                    if (ship.Orientation == ShipOrientation.Horizontal)
                        rectWidth += cellWidth * (ship.Length-1);
                    else
                    {
                        rectHeight += cellHeight * (ship.Length-1);
                        yStartGrid -= cellHeight * (ship.Length-1);
                    }

                    draw.FillRectangle(brushes[1 + colOffset],
                        xStartGrid,
                        yStartGrid,
                        rectWidth,
                        rectHeight);
                    draw.DrawLine(pens[1],
                        xCenter,
                        yCenter,
                        xCenter + rectWidth - oXCenter,
                        yCenter - rectHeight + oYCenter);
                    draw.FillRectangle(brushes[0], xCenter - 5, yCenter - 5, 10, 10);
                    draw.FillRectangle(brushes[0], xCenter + rectWidth - oXCenter - 5, yCenter - rectHeight + oYCenter - 5, 10, 10);
                }
            }
        }

        void DrawShots(Graphics draw, float cellWidth, float cellHeight)
        {
            Battlefield.OpponentInfo[] ops = fieldInfo.GetInfo();
            for (int i = 0; i < ops.Count(); i++)
            {
                foreach (Point p in ops[i].shotsMade)
                {
                    GraphicsPath hitPath = new GraphicsPath();
                    PointF point1 = new PointF(rectangles[5].X + (cellWidth * p.X), rectangles[5].Y + (cellHeight * (p.Y + 1)));
                    PointF point2 = new PointF(rectangles[5].X + (cellWidth * (p.X + 1)), rectangles[5].Y + (cellHeight * p.Y));
                    PointF point3 = new PointF(rectangles[5].X + (cellWidth * (p.X + i)), rectangles[5].Y + (cellHeight * (p.Y + i)));

                    hitPath.AddLine(point1, point2);
                    hitPath.AddBezier(point1, point3, point3, point2);
                    draw.FillPath(brushes[2 + (i * 2)], hitPath);
                    draw.DrawPath(pens[0], hitPath);
                }
            }
        }

        void DrawLayout(Graphics draw)
        {
            draw.DrawRectangle(pens[0], rectangles[0]);
            draw.DrawString("Legend:", textFonts[0], brushes[0], textPoints[0]);
            draw.DrawString("Opponent 1:", textFonts[1], brushes[0], textPoints[1]);
            draw.DrawString("Ships", textFonts[1], brushes[0], textPoints[2]);
            draw.DrawString("Shots", textFonts[1], brushes[0], textPoints[3]);
            draw.DrawString("Opponent 2:", textFonts[1], brushes[0], textPoints[4]);
            draw.DrawString("Ships", textFonts[1], brushes[0], textPoints[5]);
            draw.DrawString("Shots", textFonts[1], brushes[0], textPoints[6]);
            draw.DrawString("Y", textFonts[2], brushes[0], textPoints[7]);
            draw.DrawString("X", textFonts[2], brushes[0], textPoints[8]);

            draw.FillRectangle(brushes[1], rectangles[1]);
            draw.FillRectangle(brushes[2], rectangles[2]);
            draw.FillRectangle(brushes[3], rectangles[3]);
            draw.FillRectangle(brushes[4], rectangles[4]);
            draw.FillRectangle(brushes[5], rectangles[5]);

            draw.DrawRectangle(pens[0], rectangles[1]);
            draw.DrawRectangle(pens[0], rectangles[2]);
            draw.DrawRectangle(pens[0], rectangles[3]);
            draw.DrawRectangle(pens[0], rectangles[4]);
            draw.DrawRectangle(pens[0], rectangles[5]);
        }

        void Battlefield2D_Paint(object sender, PaintEventArgs e)
        {
            Graphics draw = e.Graphics;

            DrawLayout(draw);
            if (fieldInfo != null)
            {
                float cellWidth = (rectangles[5].Width / (float)fieldInfo.gameSize.Width);
                float cellHeight = (rectangles[5].Height / (float)fieldInfo.gameSize.Height);
                DrawShips(draw, cellWidth, cellHeight);
                DrawLines(draw, cellWidth, cellHeight);
                DrawShots(draw, cellWidth, cellHeight);
            }
        }
    }
}
