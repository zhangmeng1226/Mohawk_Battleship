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
    /// Interaction logic for Field.xaml
    /// </summary>
    public partial class Field : UserControl
    {
        public Field()
        {
            InitializeComponent();
        }

        private void FieldLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                fieldGrid.RowDefinitions.Add(new RowDefinition());
                fieldGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}
