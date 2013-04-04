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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /**
         * Not spending any more time on this... WPF is too new to me (Ryan A).
         * It sort of does what it needs to do; maintain a 1:1 ratio. Glitches up a bit though.
         */
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size s = new Size(fieldsContainer.ColumnDefinitions[0].ActualWidth, fieldsContainer.RowDefinitions[0].ActualHeight);
            if (s.Width != s.Height)
            {
                if (s.Width > s.Height)
                {
                    redField.Width = s.Height;
                    blueField.Width = s.Height;
                    redField.Width = s.Height;
                    blueField.Width = s.Height;
                }
                else
                {
                    blueField.Height = s.Width;
                    blueField.Width = s.Width;
                    redField.Height = s.Width;
                    redField.Width = s.Width;
                }
            }
        }
    }
}
