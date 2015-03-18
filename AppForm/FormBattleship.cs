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
using System.Threading;
using System.Diagnostics;

namespace MBC.App.FormBattleship
{
    
    public partial class FormBattleShip : Form
    {
        private int ROW_SIZE = 10;
        private int COL_SIZE = 10;

        const string NAME = "User";
        const string BOTNAME = "RandomBot";

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
        private UserMatch match;
        private Player user;

        public FormBattleShip()
        {
            Configuration.Initialize(Environment.CurrentDirectory + "\\..");
           
            configuration = Configuration.Global;
            configuration.SetValue("mbc_match_rounds", "1");
            
            ROW_SIZE = configuration.GetValue<int>("mbc_field_height");
            COL_SIZE = configuration.GetValue<int>("mbc_field_width");

            InitializeComponent();

            shipButtons[0] = button5Ship;
            shipButtons[1] = button4Ship;
            shipButtons[2] = button3Ship1;
            shipButtons[3] = button3Ship2;
            shipButtons[4] = button2Ship;

            NewMatch();
        }

        #region MBC
        /// <summary>
        /// This creates a new match.
        /// </summary>
        public void NewMatch()
        {
            match = new UserMatch(configuration);
            user = new Player(match, NAME);
            match.AddUser(user);

            foreach (ControllerSkeleton bot in ControllerSkeleton.LoadControllerFolder(Environment.CurrentDirectory + "\\..\\bots"))
                if (bot.Controller.Name == BOTNAME)
                {
                    match.PlayerCreate(bot);
                    break;
                }

            match.OnEvent += UpdateShot;
            match.OnEvent += UpdateShotHit;
            match.OnEvent += UpdateWinner;
            match.OnEvent += RoundEnd;

            match.Play();
        }

        /// <summary>
        /// Player has taken a shot, update on board.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(PlayerShotEvent))]
        public void UpdateShot(Event ev)
        {
            var playerShot = (PlayerShotEvent)ev;
            Debug.WriteLine("updateShot" + playerShot.Player.Name + playerShot.Shot.Coordinates.ToString());
            if (playerShot.Player == user) 
                userCells[playerShot.Shot.Coordinates].ShipState = State.Miss;
            else
                computerCells[playerShot.Shot.Coordinates].ShipState = State.Miss;
        }

        /// <summary>
        /// When a ship is hit, update board to State.hit for the coordinate.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(ShipHitEvent))]
        public void UpdateShotHit(Event ev)
        {
            var shipHit = (ShipHitEvent)ev;
            if (shipHit.Ship.Owner == user)
                computerCells[shipHit.HitCoords].ShipState = State.Hit;
            else
                userCells[shipHit.HitCoords].ShipState = State.Hit;
        }

        /// <summary>
        /// Message box will appear with who won the game.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(PlayerWonEvent))]
        public void UpdateWinner(Event ev)
        {
            var playerWon = (PlayerWonEvent)ev;
            MessageBox.Show("The winner is " + playerWon.Player.Name + " in " + playerWon.Player.ShotsMade);
        }

        /// <summary>
        /// When the round ends it will restart the board.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(RoundEndEvent))]
        public void RoundEnd(Event ev)
        {
            reset_Board();
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int row = 0; row < ROW_SIZE; row++)
                for (int col = 0; col < COL_SIZE; col++)
                {
                    var coord = new Coordinates(row, col);
                    var cell = new CellButton(coord);
                    cell.Name = "UI_" + row.ToString() + "_" + col.ToString();
                    cell.Click += new System.EventHandler(this.gridButtonUser_Click);
                    userCells.Add(coord,cell);
                    gridUser.Controls.Add(cell);
                }

            for (int row = 0; row < ROW_SIZE; row++)
                for (int col = 0; col < COL_SIZE; col++)
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
            foreach (var compCell in computerCells)
            {
                compCell.Value.ShipState = State.Open;
                compCell.Value.AllowDrop = true;
            }
            foreach (var userCell in userCells)
            {
                userCell.Value.ShipState = State.Open;
                userCell.Value.Enabled = true;
            }
            
            for (int ship = 0; ship < shipButtons.Length; ship++)
                shipButtons[ship].Visible = true;

            NewMatch();
        }

        private void gridButton_Click(object sender, EventArgs e)
        {
        }

        private void gridButtonUser_Click(object sender, EventArgs e)
        {
            CellButton b = (CellButton)sender;

            if (b.ShipState == State.Open)
            {
                foreach (Player opponent in user.Match.Players)
                {
                    if (opponent == user) continue;
                    user.Shoot(new Shot(opponent, b.Coordinate));
                    b.Enabled = false;

                    timerAI.Enabled = true;
                    gridUser.Enabled = false;
                }
            }
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
            ShipButton shipButton = (ShipButton)e.Data.GetData(DataFormats.Serializable);
            int shipSize = shipButton.ShipSize;

            CellButton dropButton = (CellButton)sender;
            int row = dropButton.Coordinate.X;
            int col = dropButton.Coordinate.Y;
            var placementDirection = shiftKey_Down(e.KeyState) ? ShipOrientation.Horizontal : ShipOrientation.Vertical;
            if (placementDirection == ShipOrientation.Horizontal)
            {
                if (col + shipSize <= COL_SIZE)
                {
                    buttonHighlight(row, col, ShipOrientation.Horizontal, shipSize, SHIPCOLOR, shipSize.ToString(), false);
                    // Gets the ship from user's ship list and then places it.
                    foreach (Ship ship in match.User.Ships)
                    {
                        if (!ship.IsPlaced && ship.Length == shipSize)
                        {
                            // Places the ships!
                            ship.Place(new Coordinates(row, col), ShipOrientation.Horizontal);
                            break;
                        }
                    }
                    shipButton.Visible = false;
                }
            }
            else
            {
                if (row + shipSize <= ROW_SIZE)
                {
                    buttonHighlight(row, col, ShipOrientation.Vertical, shipSize, SHIPCOLOR, shipSize.ToString(), false);
                    // Gets the ship from user's ship list and then places it.

                    foreach (Ship ship in match.User.Ships)
                    {
                        if (!ship.IsPlaced && ship.Length == shipSize)
                        {
                            // Places the ships!
                            //ship.Place(new Coordinates(row, col), ShipOrientation.Vertical);
                            break;
                        }
                    }
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
                if (col + shipSize <= COL_SIZE)
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
                if (row + shipSize <= ROW_SIZE)
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
            // Computer will take a turn, it gets called when timerAI.enabled = true
            match.ComputerTurn();

            timerAI.Enabled = false;
        }
    }
}
