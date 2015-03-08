using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MBC.Shared;

namespace MBC.App.FormBattleship
{


    public partial class FormBattleShip : Form
    {
        const int GAMESIZE = 10;
        const int NROWS = GAMESIZE;
        const int NCOLUMNS = GAMESIZE;

        Color SHIPCOLOR = Color.LightGray;
        Color DRAGCOLOR = Color.DarkSlateGray;
        Color DEFAULTCOLOR = Color.Black;
        Color HITCOLOR = Color.Red;
        Color MISSCOLOR = Color.SkyBlue;

        private Button[,] userCells = new Button[NROWS, NCOLUMNS];
        private Button[,] aiCells = new Button[NROWS, NCOLUMNS];
        private Button[] shipButtons = new Button[5];

        private Random rand = new Random();

        private int currentShipLength;
        private int currentRow;
        private int currentCol;
        private ShipOrientation currentDirection;

        public FormBattleShip()
        {
            InitializeComponent();
            shipButtons[0] = button5Ship;
            shipButtons[1] = button4Ship;
            shipButtons[2] = button3Ship1;
            shipButtons[3] = button3Ship2;
            shipButtons[4] = button2Ship;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int row = 0; row < NROWS; row++)
                for (int col = 0; col < NROWS; col++)
                {
                    userCells[row, col] = new Button();
                    userCells[row, col].Text = "OPEN";
                    userCells[row, col].Width = gridUser.Width / NCOLUMNS;
                    userCells[row, col].Height = gridUser.Height / NROWS;
                    userCells[row, col].Padding = new Padding(0);
                    userCells[row, col].Margin = new Padding(0);
                    userCells[row, col].BackColor = DEFAULTCOLOR;
                    userCells[row, col].ForeColor = Color.White;
                    userCells[row, col].Name = "UI_" + row.ToString() + "_" + col.ToString();
                    userCells[row, col].Click += new System.EventHandler(this.gridButtonUser_Click);
                    gridUser.Controls.Add(userCells[row, col]);
                }

            for (int row = 0; row < NROWS; row++)
                for (int col = 0; col < NROWS; col++)
                {
                    aiCells[row, col] = new Button();
                    aiCells[row, col].Text = "OPEN";
                    aiCells[row, col].Width = gridUser.Width / NCOLUMNS;
                    aiCells[row, col].Height = gridUser.Height / NROWS;
                    aiCells[row, col].Padding = new Padding(0);
                    aiCells[row, col].Margin = new Padding(0);
                    aiCells[row, col].BackColor = DEFAULTCOLOR;
                    aiCells[row, col].ForeColor = Color.White;
                    aiCells[row, col].Name = "AI_" + row.ToString() + "_" + col.ToString();
                    // aiCells[row, col].Click += new System.EventHandler(this.gridButton_Click);
                    aiCells[row, col].DragOver += new DragEventHandler(this.gridButton_DragOver);
                    aiCells[row, col].DragEnter += new DragEventHandler(this.gridButton_DragEnter);
                    aiCells[row, col].DragDrop += new DragEventHandler(this.gridButton_DragDrop);
                    aiCells[row, col].DragLeave += new EventHandler(this.gridButton_DragLeave);


                    aiCells[row, col].AllowDrop = true;
                    aiCells[row, col].Enabled = true;
                    gridAI.Controls.Add(aiCells[row, col]);
                }
            gridUser.Enabled = false;

        }

        // Reset the Board to the Starting State
        private void reset_Board()
        {
            for (int row = 0; row < NROWS; row++)
                for (int col = 0; col < NCOLUMNS; col++)
                {
                    aiCells[row, col].BackColor = DEFAULTCOLOR;
                    aiCells[row, col].AllowDrop = true;
                    aiCells[row, col].Text = "OPEN";
                }
            for (int row = 0; row < NROWS; row++)
                for (int col = 0; col < NCOLUMNS; col++)
                {
                    userCells[row, col].BackColor = DEFAULTCOLOR;
                    userCells[row, col].Text = "OPEN";
                }
            for (int ship = 0; ship < shipButtons.Length; ship++)
                shipButtons[ship].Visible = true;
        }

