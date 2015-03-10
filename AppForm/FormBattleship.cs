using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MBC.Core;
using MBC.Core.Controllers;
using MBC.Core.Game;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Entities;
using MBC.Shared.Events;

using MBC.App.FormBattleship.Controls;

namespace MBC.App.FormBattleship
{

    [Configuration("bot", "RandomBot")]
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

        private Dictionary<Coordinates, CellButton> userCells = new Dictionary<Coordinates, CellButton>();
        private Dictionary<Coordinates, CellButton> computerCells = new Dictionary<Coordinates, CellButton>();
        private Button[] shipButtons = new Button[5];

        private Random rand = new Random();

        private int currentShipLength;
        private int currentRow;
        private int currentCol;
        private ShipOrientation currentDirection;

        private Configuration configuration;
        private ControllerSkeleton ComputerPlayer;

        public FormBattleShip()
        {
            InitializeComponent();

            shipButtons[0] = button5Ship;
            shipButtons[1] = button4Ship;
            shipButtons[2] = button3Ship1;
            shipButtons[3] = button3Ship2;
            shipButtons[4] = button2Ship;

            Configuration.Initialize(Environment.CurrentDirectory + "\\..");
            configuration = Configuration.Global;
             
            foreach (ControllerSkeleton bot in ControllerSkeleton.LoadControllerFolder(Environment.CurrentDirectory + "\\..\\bots"))
                if(bot.Controller.Name == configuration.GetValue("bot")) {
                    ComputerPlayer = bot; break;
                }

        }

        #region MBC
        public void MatchRun(MatchCore comp)
        {

        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int row = 0; row < NROWS; row++)
                for (int col = 0; col < NROWS; col++)
                {
                    var coord = new Coordinates(row, col);
                    var cell = new CellButton(coord);
                    cell.Name = "UI_" + row.ToString() + "_" + col.ToString();
                    cell.Click += new System.EventHandler(this.gridButtonUser_Click);
                    userCells.Add(coord,cell);
                    gridUser.Controls.Add(cell);
                }

            for (int row = 0; row < NROWS; row++)
                for (int col = 0; col < NROWS; col++)
                {
                    var coord = new Coordinates(row, col);
                    var cell = new CellButton(coord);
                    cell.Name = "UI_" + row.ToString() + "_" + col.ToString();

                    //cell.Click += new System.EventHandler(this.gridButton_Click);
                    cell.DragOver += new DragEventHandler(this.gridButton_DragOver);
                    cell.DragEnter += new DragEventHandler(this.gridButton_DragEnter);
                    cell.DragDrop += new DragEventHandler(this.gridButton_DragDrop);
                    cell.DragLeave += new EventHandler(this.gridButton_DragLeave);

                    cell.AllowDrop = true;
                    cell.Enabled = true;

                    computerCells.Add(coord, cell);
                    gridAI.Controls.Add(cell);
                }
            gridUser.Enabled = false;

        }

        // Reset the Board to the Starting State
        private void reset_Board()
        {
            foreach (var cell in computerCells)
            {
                cell.Value.AllowDrop = true;
                cell.Value.ShipState = State.Open;
            }
            foreach (var cell in userCells)
                cell.Value.ShipState = State.Open;
            
            for (int ship = 0; ship < shipButtons.Length; ship++)
                shipButtons[ship].Visible = true;
        }

        private void gridButton_Click(object sender, EventArgs e)
        {
            CellButton b = (CellButton)sender;
            if (b.BackColor == SHIPCOLOR)
            {
                b.ShipState = State.Hit;
            }
            else
            {
                b.ShipState = State.Miss;
            }
        }

        private void gridButtonUser_Click(object sender, EventArgs e)
        {
            CellButton b = (CellButton)sender;
            b.BackColor = Color.SkyBlue;
            b.ShipState = State.Miss; 
            timerAI.Enabled = true;
            gridUser.Enabled = false;
        }

        private void gridButton_DragOver(object sender, DragEventArgs e)
        {
            CellButton b = (CellButton)sender;
            int row = b.Coordinate.X;
            int col = b.Coordinate.Y;
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

            CellButton dropButton = (CellButton)sender;
            int row = dropButton.Coordinate.X;
            int col = dropButton.Coordinate.Y;
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
            ShipButton shipButton = (ShipButton)e.Data.GetData(DataFormats.Serializable);
            int shipSize = shipButton.ShipSize;

            CellButton dropButton = (CellButton)sender;
            int row = dropButton.Coordinate.X;
            int col = dropButton.Coordinate.Y;

            ShipOrientation orientation = shiftKey_Down(e.KeyState) ? ShipOrientation.Horizontal : ShipOrientation.Vertical;

            if (orientation == ShipOrientation.Horizontal)
            {
                if (col + shipSize <= GAMESIZE)
                {
                    e.Effect = DragDropEffects.Copy;
                    for (int i = col; i < col + shipSize; i++)
                        if (userCells[new Coordinates(row,i)].BackColor == SHIPCOLOR)
                        {
                            e.Effect = DragDropEffects.None;
                            break;
                        }
                }
                else
                    e.Effect = DragDropEffects.None;
                if (e.Effect == DragDropEffects.Copy)
                    buttonHighlight(row, col, orientation, shipSize, Color.DarkSlateGray, "", true);
            }
            else
            {
                if (row + shipSize <= GAMESIZE)
                {
                    e.Effect = DragDropEffects.Copy;
                    for (int i = row; i < row + shipSize; i++)
                        if (computerCells[new Coordinates(i,col)].BackColor == SHIPCOLOR)
                        {
                            e.Effect = DragDropEffects.None;
                            break;
                        }
                }
                else
                    e.Effect = DragDropEffects.None;
                if (e.Effect == DragDropEffects.Copy)
                    buttonHighlight(row, col, orientation, shipSize, Color.DarkSlateGray, "", true);
            }
            if (e.Effect == DragDropEffects.Copy)
            {

                currentShipLength = shipSize;
                currentRow = row;
                currentCol = col;
                currentDirection = orientation;
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
                    computerCells[new Coordinates(row, xpos)].BackColor = color;
                    if (text != "")
                        computerCells[new Coordinates(row, xpos)].Text = text;
                    computerCells[new Coordinates(row, xpos)].AllowDrop = enabled;
                }
            else
                for (int ypos = row; ypos < row + size; ypos++)
                {
                    computerCells[new Coordinates(ypos, col)].BackColor = color;
                    if (text != "")
                        computerCells[new Coordinates(ypos,col)].Text = text;
                    computerCells[new Coordinates(ypos,col)].AllowDrop = enabled;
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
            gridButton_Click(computerCells[new Coordinates(x,y)], null);
            timerAI.Enabled = false;
            
            // Re-enable ability for User to select a square
            gridUser.Enabled = true;
        }

        private void buttonNShip_Click(object sender, EventArgs e)
        {
            //((ShipButton)sender).toggleOrientation();
        }

    }
}
