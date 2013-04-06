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
        IBattleshipController controller;
        Field battlefield;

        public FieldControl()
        {
            InitializeComponent();
        }

        private void MakeGrid(int rows, int cols)
        {
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
        }

        /**
         * <summary>Modifies the internal grid to represent the given Field.</summary>
         * <param name="f">The field to monitor</param>
         * <param name="ibc">The controller to monitor in the field</param>
         */
        public void SetBattlefield(Field f, IBattleshipController ibc)
        {
            controller = ibc;
            battlefield = f;
            MakeGrid(f.gameSize.Width, f.gameSize.Height);
            fieldLabel.Content = Util.ControllerToString(ibc);
        }

        private void FieldLoaded(object sender, RoutedEventArgs e)
        {
            MakeGrid(10, 10); //Just start the grid out with 10 x 10 squares.
        }
    }
}
