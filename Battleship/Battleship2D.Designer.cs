using System.Windows.Forms;
namespace Battleship
{
    public partial class Battleship2D
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.playOutCheck = new System.Windows.Forms.CheckBox();
            this.op2ScoreLabel = new System.Windows.Forms.Label();
            this.op1ScoreLabel = new System.Windows.Forms.Label();
            this.selectOpBtn = new System.Windows.Forms.Button();
            this.rndTickBtn = new System.Windows.Forms.Button();
            this.newRndBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.fastPlayBtn = new System.Windows.Forms.Button();
            this.roundPlays = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.resetBtn = new System.Windows.Forms.Button();
            this.op2Label = new System.Windows.Forms.Label();
            this.op1Label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cfgSaveBtn = new System.Windows.Forms.Button();
            this.cfgItemRemoveBtn = new System.Windows.Forms.Button();
            this.cfgItemAddBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.configView = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roundPlays)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(635, 431);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.playOutCheck);
            this.tabPage1.Controls.Add(this.op2ScoreLabel);
            this.tabPage1.Controls.Add(this.op1ScoreLabel);
            this.tabPage1.Controls.Add(this.selectOpBtn);
            this.tabPage1.Controls.Add(this.rndTickBtn);
            this.tabPage1.Controls.Add(this.newRndBtn);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.fastPlayBtn);
            this.tabPage1.Controls.Add(this.roundPlays);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.resetBtn);
            this.tabPage1.Controls.Add(this.op2Label);
            this.tabPage1.Controls.Add(this.op1Label);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(627, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Battlefield View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // playOutCheck
            // 
            this.playOutCheck.AutoSize = true;
            this.playOutCheck.Location = new System.Drawing.Point(441, 207);
            this.playOutCheck.Name = "playOutCheck";
            this.playOutCheck.Size = new System.Drawing.Size(137, 21);
            this.playOutCheck.TabIndex = 16;
            this.playOutCheck.Text = "Play out rounds?";
            this.playOutCheck.UseVisualStyleBackColor = true;
            // 
            // op2ScoreLabel
            // 
            this.op2ScoreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.op2ScoreLabel.AutoSize = true;
            this.op2ScoreLabel.Location = new System.Drawing.Point(583, 66);
            this.op2ScoreLabel.Name = "op2ScoreLabel";
            this.op2ScoreLabel.Size = new System.Drawing.Size(16, 17);
            this.op2ScoreLabel.TabIndex = 15;
            this.op2ScoreLabel.Text = "0";
            this.op2ScoreLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // op1ScoreLabel
            // 
            this.op1ScoreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.op1ScoreLabel.AutoSize = true;
            this.op1ScoreLabel.Location = new System.Drawing.Point(583, 49);
            this.op1ScoreLabel.Name = "op1ScoreLabel";
            this.op1ScoreLabel.Size = new System.Drawing.Size(16, 17);
            this.op1ScoreLabel.TabIndex = 14;
            this.op1ScoreLabel.Text = "0";
            this.op1ScoreLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // selectOpBtn
            // 
            this.selectOpBtn.Location = new System.Drawing.Point(438, 87);
            this.selectOpBtn.Name = "selectOpBtn";
            this.selectOpBtn.Size = new System.Drawing.Size(183, 23);
            this.selectOpBtn.TabIndex = 13;
            this.selectOpBtn.Text = "Select Opponents";
            this.selectOpBtn.UseVisualStyleBackColor = true;
            this.selectOpBtn.Click += new System.EventHandler(this.selectOpBtn_Click);
            // 
            // rndTickBtn
            // 
            this.rndTickBtn.Location = new System.Drawing.Point(439, 334);
            this.rndTickBtn.Name = "rndTickBtn";
            this.rndTickBtn.Size = new System.Drawing.Size(182, 23);
            this.rndTickBtn.TabIndex = 12;
            this.rndTickBtn.Text = "Tick";
            this.rndTickBtn.UseVisualStyleBackColor = true;
            this.rndTickBtn.Click += new System.EventHandler(this.rndTickBtn_Click);
            // 
            // newRndBtn
            // 
            this.newRndBtn.Location = new System.Drawing.Point(438, 304);
            this.newRndBtn.Name = "newRndBtn";
            this.newRndBtn.Size = new System.Drawing.Size(183, 23);
            this.newRndBtn.TabIndex = 11;
            this.newRndBtn.Text = "New Round";
            this.newRndBtn.UseVisualStyleBackColor = true;
            this.newRndBtn.Click += new System.EventHandler(this.newRndBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(435, 283);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Round Control:";
            // 
            // fastPlayBtn
            // 
            this.fastPlayBtn.Location = new System.Drawing.Point(441, 234);
            this.fastPlayBtn.Name = "fastPlayBtn";
            this.fastPlayBtn.Size = new System.Drawing.Size(180, 23);
            this.fastPlayBtn.TabIndex = 9;
            this.fastPlayBtn.Text = "Go";
            this.fastPlayBtn.UseVisualStyleBackColor = true;
            this.fastPlayBtn.Click += new System.EventHandler(this.fastPlayBtn_Click);
            // 
            // roundPlays
            // 
            this.roundPlays.Location = new System.Drawing.Point(506, 179);
            this.roundPlays.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.roundPlays.Name = "roundPlays";
            this.roundPlays.Size = new System.Drawing.Size(115, 22);
            this.roundPlays.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(438, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Rounds:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(435, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Fast play:";
            // 
            // resetBtn
            // 
            this.resetBtn.Location = new System.Drawing.Point(436, 116);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(185, 23);
            this.resetBtn.TabIndex = 5;
            this.resetBtn.Text = "Reset Scores";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // op2Label
            // 
            this.op2Label.AutoSize = true;
            this.op2Label.Location = new System.Drawing.Point(436, 66);
            this.op2Label.Name = "op2Label";
            this.op2Label.Size = new System.Drawing.Size(32, 17);
            this.op2Label.TabIndex = 4;
            this.op2Label.Text = "[2]: ";
            // 
            // op1Label
            // 
            this.op1Label.AutoSize = true;
            this.op1Label.Location = new System.Drawing.Point(436, 49);
            this.op1Label.Name = "op1Label";
            this.op1Label.Size = new System.Drawing.Size(32, 17);
            this.op1Label.TabIndex = 3;
            this.op1Label.Text = "[1]: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(433, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Opponents:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(432, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Battleship Round Control";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.configView);
            this.tabPage2.Controls.Add(this.cfgSaveBtn);
            this.tabPage2.Controls.Add(this.cfgItemRemoveBtn);
            this.tabPage2.Controls.Add(this.cfgItemAddBtn);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(627, 402);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cfgSaveBtn
            // 
            this.cfgSaveBtn.Location = new System.Drawing.Point(433, 134);
            this.cfgSaveBtn.Name = "cfgSaveBtn";
            this.cfgSaveBtn.Size = new System.Drawing.Size(188, 23);
            this.cfgSaveBtn.TabIndex = 4;
            this.cfgSaveBtn.Text = "Save Configuration";
            this.cfgSaveBtn.UseVisualStyleBackColor = true;
            // 
            // cfgItemRemoveBtn
            // 
            this.cfgItemRemoveBtn.Location = new System.Drawing.Point(436, 58);
            this.cfgItemRemoveBtn.Name = "cfgItemRemoveBtn";
            this.cfgItemRemoveBtn.Size = new System.Drawing.Size(185, 23);
            this.cfgItemRemoveBtn.TabIndex = 3;
            this.cfgItemRemoveBtn.Text = "Remove Item";
            this.cfgItemRemoveBtn.UseVisualStyleBackColor = true;
            // 
            // cfgItemAddBtn
            // 
            this.cfgItemAddBtn.Location = new System.Drawing.Point(436, 28);
            this.cfgItemAddBtn.Name = "cfgItemAddBtn";
            this.cfgItemAddBtn.Size = new System.Drawing.Size(185, 23);
            this.cfgItemAddBtn.TabIndex = 2;
            this.cfgItemAddBtn.Text = "Add Item";
            this.cfgItemAddBtn.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(433, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "Configuration Editor:";
            // 
            // configView
            // 
            this.configView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.configView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.configView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.configView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.configView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.configView.EnableHeadersVisualStyles = false;
            this.configView.Location = new System.Drawing.Point(7, 7);
            this.configView.MultiSelect = false;
            this.configView.Name = "configView";
            this.configView.RowHeadersVisible = false;
            this.configView.RowHeadersWidth = 4;
            this.configView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.configView.RowTemplate.Height = 24;
            this.configView.Size = new System.Drawing.Size(420, 389);
            this.configView.TabIndex = 5;
            // 
            // Battleship2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 456);
            this.Controls.Add(this.tabControl1);
            this.Name = "Battleship2D";
            this.Text = "Battleship2D";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.roundPlays)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.configView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.NumericUpDown roundPlays;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Label op2Label;
        private System.Windows.Forms.Label op1Label;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button fastPlayBtn;
        private System.Windows.Forms.Button rndTickBtn;
        private System.Windows.Forms.Button newRndBtn;
        private System.Windows.Forms.Button cfgSaveBtn;
        private System.Windows.Forms.Button cfgItemRemoveBtn;
        private System.Windows.Forms.Button cfgItemAddBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button selectOpBtn;
        private Label op2ScoreLabel;
        private Label op1ScoreLabel;
        private CheckBox playOutCheck;
        private DataGridView configView;
    }
}