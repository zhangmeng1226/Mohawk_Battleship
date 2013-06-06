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

namespace MBC.App.WPF
{
    /// <summary>
    /// The AppWPF application uses the MainWindow to provide controls and display information to the user graphically.
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<RoundActivityEntry> roundActLogEntries = new ObservableCollection<RoundActivityEntry>();
        ObservableCollection<RoundEntry> roundLogEntries = new ObservableCollection<RoundEntry>();
        Configuration config;

        /// <summary>
        /// Constructor for the MainWindow. Collapses the collapseable elements of the WPF application.
        /// </summary>
        public MainWindow()
        {
            Configuration.LoadConfigurationDefaults();
            InitializeComponent();
            centerConsoleBorder.Visibility = System.Windows.Visibility.Collapsed;
            advTabs.Visibility = System.Windows.Visibility.Collapsed;
            UpdateLayout();
            config = Configuration.Global;
        }

        /// <summary>
        /// Adds a RoundActivity to the display that shows information about the current RoundLog.
        /// </summary>
        /// <param name="action">The RoundActivity to append.</param>
        private void AddRoundActivity(RoundLog.RoundActivity action)
        {
            var entry = new RoundActivityEntry();
            entry.Number = roundActLogEntries.Count().ToString();
            entry.Action = RoundLog.GetActionStr(action.action);
            entry.ControllerName = action.fieldState != null ? Utility.ControllerToString(action.fieldState, action.ibc) : "Null";
            entry.Message = action.activityInfo;
            entry.Time = action.timeElapsed+"ms";

            var timeout = config.GetValue<long>("mbc_timeout");
            var diff = (int)(timeout - action.timeElapsed);
            diff = diff < 0 ? 255 : (int)((1 - (diff / timeout)) * 255);
            entry.Color = Color.FromArgb(255, 255, (byte)diff, (byte)diff);

            entry.Accolades = new Grid();
            var colNum = 0;
            foreach (RoundLog.RoundAccolade acc in action.accoladeTimelined)
            {
                var col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Auto);
                entry.Accolades.ColumnDefinitions.Add(col);

                var img = (Image)FindResource("Acc" + RoundLog.GetAccoladeStr(acc) + "Img");

                var tip = new ToolTip();
                tip.Content = RoundLog.GetAccoladeStr(acc);

                img.ToolTip = tip;
                Grid.SetColumn(img, colNum++);

                entry.Accolades.Children.Add(img);
            }
        }

        /// <summary>
        /// Provides information to the WPF ListView control that displays the current round's RoundLog.
        /// </summary>
        public class RoundActivityEntry
        {
            /// <summary>
            /// The activity number (starting from 1)
            /// </summary>
            public string Number;

            /// <summary>
            /// A string representation of the action that describes a RoundActivity.
            /// </summary>
            public string Action;

            /// <summary>
            /// The name of the controller that had performed this activity (if it applies)
            /// </summary>
            public string ControllerName;

            /// <summary>
            /// The message that provides extra detail about this activity.
            /// </summary>
            public string Message;

            /// <summary>
            /// A string representation of the time it took for the controller to perform this activity.
            /// </summary>
            public string Time;

            /// <summary>
            /// A WPF Grid component that contains icons of the accolades earned by this activity.
            /// </summary>
            public Grid Accolades;

            /// <summary>
            /// Colors the ListViewItem that displays this class's information based on the <see cref="Time"/>
            /// </summary>
            public Color Color;
        }

        /// <summary>
        /// Contained within a matchup, identifies a round by number and victor.
        /// </summary>
        public class RoundEntry
        {
            /// <summary>
            /// The round number in the benchmark.
            /// </summary>
            public string Number;

            /// <summary>
            /// A string representation of the name of the controller that had won this round.
            /// </summary>
            public string Victor;
        }

        /// <summary>
        /// Invoked when the size of either FieldControl attached to this MainWindow has changed, either by
        /// clicking a checkbox that changes a view, or through a window resize.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var s = new Size(fieldsContainer.ColumnDefinitions[0].ActualWidth, fieldsContainer.RowDefinitions[0].ActualHeight);
            if (s.Width == s.Height)
            {
                return;
            }
            double smaller = s.Width > s.Height ? s.Height : s.Width;
            redField.Width = smaller;
            redField.Height = smaller;
            blueField.Width = smaller;
            blueField.Height = smaller;
        }

        /// <summary>
        /// Invoked when the checkbox that represents the center console display, is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void consBasicCheck_Click(object sender, RoutedEventArgs e)
        {
            if (consBasicCheck.IsChecked == true)
            {
                centerConsoleBorder.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                centerConsoleBorder.Visibility = System.Windows.Visibility.Collapsed;
            }
            UpdateLayout();
            Grid_SizeChanged(null, null);
        }

        /// <summary>
        /// Invoked when the checkbox that represents the advanced tabs display, is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        
        /// <summary>Called when the user clicks on the blue opponent selector on the top menu.
        /// Should provide a popup menu displaying all of the available controllers.</summary>
        private void btnBlueSelect_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Reset Scores" button on the top menu.
        /// Should reset the scores between the two opponents.</summary>
        private void btnScoreReset_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the benchmark start button on the top menu.
        /// Should utilize txtNumOfRounds and chkPlayOut to modify the competition benchmark
        /// parameters.</summary>
        private void btnRndBegin_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "New Round" button on the top menu.
        /// Should end the current round between the two opponents, if it is still in progress,
        /// and start a new one.</summary>
        private void btnNewRound_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Shoot" button on the top menu.
        /// Should progress the current round if it is still in progress.</summary>
        private void btnRoundShoot_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the red opponent selector on the top menu.
        /// Should provide a popup menu displaying all of the available controllers.</summary>
        private void btnRedSelect_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Load Benchmark..." button in the Round Log tab.
        /// Should load a benchmark (a collection of rounds) from a file, and display all the rounds
        /// in the lstRoundLog view.</summary>
        private void BtnBenchmarkLoad_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Save Benchmark..." button in the Round Log tab.
        /// Should save a benchmark (a collection of rounds) to a file.</summary>
        private void BtnBenchmarkSave_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Load Round..." button in the Round Log tab.
        /// Should load the selected round from the lstRoundLog view into the main display.</summary>
        private void BtnRoundLoad_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>Called when the user clicks on the increment button in the top menu.
        /// Should increment the numeric value in txtNumOfRounds (# of rounds benchmark).</summary>
        private void btnRndsUp_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the decrement button in the top menu.
        /// Should decrement the numeric value in txtNumofRounds (# of rounds benchmark).</summary>
        private void btnRndsDown_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Add entry" button under the configuration tab.
        /// Should place a new entry into the lstConfigValues view and allow the user to modify it.</summary>
        private void BtnAddConfigEntry_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>Called when the user clicks on the "Remove entry" button under the configuration tab.
        /// Should remove the selected entry(s) in the lstConfigValues view, and from the global
        /// configuration.</summary>
        private void BtnRemConfigEntry_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>Called when the user clicks on the "Set entry to default" button under the configuration tab.
        /// Should cause the selected entry to revert to its default setting, and reflect this change in the
        /// lstConfigValues view.</summary>
        private void BtnEntryDefault_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Reset to default" button under the configuration tab.
        /// Should cause the current configuration to be cleared, or replaced with the default configuration.</summary>
        private void BtnResetConfig_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Save config..." button under the configuration tab.
        /// Should save the current configuration to a configuration file, under a name specified by the user.</summary>
        private void BtnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>Called when the user clicks on the "Load config..." button under the configuration tab.
        /// Should load the selected configuration in the lstConfigurations list from a file.</summary>
        private void BtnLoadConfig_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
