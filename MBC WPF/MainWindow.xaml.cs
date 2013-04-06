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
using System.Collections.ObjectModel;
using MBC.Core;
using System.ComponentModel;

namespace MBC.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<RoundActivityEntry> roundActLogEntries = new ObservableCollection<RoundActivityEntry>();
        ObservableCollection<RoundEntry> roundLogEntries = new ObservableCollection<RoundEntry>();
        Configuration config;

        public MainWindow()
        {
            InitializeComponent();
            redField.fieldLabel.Content = "Red";
            redField.fieldLabel.Foreground = System.Windows.Media.Brushes.Red;
            blueField.fieldLabel.Content = "Blue";
            blueField.fieldLabel.Foreground = System.Windows.Media.Brushes.Blue;
            config = Configuration.GetGlobalDefault();
        }

        private void AddRoundActivity(RoundLog.RoundActivity action)
        {
            RoundActivityEntry entry = new RoundActivityEntry();
            entry.Number = roundActLogEntries.Count().ToString();
            entry.Action = RoundLog.GetActionStr(action.action);
            entry.ControllerName = action.ibc != null ? Util.ControllerToString(action.ibc) : "Null";
            entry.Message = action.activityInfo;
            entry.Time = action.timeElapsed+"ms";

            long timeout = config.GetConfigValue<long>("timeout_millis", 500);
            int diff = (int)(timeout - action.timeElapsed);
            diff = diff < 0 ? 255 : (int)((1 - (diff / timeout)) * 255);
            entry.Color = Color.FromArgb(255, 255, (byte)diff, (byte)diff);

            entry.Accolades = new Grid();
            int colNum = 0;
            foreach (RoundLog.RoundAccolade acc in action.accoladeTimelined)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Auto);
                entry.Accolades.ColumnDefinitions.Add(col);

                Image img = (Image)FindResource("Acc" + RoundLog.GetAccoladeStr(acc) + "Img");

                ToolTip tip = new ToolTip();
                tip.Content = RoundLog.GetAccoladeStr(acc);

                img.ToolTip = tip;
                Grid.SetColumn(img, colNum++);

                entry.Accolades.Children.Add(img);
            }
        }

        public class RoundActivityEntry
        {
            public string Number;
            public string Action;
            public string ControllerName;
            public string Message;
            public string Time;
            public Grid Accolades;
            public Color Color;
        }

        public class RoundEntry
        {
            public string Number;
            public string Victor;
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

        private void BtnBenchmarkLoad_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnBenchmarkSave_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnRoundLoad_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnRoundSave_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnRndsUp_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnRndsDown_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
