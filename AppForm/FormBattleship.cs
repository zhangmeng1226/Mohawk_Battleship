using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
using Ship = MBC.Shared.Entities.Ship;

namespace MBC.App.FormBattleship
{

    public partial class FormBattleShip : Form
    {
        private int compScore = 0;
        private int userScore = 0;

        const string NAME = "User";
        const string BOTNAME = "RandomBot";

        private ShipButton[] shipButtons = new ShipButton[5];

        private Random rand = new Random();

        private int currentShipLength;
        private int currentRow;
        private int currentCol;
        private ShipOrientation currentDirection;

        private Configuration configuration;
        private UserMatch match;
        private Player user;
        private Player computer;

        private Coordinates lastHit;

        public FormBattleShip()
        {
            Configuration.Initialize(Environment.CurrentDirectory + "\\..");

            configuration = Configuration.Global;
            configuration.SetValue("mbc_match_rounds", "1");

            InitializeComponent();

            shipButtons[0] = button5Ship;
            shipButtons[0].ShipPiece = ShipPiece.Carrier;
            shipButtons[1] = button4Ship;
            shipButtons[1].ShipPiece = ShipPiece.Battleship;
            shipButtons[2] = button3Ship1;
            shipButtons[2].ShipPiece = ShipPiece.Cruiser;
            shipButtons[3] = button3Ship2;
            shipButtons[3].ShipPiece = ShipPiece.Sub;
            shipButtons[4] = button2Ship;
            shipButtons[4].ShipPiece = ShipPiece.Destroyer;

            showStats();

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

            var bots = ControllerSkeleton.LoadControllerFolder(Environment.CurrentDirectory + "\\..\\bots");
            foreach (ControllerSkeleton bot in bots)
                if (bot.Controller.Name == BOTNAME)
                {
                    computer = match.PlayerCreate(bot);
                    break;
                }

            match.OnEvent += UpdateShot;
            match.OnEvent += UpdateShotHit;
            match.OnEvent += UpdateWinner;
            match.OnEvent += RoundEnd;
            match.OnEvent += UpdateShipDestroyed;

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
            if (playerShot.Player == user)
            {
                gridUser.SetCellState(playerShot.Shot.Coordinates, CellState.Miss);
                gridUser.Invalidate();
            }
            else
            {
                gridAI.SetCellState(playerShot.Shot.Coordinates, CellState.Miss);
                gridAI.Invalidate();
            }
        }
        
        [EventFilter(typeof(ShipDestroyedEvent))]
        public void UpdateShipDestroyed(Event ev)
        {
            ShipDestroyedEvent des = (ShipDestroyedEvent) ev;

            if (des.Ship.Owner == user)
            {
                gridAI.SetCellState(lastHit, CellState.Sunk);
            }
            else
            {
                gridUser.SetCellState(lastHit, CellState.Sunk);   
            }
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
            {
                gridAI.SetCellState(shipHit.HitCoords, CellState.Hit);
                gridAI.Invalidate();
            }
            else
            {
                gridUser.SetCellState(shipHit.HitCoords, CellState.Hit);
                gridUser.Invalidate();
            }

            lastHit = shipHit.HitCoords;
        }

        /// <summary>
        /// Message box will appear with who won the game.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(PlayerWonEvent))]
        public void UpdateWinner(Event ev)
        {
            var playerWon = (PlayerWonEvent)ev;
            MessageBox.Show("The winner is " + playerWon.Player.Name + " in " + playerWon.Player.ShotsMade.Count + " shots!");
            if (playerWon.Player == user)
            {
                userScore++;
            }
            else
            {
                compScore++;
            }
            showStats();
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
            //            gridAI.Click += GridAIClick;
            gridAI.AllowDrop = true;
            gridAI.DragOver += GridAIDrag;
            gridAI.DragEnter += GridAIDrag;
            gridAI.DragDrop += GridAIDragDrop;
            gridAI.DragLeave += GridAIDragLeave;

            gridUser.MouseUp += GridUserClick;
            gridUser.MouseMove += GridUserMove;
            gridUser.MouseLeave += GridUserMouseLeave;

            gridUser.Enabled = false;
        }

        // Reset the Board to the Starting State
        private void reset_Board()
        {
            gridAI.Cells.Clear();
            gridUser.Cells.Clear();
            gridAI.Invalidate();
            gridUser.Invalidate();

            for (int ship = 0; ship < shipButtons.Length; ship++)
            {
                shipButtons[ship].Visible = true;
            }

            NewMatch();
        }

        private void GridUserClick(object sender, MouseEventArgs e)
        {
            if (gridUser.Enabled)
            {
                Coordinates coordinates = gridUser.GetCoordinatesFromMouse(e.X, e.Y);
                Cell clickedCell;
                if (!gridUser.Cells.TryGetValue(coordinates, out clickedCell) || clickedCell.State == CellState.Open)
                {
                    match.UserShoot(new Shot(computer, coordinates));
                    timerAI.Enabled = true;
                    gridUser.Enabled = false;
                }
            }
        }

        private void GridUserMove(object sender, MouseEventArgs e)
        {
            if (gridUser.Enabled)
            {
                if (gridUser.CurrentHover == null)
                {
                    gridUser.CurrentHover = new FieldHover();
                    gridUser.CurrentHover.HoverSize = 1;
                    gridUser.CurrentHover.ShipPiece = ShipPiece.None;
                }
                gridUser.CurrentHover.Coordinates = gridUser.GetCoordinatesFromMouse(e.X, e.Y);
                gridUser.Invalidate();
            }
        }

