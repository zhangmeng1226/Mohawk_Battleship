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

namespace MBC.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            redField.fieldLabel.Content = "Red";
            redField.fieldLabel.Foreground = System.Windows.Media.Brushes.Red;
            blueField.fieldLabel.Content = "Blue";
            blueField.fieldLabel.Foreground = System.Windows.Media.Brushes.Blue;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size s = new Size(fieldsContainer.ColumnDefinitions[0].ActualWidth, fieldsContainer.RowDefinitions[0].ActualHeight);
            if (s.Width == s.Height)
                return;
            double smaller = s.Width > s.Height ? s.Height : s.Width;
            redField.Width = smaller;
            redField.Height = smaller;
            blueField.Width = smaller;
            blueField.Height = smaller;
        }

        private void consBasicCheck_Click(object sender, RoutedEventArgs e)
        {
            if (consBasicCheck.IsChecked == true)
                centerConsoleBorder.Visibility = System.Windows.Visibility.Visible;
            else
                centerConsoleBorder.Visibility = System.Windows.Visibility.Collapsed;
            UpdateLayout();
            Grid_SizeChanged(null, null);
        }

        private void consAdvCheck_Click(object sender, RoutedEventArgs e)
        {
            if (consAdvCheck.IsChecked == true)
            {
                advTabs.Visibility = System.Windows.Visibility.Visible;
                Height += 180;
            }
            else
            {
                Height -= 180;
                advTabs.Visibility = System.Windows.Visibility.Collapsed;
            }
            UpdateLayout();
            Grid_SizeChanged(null, null);
        }

        private void btnBlueSelect_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnScoreReset_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnRndBegin_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnNewRound_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnRoundShoot_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnRedSelect_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