        private void gridButton_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.BackColor == SHIPCOLOR)
            {
                b.Text = "HIT";
                b.BackColor = Color.Tomato;
            }
            else
            {
                b.Text = "MISS";
                b.BackColor = Color.SkyBlue;
            }
        }

        private void gridButtonUser_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            b.BackColor = Color.SkyBlue;
            b.Text = "MISS";
            timerAI.Enabled = true;
            gridUser.Enabled = false;
        }

        private void gridButton_DragOver(object sender, DragEventArgs e)
        {
            Button b = (Button)sender;
            int row = int.Parse(b.Name.Substring(3, 1));
            int col = int.Parse(b.Name.Substring(5, 1));
            gridButton_DragEnter(sender, e);
            labelFire.Text = "Dragging on # " + row + "," + col + " KeyState = " + e.KeyState;
        }

        // Terminate the Drag operation 
        // If the Ship can fit in location - Draw it onto the board.

        void gridButton_DragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            Button shipButton = (Button)e.Data.GetData(DataFormats.Serializable);
            int shipSize = int.Parse(shipButton.Text);

            Button dropButton = (Button)sender;
            int row = int.Parse(dropButton.Name.Substring(3, 1));
            int col = int.Parse(dropButton.Name.Substring(5, 1));
            var placementDirection = shiftKey_Down(e.KeyState) ? ShipOrientation.Horizontal : ShipOrientation.Vertical;
            if (placementDirection == ShipOrientation.Horizontal)
            {
                if (col + shipSize <= GAMESIZE)
                {
                    buttonHighlight(row, col, ShipOrientation.Horizontal, shipSize, SHIPCOLOR, shipSize.ToString(), false);
                    shipButton.Visible = false;
                }
            }
            else
            {
                if (row + shipSize <= GAMESIZE)
                {
                    buttonHighlight(row, col, ShipOrientation.Vertical, shipSize, SHIPCOLOR, shipSize.ToString(), false);
                    shipButton.Visible = false;
                }
            }

            gridUser.Enabled = true;
            for (int i = 0; i < shipButtons.Length; i++)
                if (shipButtons[i].Visible == true)
                    gridUser.Enabled = false;
            
            // No ship is now selected 
            currentShipLength = 0;
        }

        // If we leave a grid location during drag we must erase the ship
        void gridButton_DragLeave(object sender, EventArgs e)
        {
            if (currentShipLength > 0)
            {
                buttonHighlight(currentRow, currentCol, currentDirection, currentShipLength, DEFAULTCOLOR, "", true);
            }
        }

        // Highlight the Ship possibility if it will fir in this location
        void gridButton_DragEnter(object sender, DragEventArgs e)
        {
            Button shipButton = (Button)e.Data.GetData(DataFormats.Serializable);
            int shipSize = int.Parse(shipButton.Text);

            Button dropButton = (Button)sender;
            int row = int.Parse(dropButton.Name.Substring(3, 1));
            int col = int.Parse(dropButton.Name.Substring(5, 1));
            ShipOrientation placementDirection = shiftKey_Down(e.KeyState) ? ShipOrientation.Horizontal : ShipOrientation.Vertical;
                gridButton_DragLeave(sender, e);
            if (placementDirection == ShipOrientation.Horizontal)
            {
                if (col + shipSize <= GAMESIZE)
                {
                    e.Effect = DragDropEffects.Copy;
                    for (int i = col; i < col + shipSize; i++)
                        if (aiCells[row, i].BackColor == SHIPCOLOR)
                        {
                            e.Effect = DragDropEffects.None;
                            break;
                        }
                }
                else
                    e.Effect = DragDropEffects.None;
                if (e.Effect == DragDropEffects.Copy)
                    buttonHighlight(row, col, placementDirection, shipSize, Color.DarkSlateGray, "", true);
            }
            else
            {
                if (row + shipSize <= GAMESIZE)
                {
                    e.Effect = DragDropEffects.Copy;
                    for (int i = row; i < row + shipSize; i++)
                        if (aiCells[i, col].BackColor == SHIPCOLOR)
                        {
                            e.Effect = DragDropEffects.None;
                            break;
                        }
                }
                else
                    e.Effect = DragDropEffects.None;
                if (e.Effect == DragDropEffects.Copy)
                    buttonHighlight(row, col, placementDirection, shipSize, Color.DarkSlateGray, "", true);
            }
            if (e.Effect == DragDropEffects.Copy)
            {

                currentShipLength = shipSize;
                currentRow = row;
                currentCol = col;
                currentDirection = placementDirection;
            }
            else
            {
                currentShipLength = 0;
            }

        }


        private void buttonNShip_MouseDown(object sender, MouseEventArgs e)
        {
            DataObject data = new DataObject(DataFormats.Serializable, sender as Button);
            DoDragDrop(data, DragDropEffects.Copy);
        }

        // Used in Dragging / Dropping Ships onto the Board
        private void buttonHighlight(int row, int col, ShipOrientation direction, int size, Color color, String text, bool enabled)
        {
            if (direction == ShipOrientation.Horizontal)
                for (int xpos = col; xpos < col + size; xpos++)
                {
                    aiCells[row, xpos].BackColor = color;
                    if (text != "")
                        aiCells[row, xpos].Text = text;
                    aiCells[row, xpos].AllowDrop = enabled;
                }
            else
                for (int ypos = row; ypos < row + size; ypos++)
                {
                    aiCells[ypos, col].BackColor = color;
                    if (text != "")
                        aiCells[ypos, col].Text = text;
                    aiCells[ypos, col].AllowDrop = enabled;
                }
        }

        // Detremines if Shift Key is down
        private Boolean shiftKey_Down(int KeyState)
        {
            return (KeyState & 4) == 4;
        }

        // User select the Reset / restyart a Game
        private void buttonReset_Click(object sender, EventArgs e)
        {
            reset_Board();
        }

        // Exit the Game
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Use as part of a simulation delay
        private void timerAI_Tick(object sender, EventArgs e)
        {
            // Fires Randomly
            int x = (int)(rand.NextDouble() * GAMESIZE);
            int y = (int)(rand.NextDouble() * GAMESIZE);
            
            //Simulate a Click
            gridButton_Click(aiCells[x, y], null);
            timerAI.Enabled = false;
            
            // Re-enable ability for User to select a square
            gridUser.Enabled = true;
        }

        private void button5Ship_Click(object sender, EventArgs e)
        {

        }
    }
}
