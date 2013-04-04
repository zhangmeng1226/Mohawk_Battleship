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

namespace Battleship
{
    /// <summary>
    /// Interaction logic for Field.xaml
    /// </summary>
    public partial class Field : UserControl
    {
        Polyline axis;

        public Field()
        {
            InitializeComponent();
        }

        private void CreateAxis()
        {
            axis = new Polyline();
            axis.Stroke = System.Windows.Media.Brushes.Black;
            axis.StrokeThickness = 4;
            axis.FillRule = FillRule.EvenOdd;
            PointCollection axisLines = new PointCollection();
            axisLines.Add(new Point(0, 0));
            axisLines.Add(new Point(0, fieldGrid.ActualHeight));
            axisLines.Add(new Point(fieldGrid.ActualWidth, fieldGrid.ActualHeight));
            axis.Points = axisLines;
            Grid.SetColumn(axis, 1);
            Grid.SetRow(axis, 0);
            fieldGrid.Children.Add(axis);
        }

        private void FieldLoaded(object sender, RoutedEventArgs e)
        {
            Program.PrintDebugMessage("Width: " + ActualWidth);
            Program.PrintDebugMessage("Height: " + ActualHeight);
            CreateAxis();
            for (int i = 0; i < 10; i++)
            {
                fieldGrid.RowDefinitions.Add(new RowDefinition());
                fieldGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}
