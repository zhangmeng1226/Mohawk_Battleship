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

namespace MBC.App.WPF
{
    /// <summary>
    /// The FieldControl class is a WPF control that graphically displays the state of a Field object.
    /// The FieldControl must be given the IBattleshipController index that is being displayed on this Field.
    /// Usage of this class is simple by utilizing three class members:
    /// <list type="bullet">
    ///      <item>SetController(int) -  Sets the controller index in a Field.</item>
    ///      <item>SetField(Field) - Sets the Field to display.</item>
    ///      <item>UpdateFieldDisplay() - Updates the FieldControl to display the Field's current state.</item>
    /// </list>
    /// </summary>
    public partial class FieldControl : UserControl
    {
        /// <summary>
        /// The Color property that this FieldControl uses to display a certain controller.
        /// </summary>
        public static DependencyProperty ControllerColorProperty;

        static FieldControl()
        {
            ControllerColorProperty = DependencyProperty.Register("ControllerColor", typeof(Color), typeof(FieldControl));
        }

        int controller; //The controller index this FieldControl displays information for.
        Field battlefield; //The Field object this FieldControl gets the information from.
        Dictionary<int, List<Rectangle>> ships; //A Dictionary containing graphics information for the sizes of ships.
        List<Ellipse> opponentShots; //A List of ellipses drawn on the display.

        /// <summary>
        /// Constructs a FieldControl. Does not do anything special besides initialize WPF components.
        /// </summary>
        public FieldControl()
        {
            InitializeComponent();
            opponentShots = new List<Ellipse>();
            ships = new Dictionary<int, List<Rectangle>>();
        }

        /// <summary>
        /// Gets or sets the colour representation of this FieldControl.
        /// </summary>
        public Color ControllerColor
        {
            get { return (Color)GetValue(ControllerColorProperty); }
            set { SetValue(ControllerColorProperty, value); }
        }

        /// <summary>
        /// Generates the WPF grid with a specified number of rows and columns. Internally used.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="cols">The number of columns.</param>
        private void MakeGrid(int rows, int cols)
        {
            fieldGrid.RowDefinitions.Clear();
            fieldGrid.ColumnDefinitions.Clear();
            fieldGrid.Children.Clear();
            opponentShots.Clear();
            for (int i = 0; i < rows; i++)
            {
                fieldGrid.RowDefinitions.Add(new RowDefinition());
                if (i == 0)
                {
                    continue;
                }
                var l = new Line();
                Grid.SetColumn(l, 0);
                Grid.SetRow(l, i);
                Grid.SetColumnSpan(l, cols);
                Grid.SetZIndex(l, 0);
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                l.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                l.Stroke = new SolidColorBrush(Colors.DarkGray);
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
                if (i == 0)
                {
                    continue;
                }
                var l = new Line();
                Grid.SetColumn(l, i);
                Grid.SetRow(l, 0);
                Grid.SetRowSpan(l, rows);
                Grid.SetZIndex(l, 0);
                l.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                l.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                l.Stroke = new SolidColorBrush(Colors.DarkGray);
                l.StrokeThickness = 1;
                l.StrokeDashArray = new DoubleCollection();
                l.StrokeDashArray.Add(3);
                l.StrokeDashArray.Add(5);
                l.Y2 = fieldGrid.ActualHeight;
                fieldGrid.Children.Add(l);
            }
            fieldGrid.UpdateLayout();
        }

        /// <summary>
        /// Creates the graphics that represent the ships that will be displayed. Internally used.
        /// </summary>
        private void CreateShips()
        {
            ships.Clear();
            foreach (int size in battlefield.shipSizes)
            {
                //All ships are oriented vertically, so rotate them when needed.
                var r = new Rectangle
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

        /// <summary>
        /// Sets the Rectangle objects that represent the ships that will be displayed. Internally used.
        /// </summary>
        private void LayShips()
        {
            var used = new List<Rectangle>();
            foreach (Ship ship in battlefield[controller].ships)
            {
                foreach (Rectangle rect in ships[ship.Length])
                {
                    if (used.Contains(rect))
                    {
                        continue;
                    }

                    rect.LayoutTransform = new RotateTransform(ship.Orientation == ShipOrientation.Horizontal ? 90 : 180, rect.Width / 2, 0);

                    Grid.SetRow(rect, battlefield.gameSize.Height - ship.Location.Y - ship.Length);
                    Grid.SetColumn(rect, ship.Location.X);
                    Grid.SetZIndex(rect, 100);
                    if (ship.Orientation == ShipOrientation.Horizontal)
                    {
                        Grid.SetColumnSpan(rect, ship.Length);
                    }
                    else
                    {
                        Grid.SetRowSpan(rect, ship.Length);
                    }
                    used.Add(rect);
                    break;
                }
            }
        }

        /// <summary>
        /// Sets the Ellipse objects that represent the opponent controller's shots which will be displayed
        /// on the grid. Internally used.
        /// </summary>
        private void LayShots()
        {
            foreach (Ellipse e in opponentShots)
            {
                fieldGrid.Children.Remove(e);
            }
            opponentShots.Clear();
            foreach (System.Drawing.Point p in battlefield[1 - controller].shotsMade)
            {
                var circle = new Ellipse
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
         * <summary>Sets the controller that this FieldControl displays through the internally presented Field.</summary>
         * <param name="ibc">The index of the battleship controller in the Field to switch to. Refer to the
         * constants available in the Controller class.</param>
         * <seealso cref="Field"/>
         */
        public void SetController(int ibc)
        {
            controller = ibc;
        }

        /**
         * <summary>Makes this FieldControl update its display to reflect the 
         * current state of the internal Field.</summary>
         */
        public void UpdateFieldDisplay()
        {
            LayShips();
            LayShots();
        }

        /**
         * <summary>Sets the Field this FieldControl will present.</summary>
         * <param name="field">The Field object to display.</param>
         * <seealso cref="Field"/>
         */
        public void SetField(Field field)
        {
            battlefield = field;
            fieldLabel.Content = Utility.ControllerToString(field, controller);
            MakeGrid(field.gameSize.Width, field.gameSize.Height);
            CreateShips();
            LayShips();
            LayShots();
        }

        /// <summary>
        /// Invoked when the FieldControl has been loaded into the WPF interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FieldLoaded(object sender, RoutedEventArgs e)
        {
            MakeGrid(10, 10); //Just start the grid out with 10 x 10 squares.
            fieldLabel.Foreground = new SolidColorBrush(ControllerColor);
        }
    }
}
