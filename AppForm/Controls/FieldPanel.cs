using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MBC.Shared;

namespace MBC.App.FormBattleship.Controls
{
    public class FieldPanel : Panel
    {
        private Dictionary<Coordinates, Cell> cells = new Dictionary<Coordinates, Cell>();

        private Rectangle panelSize;
        private Rectangle cellSize;
        private Brush hoverBrush;
        private float rotation;

        public FieldPanel()
        {
            panelSize = new Rectangle(0, 0, Width, Height);
            cellSize = new Rectangle(0, 0, Width / 10, Height / 10);
            hoverBrush = new SolidBrush(Color.FromArgb(128, Color.Black));
            Resize += (sender, args) =>
            {
                panelSize = new Rectangle(0, 0, Width, Height);
                cellSize = new Rectangle(0, 0, Width / 10, Height / 10);
            };
        }

        public Dictionary<Coordinates, Cell> Cells
        {
            get { return cells; }
        }

        public FieldHover CurrentHover { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(FieldResources.WaterImage, panelSize);

            DrawCellStateAndShips(g);

            if (CurrentHover != null)
            {
                DrawCellHover(g);
            }
        }

        private void DrawCellHover(Graphics g)
        {
            int modX = CurrentHover.Orientation == ShipOrientation.Horizontal ? 1 : 0;
            int modY = CurrentHover.Orientation == ShipOrientation.Vertical ? 1 : 0;
            int hoverWidth = cellSize.Width + cellSize.Width * (CurrentHover.HoverSize - 1);
            int hoverHeight = cellSize.Height;
            Rectangle hoverRectangle = new Rectangle(0, 0, hoverWidth, hoverHeight);

            if (CurrentHover.ShipPiece != ShipPiece.None)
            {
                g.TranslateTransform(CurrentHover.MouseX + (30 * modY) - (30 * modX), CurrentHover.MouseY - (10 * modY) - (30 * modX));
                if (CurrentHover.Orientation == ShipOrientation.Vertical)
                {
                    g.RotateTransform(90f);
                }
                g.DrawImage(GetShipImage(CurrentHover.ShipPiece), hoverRectangle);
                g.ResetTransform();
            }
            if (CurrentHover.HoverSize > 0)
            {
                int hoverX = (CurrentHover.Coordinates.X + (1 * modY)) * cellSize.Width;
                int hoverY = (CurrentHover.Coordinates.Y + (0 * modX)) * cellSize.Height;
                g.TranslateTransform(hoverX, hoverY);
                if (CurrentHover.Orientation == ShipOrientation.Vertical)
                {
                    g.RotateTransform(90f);
                }

                g.FillRectangle(hoverBrush, hoverRectangle);
                g.ResetTransform();
            }
        }

        private void DrawCellStateAndShips(Graphics g)
        {
            foreach (Cell cell in cells.Values)
            {
                int transformX = cell.Coordinates.X + 1;
                int transformY = cell.Coordinates.Y + 1;
                if (cell.Orientation == ShipOrientation.Horizontal)
                {
                    transformY -= 1;
                    transformX -= 1;
                }
                else
                {
                    transformY -= 1;
                }

                g.TranslateTransform(transformX * cellSize.Width, transformY * cellSize.Height);
                if (cell.Orientation == ShipOrientation.Vertical)
                {
                    g.RotateTransform(90f);
                }
                if (cell.ShipPiece != ShipPiece.None)
                {
                    g.DrawImage(GetShipImage(cell.ShipPiece, cell.ShipSection), cellSize);
                }
                switch (cell.State)
                {
                    case CellState.Hit:
                        g.DrawImage(FieldResources.HitImage, cellSize);
                        break;
                    case CellState.Miss:
                        g.DrawImage(FieldResources.MissImage, cellSize);
                        break;
                        case CellState.Sunk:
                        g.DrawImage(FieldResources.SunkImage, cellSize);
                        break;
                }
                g.ResetTransform();
            }
        }

        private Image GetShipImage(ShipPiece piece, int section)
        {
            switch (piece)
            {
                case ShipPiece.Carrier:
                    return FieldResources.CarrierImages[section];
                case ShipPiece.Battleship:
                    return FieldResources.BattleshipImages[section];
                case ShipPiece.Cruiser:
                    return FieldResources.CruiserImages[section];
                case ShipPiece.Sub:
                    return FieldResources.SubImages[section];
                case ShipPiece.Destroyer:
                    return FieldResources.DestroyerImages[section];
            }
            return null;
        }

        private Image GetShipImage(ShipPiece piece)
        {
            switch (piece)
            {
                case ShipPiece.Carrier:
                    return FieldResources.CarrierImage;
                case ShipPiece.Battleship:
                    return FieldResources.BattleshipImage;
                case ShipPiece.Cruiser:
                    return FieldResources.CruiserImage;
                case ShipPiece.Sub:
                    return FieldResources.SubImage;
                case ShipPiece.Destroyer:
                    return FieldResources.DestroyerImage;
            }
            return null;
        }

        public void SetCellState(Coordinates coordinates, CellState state)
        {
            Cell cell;
            if (!Cells.TryGetValue(coordinates, out cell))
            {
                cell = new Cell();
                cell.ShipPiece = ShipPiece.None;
                cell.ShipSection = 0;
                cell.Coordinates = coordinates;
                Cells.Add(coordinates, cell);
            }
            cell.State = state;
        }

        public void SetCellShip(Coordinates coordinates, ShipPiece shipPiece, int section, ShipOrientation orientation)
        {
            Cell cell;
            if (!Cells.TryGetValue(coordinates, out cell))
            {
                cell = new Cell();
                cell.Coordinates = coordinates;
                Cells.Add(coordinates, cell);
            }
            cell.ShipPiece = shipPiece;
            cell.ShipSection = section;
            cell.Orientation = orientation;
        }

        public Coordinates GetCoordinatesFromMouse(int mouseX, int mouseY)
        {
            int resultX = mouseX / cellSize.Width;
            int resultY = mouseY / cellSize.Height;
            if (resultX == 10)
            {
                resultX = 9;
            }
            if (resultY == 10)
            {
                resultY = 9;
            }
            return new Coordinates(resultX, resultY);
        }
    }
}