        private void GridUserMouseLeave(object sender, EventArgs e)
        {
            gridUser.CurrentHover = null;
            gridUser.Invalidate();
        }

        private Ship SelectShip(int size)
        {
            foreach (Ship ship in user.Ships)
            {
                if (!ship.IsPlaced && ship.Length == size)
                {
                    return ship;
                }
            }
            return null;
        }

        private void AttemptPlayMatch()
        {
            gridUser.Enabled = true;
            for (int i = 0; i < shipButtons.Length; i++)
            {
                if (shipButtons[i].Visible == true)
                {
                    gridUser.Enabled = false;
                }
            }

            if (gridUser.Enabled)
            {
                match.Play(); // Sometimes Computer player goes first.
            }
        }

        private bool ShipCanFit(Coordinates destCoordinates, int size, int modX, int modY)
        {
            int maxX = destCoordinates.X + (size * modX);
            int maxY = destCoordinates.Y + (size * modY);
            if (maxX <= 10 && maxY <= 10)
            {
                for (int x = destCoordinates.X; x <= maxX; x++)
                {
                    for (int y = destCoordinates.Y; y <= maxY; y++)
                    {
                        Coordinates testCoordinates = new Coordinates(x, y);
                        Cell fieldCell;
                        if (gridAI.Cells.TryGetValue(testCoordinates, out fieldCell) && fieldCell.ShipPiece != ShipPiece.None)
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Places the dragged ship onto the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridAIDragDrop(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;

            ShipButton shipButton = (ShipButton)e.Data.GetData(DataFormats.Serializable);
            Ship selectedShip = SelectShip(shipButton.ShipSize);
            Point relativePoint = gridAI.PointToClient(new Point(e.X, e.Y));
            int shipSize = selectedShip.Length;

            Coordinates coordinates = gridAI.GetCoordinatesFromMouse(relativePoint.X, relativePoint.Y);
            ShipOrientation direction = shiftKey_Down(e.KeyState)
                ? ShipOrientation.Horizontal
                : ShipOrientation.Vertical;
            int modX = direction == ShipOrientation.Horizontal ? 1 : 0;
            int modY = direction == ShipOrientation.Vertical ? 1 : 0;

            int count = 0;
            for (int x = coordinates.X; x <= coordinates.X + ((shipSize - 1) * modX); x++)
            {
                for (int y = coordinates.Y; y <= coordinates.Y + ((shipSize - 1) * modY); y++)
                {
                    Coordinates coords = new Coordinates(x, y);
                    gridAI.SetCellShip(coords, shipButton.ShipPiece, count++, direction);
                }
            }
            selectedShip.Place(coordinates, direction);
            shipButton.Visible = false;
            gridAI.CurrentHover = null;
            gridAI.Invalidate();
            AttemptPlayMatch();
        }

        private void GridAIDragLeave(object sender, EventArgs e)
        {
            gridAI.CurrentHover = null;
            gridAI.Invalidate();
        }

        private void GridAIDrag(object sender, DragEventArgs e)
        {
            ShipButton shipButton = (ShipButton)e.Data.GetData(DataFormats.Serializable);
            int shipSize = shipButton.ShipSize;

            Point relativePoint = gridAI.PointToClient(new Point(e.X, e.Y));
            Coordinates coordinates = gridAI.GetCoordinatesFromMouse(relativePoint.X, relativePoint.Y);
            ShipOrientation orientation = shiftKey_Down(e.KeyState) ? ShipOrientation.Horizontal : ShipOrientation.Vertical;
            int modX = orientation == ShipOrientation.Horizontal ? 1 : 0;
            int modY = orientation == ShipOrientation.Vertical ? 1 : 0;

            if (gridAI.CurrentHover == null)
            {
                gridAI.CurrentHover = new FieldHover();
                gridAI.CurrentHover.HoverSize = shipSize;
                gridAI.CurrentHover.ShipPiece = shipButton.ShipPiece;
            }
            gridAI.CurrentHover.MouseX = relativePoint.X;
            gridAI.CurrentHover.MouseY = relativePoint.Y;
            gridAI.CurrentHover.Orientation = orientation;
            gridAI.CurrentHover.Coordinates = coordinates;

            if (ShipCanFit(coordinates, shipSize, modX, modY))
            {
                e.Effect = DragDropEffects.Copy;
                gridAI.CurrentHover.HoverSize = shipSize;
            }
            else
            {
                e.Effect = DragDropEffects.None;
                gridAI.CurrentHover.HoverSize = 0;
            }
            gridAI.Invalidate();
        }

        private void buttonNShip_MouseDown(object sender, MouseEventArgs e)
        {
            DataObject data = new DataObject(DataFormats.Serializable, sender as Button);
            DoDragDrop(data, DragDropEffects.Copy);
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

            gridUser.Enabled = true;
            timerAI.Enabled = false;
        }

        /// <summary>
        /// Show the score between user and computer in labelfire.
        /// </summary>
        private void showStats()
        {
            labelFire.Text = "User wins: " + userScore + " Computer wins: " + compScore;
        }
    }
}
