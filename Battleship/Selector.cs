using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Battleship
{
    public partial class Selector : Form
    {
        Battleship2D main;
        public Selector(BattleshipConfig config, Battleship2D main)
        {
            InitializeComponent();
            this.main = main;
            firstBox.DataSource = config.BotNames;
            secondBox.DataSource = config.BotNames;
            firstBox.SelectionMode = SelectionMode.One;

            Show();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (firstBox.SelectedItem == null || secondBox.SelectedItem == null)
            {
                MessageBox.Show(this, "You must select at least one opponent for each box.", "Must Select", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (main.BotSelect((string)firstBox.SelectedItem, (string)secondBox.SelectedItem))
            {
                Hide();
                return;
            }

            MessageBox.Show(this, "Something went wrong when loading one of the two bots!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            main.BotSelect(null, null);
        }
    }
}
