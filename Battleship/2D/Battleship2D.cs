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
    public partial class Battleship2D : Form, ProgramMode
    {
        BattleshipConfig config;
        private Battlefield2D fieldPanel;
        private IBattleshipOpponent[] bots;
        private BattleshipCompetition competition;

        public Battleship2D()
        {
            InitializeComponent();

            config = new BattleshipConfig(Environment.CurrentDirectory + "\\..\\config.ini");
            this.FormClosed += OnClose;

            fieldPanel = new Battlefield2D(new Size(419, 389));
            fieldPanel.Location = new Point(7, 7);
            fieldPanel.Name = "battlefieldView";
            tabPage1.Controls.Add(fieldPanel);

            configView.DataSource = config.GetDataSourceObject();
            cfgItemAddBtn.Enabled = false;
            cfgItemRemoveBtn.Enabled = false;
            cfgSaveBtn.Enabled = false;

            OptButtonsSet(false);

            SizeF scale = new SizeF(Screen.PrimaryScreen.Bounds.Width / 1920f, Screen.PrimaryScreen.Bounds.Height / 1080f);

            Scale(scale);

            roundPlays.Value = config.GetConfigValue<int>("competition_rounds", 110);
        }

        public void Start()
        {
            Show();
        }

        private void OptButtonsSet(bool state)
        {
            resetBtn.Enabled = state;
            fastPlayBtn.Enabled = state;
            newRndBtn.Enabled = state;
            rndTickBtn.Enabled = state;
        }

        private void UpdateScores(Dictionary<IBattleshipOpponent, int> sc)
        {
            op1ScoreLabel.Text = "" + sc[bots[0]];
            op2ScoreLabel.Text = "" + sc[bots[1]];
        }

        public bool BotSelect(string first, string second)
        {
            if (first == null || second == null)
                return false;
            bots = new IBattleshipOpponent[2];
            bots[0] = config.GetRobot(first);
            bots[1] = config.GetRobot(second);
            if (bots[0] != null && bots[1] != null)
            {
                competition = new BattleshipCompetition(bots, config);
                Dictionary<IBattleshipOpponent, int> scores = competition.RunCompetition();
                fieldPanel.SetBattlefield(competition.GetBattlefield());
                op1Label.Text = "[1]: " + bots[0].Name + " (v" + bots[0].Version + ")";
                op2Label.Text = "[2]: " + bots[1].Name + " (v" + bots[1].Version + ")";
                UpdateScores(scores);
                OptButtonsSet(true);
                rndTickBtn.Enabled = false;
                return true;
            }
            return false;
        }

        private void selectOpBtn_Click(object sender, EventArgs e)
        {
            Selector select = new Selector(config, this);
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            foreach (Battlefield.OpponentInfo op in competition.GetBattlefield().GetInfo())
                op.score = 0;
            op1ScoreLabel.Text = "" + 0;
            op2ScoreLabel.Text = "" + 0;
        }

        private void fastPlayBtn_Click(object sender, EventArgs e)
        {
            UpdateScores(competition.RunRounds((int)roundPlays.Value, playOutCheck.Checked));
            fieldPanel.Refresh();
        }

        private void newRndBtn_Click(object sender, EventArgs e)
        {
            competition.NewRound();
            rndTickBtn.Enabled = true;
            fieldPanel.Refresh();
        }

        private void rndTickBtn_Click(object sender, EventArgs e)
        {
            if (competition.RoundTick())
            {
                rndTickBtn.Enabled = false;
                op1ScoreLabel.Text = "" + competition.GetBattlefield().GetInfo()[0].score;
                op2ScoreLabel.Text = "" + competition.GetBattlefield().GetInfo()[1].score;
            }
            fieldPanel.Refresh();
        }

        private void OnClose(object sender, EventArgs e)
        {
            config.SaveConfigFile();
        }
    }
}
