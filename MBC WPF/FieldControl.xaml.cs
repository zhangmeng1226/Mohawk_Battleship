using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MBC.Core;

namespace MBC.WPF
{
    /// <summary>
    /// Interaction logic for Field.xaml
    /// </summary>
    public partial class FieldControl : UserControl
    {
        public static DependencyProperty ControllerColorProperty;

        static FieldControl()
        {
            ControllerColorProperty = DependencyProperty.Register("ControllerColor", typeof(Color), typeof(FieldControl));
        }

        IBattleshipController controller;
        Field battlefield;
        Dictionary<int, List<Rectangle>> ships;
        List<Ellipse> opponentShots;

        public FieldControl()
        {
            InitializeComponent();
            opponentShots = new List<Ellipse>();
            ships = new Dictionary<int, List<Rectangle>>();
        }

        public Color ControllerColor
        {
            get { return (Color)GetValue(ControllerColorProperty); }
            set { SetValue(ControllerColorProperty, value); }
        }

        private void MakeGrid(int rows, int cols)
        {
            fieldGrid.RowDefinitions.Clear();
            fieldGrid.ColumnDefinitions.Clear();
            fieldGrid.Children.Clear();
            opponentShots.Clear();
            for (int i = 0; i < rows; i++)
            {
                fieldGrid.RowDefinitions.Add(new RowDefinition());
                if (i == 0) continue;
                Line l = new Line();
                Grid.SetColumn(l, 0);
                Grid.SetRow(l, i);
                Grid.SetColumnSpan(l, cols);
                Grid.SetZIndex(l, 0);
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                l.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                l.Stroke = new SolidColorBrush(Colors.Black);
                l.StrokeThickness = 1;
                l.StrokeDashArray = new DoubleCollection();
                l.StrokeDashArray.Add(3);
                l.StrokeDashArray.Add(5);
                l.X2 = fieldGrid.ActualWidth;
                fieldGrid.Children.Add(l);
            }
            for (int i = 0; i < cols; i++)
            {
                fieldGrid.ColumnDefinitions.Add(new ColumnDefinition());
                if (i == 0) continue;
                Line l = new Line();
                Grid.SetColumn(l, i);
                Grid.SetRow(l, 0);
                Grid.SetRowSpan(l, rows);
                Grid.SetZIndex(l, 0);
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                l.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                l.Stroke = new SolidColorBrush(Colors.Black);
                l.StrokeThickness = 1;
                l.StrokeDashArray = new DoubleCollection();
                l.StrokeDashArray.Add(3);
                l.StrokeDashArray.Add(5);
                l.Y2 = fieldGrid.ActualHeight;
                fieldGrid.Children.Add(l);
            }
            fieldGrid.UpdateLayout();
        }

        private void CreateShips()
        {
            ships.Clear();
            foreach (int size in battlefield.shipSizes)
            {
                //All ships are oriented vertically, so rotate them when needed.
                Rectangle r = new Rectangle
                {
                    Width = fieldGrid.ActualWidth / fieldGrid.ColumnDefinitions.Count / 1.05,
                    Height = (fieldGrid.ActualHeight / fieldGrid.RowDefinitions.Count / 1.05) * size,
                    Fill = new SolidColorBrush(ControllerColor),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                };
                List<Rectangle> shipList = null;
                ships.TryGetValue(size, out shipList);
                if (shipList == null)
                {
                    shipList = new List<Rectangle>();
                    ships.Add(size, shipList);
                }
                shipList.Add(r);
            }
        }

        private void LayShips()
        {
            List<Rectangle> used = new List<Rectangle>();
            foreach (Ship ship in battlefield[controller].ships)
            {
                foreach (Rectangle rect in ships[ship.Length])
                {
                    if (used.Contains(rect)) continue;

                    rect.LayoutTransform = new RotateTransform(ship.Orientation == ShipOrientation.Horizontal ? 90 : 180, rect.Width / 2, 0);

                    Grid.SetRow(rect, battlefield.gameSize.Height - ship.Location.Y - ship.Length);
                    Grid.SetColumn(rect, ship.Location.X);
                    Grid.SetZIndex(rect, 100);
                    if (ship.Orientation == ShipOrientation.Horizontal)
                        Grid.SetColumnSpan(rect, ship.Length);
                    else
                        Grid.SetRowSpan(rect, ship.Length);
                    used.Add(rect);
                    break;
                }
            }
        }

        private void LayShots()
        {
            foreach (Ellipse e in opponentShots)
                fieldGrid.Children.Remove(e);
            opponentShots.Clear();
            foreach (System.Drawing.Point p in battlefield[battlefield.GetOpponent(controller)].shotsMade)
            {
                Ellipse circle = new Ellipse
                {
                    Width = fieldGrid.ActualWidth / fieldGrid.ColumnDefinitions.Count / 1.1,
                    Height = fieldGrid.ActualHeight / fieldGrid.RowDefinitions.Count / 1.1,
                    Fill = new SolidColorBrush(Colors.DarkGray),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 1,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                };
                Grid.SetRow(circle, battlefield.gameSize.Height - p.Y);
                Grid.SetColumn(circle, p.X);
                Grid.SetZIndex(circle, 99999);

                fieldGrid.Children.Add(circle);
            }
        }

        /**
         * <summary>Modifies the internal grid to represent the given Field. This may be
         * a CPU intensive process, so do not use this too often.</summary>
         * <param name="f">The field to monitor</param>
         * <param name="ibc">The controller to monitor in the field</param>
         */
        public void SetBattlefield(Field f, IBattleshipController ibc)
        {
            if (f[ibc] == null)
                throw new InvalidOperationException("Cannot set the FieldControl to contain a field with a non-existing controller.");
            if (controller != ibc)
            {
                controller = ibc;
                fieldLabel.Content = Util.ControllerToString(ibc);
            }
            if (battlefield == null)
                battlefield = new Field(f);
            if (f.gameSize.Width != battlefield.gameSize.Width || f.gameSize.Height != battlefield.gameSize.Height)
                MakeGrid(f.gameSize.Width, f.gameSize.Height);
            if (!f.shipSizes.SequenceEqual(battlefield.shipSizes))
                CreateShips();
            LayShips();
            LayShots();
        }

        private void FieldLoaded(object sender, RoutedEventArgs e)
        {
            MakeGrid(10, 10); //Just start the grid out with 10 x 10 squares.
            fieldLabel.Foreground = new SolidColorBrush(ControllerColor);
        }
    }
}
