namespace MBC.App.FormBattleship
{
  partial class FormBattleShip
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
            this.components = new System.ComponentModel.Container();
            this.gridUser = new System.Windows.Forms.TableLayoutPanel();
            this.gridAI = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelFire = new System.Windows.Forms.Label();
            this.timerAI = new System.Windows.Forms.Timer(this.components);
            this.button5Ship = new System.Windows.Forms.Button();
            this.button4Ship = new System.Windows.Forms.Button();
            this.button3Ship1 = new System.Windows.Forms.Button();
            this.button3Ship2 = new System.Windows.Forms.Button();
            this.button2Ship = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gridUser
            // 
            this.gridUser.ColumnCount = 10;
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridUser.Location = new System.Drawing.Point(679, 68);
            this.gridUser.Margin = new System.Windows.Forms.Padding(0);
            this.gridUser.Name = "gridUser";
            this.gridUser.RowCount = 10;
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridUser.Size = new System.Drawing.Size(600, 600);
            this.gridUser.TabIndex = 0;
            // 
            // gridAI
            // 
            this.gridAI.ColumnCount = 10;
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gridAI.Location = new System.Drawing.Point(35, 68);
            this.gridAI.Margin = new System.Windows.Forms.Padding(0);
            this.gridAI.Name = "gridAI";
            this.gridAI.RowCount = 10;
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridAI.Size = new System.Drawing.Size(600, 600);
            this.gridAI.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(168, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "User BattleShip Board";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(841, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(315, 37);
            this.label2.TabIndex = 1;
            this.label2.Text = "AI BattleShip Board";
            // 
            // labelFire
            // 
            this.labelFire.AutoSize = true;
            this.labelFire.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFire.ForeColor = System.Drawing.Color.White;
            this.labelFire.Location = new System.Drawing.Point(510, 702);
            this.labelFire.Name = "labelFire";
            this.labelFire.Size = new System.Drawing.Size(0, 37);
            this.labelFire.TabIndex = 1;
            // 
            // timerAI
            // 
            this.timerAI.Interval = 1000;
            this.timerAI.Tick += new System.EventHandler(this.timerAI_Tick);
            // 
            // button5Ship
            // 
            this.button5Ship.Location = new System.Drawing.Point(54, 702);
            this.button5Ship.Name = "button5Ship";
            this.button5Ship.Size = new System.Drawing.Size(75, 23);
            this.button5Ship.TabIndex = 2;
            this.button5Ship.Text = "5";
            this.button5Ship.UseVisualStyleBackColor = true;
            this.button5Ship.Click += new System.EventHandler(this.button5Ship_Click);
            this.button5Ship.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonNShip_MouseDown);
            // 
            // button4Ship
            // 
            this.button4Ship.Location = new System.Drawing.Point(145, 702);
            this.button4Ship.Name = "button4Ship";
            this.button4Ship.Size = new System.Drawing.Size(75, 23);
            this.button4Ship.TabIndex = 2;
            this.button4Ship.Text = "4";
            this.button4Ship.UseVisualStyleBackColor = true;
            this.button4Ship.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonNShip_MouseDown);
            // 
            // button3Ship1
            // 
            this.button3Ship1.Location = new System.Drawing.Point(236, 702);
            this.button3Ship1.Name = "button3Ship1";
            this.button3Ship1.Size = new System.Drawing.Size(75, 23);
            this.button3Ship1.TabIndex = 2;
            this.button3Ship1.Text = "3";
            this.button3Ship1.UseVisualStyleBackColor = true;
            this.button3Ship1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonNShip_MouseDown);
            // 
            // button3Ship2
            // 
            this.button3Ship2.Location = new System.Drawing.Point(327, 702);
            this.button3Ship2.Name = "button3Ship2";
            this.button3Ship2.Size = new System.Drawing.Size(75, 23);
            this.button3Ship2.TabIndex = 2;
            this.button3Ship2.Text = "3";
            this.button3Ship2.UseVisualStyleBackColor = true;
            this.button3Ship2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonNShip_MouseDown);
            // 
            // button2Ship
            // 
            this.button2Ship.Location = new System.Drawing.Point(419, 702);
            this.button2Ship.Name = "button2Ship";
            this.button2Ship.Size = new System.Drawing.Size(75, 23);
            this.button2Ship.TabIndex = 2;
            this.button2Ship.Text = "2";
            this.button2Ship.UseVisualStyleBackColor = true;
            this.button2Ship.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonNShip_MouseDown);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(1123, 702);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(1204, 702);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 4;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // FormBattleShip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1301, 759);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.button2Ship);
            this.Controls.Add(this.button3Ship2);
            this.Controls.Add(this.button3Ship1);
            this.Controls.Add(this.button4Ship);
            this.Controls.Add(this.button5Ship);
            this.Controls.Add(this.labelFire);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridAI);
            this.Controls.Add(this.gridUser);
            this.Name = "FormBattleShip";
            this.Text = "Battleship";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel gridUser;
    private System.Windows.Forms.TableLayoutPanel gridAI;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label labelFire;
    private System.Windows.Forms.Timer timerAI;
    private System.Windows.Forms.Button button5Ship;
    private System.Windows.Forms.Button button4Ship;
    private System.Windows.Forms.Button button3Ship1;
    private System.Windows.Forms.Button button3Ship2;
    private System.Windows.Forms.Button button2Ship;
    private System.Windows.Forms.Button buttonReset;
    private System.Windows.Forms.Button buttonExit;
  }
}

