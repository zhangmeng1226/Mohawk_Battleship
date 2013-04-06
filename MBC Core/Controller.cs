using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Collections.ObjectModel;

namespace MBC.Core
{
    /**
     * <summary>A BattleshipOpponent is an object that contains various data pertaining to
     * an IBattleshipOpponent class. BattleshipOpponent contains various methods
     * to aid in game logic.</summary>
     * 
     */
    public class Controller
    {
        public const int LOSE_MAGIC_NUMBER = -50; //Number used in Point objects to signify a loss (timed out).
        public IBattleshipController ibc; //The class implementing the IBattleshipOpponent interface playing the game.
        private Stopwatch stopwatch; //Used to time each call to the iOpponent
        private Field field;
        private Field.ControllerInfo info;

        /**
         * <summary>Sets up this BattleshipOpponent</summary>
         * <param name="ic">The object that implements the IBattleshipOpponent interface to play the game.</param>
         * <param name="size">A reference to the board size in the competition</param>
         * <param name="timeout">A reference to the maximum alloted time in the competition</param>
         * 
         */
        public Controller(IBattleshipController ic, Field f)
        {
            ibc = ic;
            field = f;

            info = field[ibc];
            if (info == null)
                throw new NullReferenceException(ibc.Name + " bot: Unable to get info.");

            info.score = 0;
            info.shotsMade = new List<Point>();
            stopwatch = new Stopwatch();
        }

        /**
         * <returns>The information related to this opponent on the battlefield</returns>
         */
        public Field.ControllerInfo GetFieldInfo()
        {
            return info;
        }

        /**
         * <returns>The time the last action took for this bot to perform.</returns>
         */
        public long GetTimeTaken()
        {
            return stopwatch.ElapsedMilliseconds;
        }

        /**
         * <summary>Checks whether all of the ships have been placed in valid locations</summary>
         * <returns>True if all ships are placed, not conflicting each other, and in valid locations.
         * False if otherwise</returns>
         */
        public bool ShipsReady()
        {
            foreach (Ship s1 in info.ships)
            {

                if (!s1.IsPlaced || !s1.IsValid(field.gameSize))
                    return false;

                foreach (Ship s2 in info.ships)
                {
                    if (s2 == s1) continue;
                    if (s2.ConflictsWith(s1)) return false;
                }
            }
            return true;
        }

        /**
         * <summary>Notifys the player that they are matched up with a new opponent.</summary>
         * <param name="opponent">The name of the opponent as a string</param>
         */
        public void NewMatch(string opponent)
        {
            ibc.NewMatch(opponent);
        }

        /**
         * <summary>Determines if the stopwatch time passed the maximum time allowed.</summary>
         * <returns>True if the player ran out of time. False if they didn't</returns>
         */
        public bool RanOutOfTime()
        {
            if (stopwatch.Elapsed > field.timeoutLimit)
                return true;
            return false;
        }

        /**
         * <summary>Resets certain data for a new game and notifys the player of the new game being commenced</summary>
         * <returns>True if the player ran out of time. False if they didn't</returns>
         */
        public bool NewGame()
        {
            stopwatch.Reset();
            info.shotsMade.Clear();
            stopwatch.Start();

            ibc.NewGame(field.gameSize, field.timeoutLimit, field.fixedRandom);

            stopwatch.Stop();
            return RanOutOfTime();
        }

        /**
         * <summary>Notifys the player to make ship placements.</summary>
         * <param name="newShips">A list of ships to place</param>
         * <returns>True if the player ran out of time. False if they didn't</returns>
         */
        public bool PlaceShips(List<Ship> newShips)
        {
            info.ships = newShips;
            stopwatch.Start();
            ibc.PlaceShips(info.ships.AsReadOnly());
            stopwatch.Stop();
            return RanOutOfTime();
        }

        /**
         * <returns>The ship at point p. Null if there is no ship.</returns>
         */
        public Ship GetShipAtPoint(Point p)
        {
            foreach (Ship s in info.ships)
                if (s.IsAt(p))
                    return s;
            return null;
        }

        /**
         * <returns>True if the opponent is still in the match, false if the opponent has lost</returns>
         */
        public bool IsAlive(List<Point> shots)
        {
            foreach (Ship s in info.ships)
                if (!s.IsSunk(shots))
                    return true;
            return false;
        }

        /**
         * <summary>Asks for the player's shot. ShootAt will repeatedly request the shot
         * until it hasn't made the same shot twice.</summary>
         * <param name="opponent">The BattleshipOpponent to "shoot at"</param>
         * <returns>The point the player has shot at. If the player ran out of time,
         * a point at (LOSE_MAGIC_NUMBER, LOSE_MAGIC_NUMBER) will be returned instead.</returns>
         */
        public Point ShootAt(Controller opponent)
        {
            stopwatch.Start();
            Point shot = ibc.GetShot();
            stopwatch.Stop();

            if (shot.X < 0)
                shot.X = 0;
            if (shot.Y < 0)
                shot.Y = 0;

            if (RanOutOfTime())
                return new Point(LOSE_MAGIC_NUMBER, LOSE_MAGIC_NUMBER);

            if (info.shotsMade.Where(s => s.X == shot.X && s.Y == shot.Y).Any())
                return ShootAt(opponent);

            info.shotsMade.Add(shot);
            return shot;
        }

        /**
         * <summary>Notifys the player that a shot has been made by the other opponent at
         * a certain Point</summary>
         * <param name="shot">The point where the other opponent shot</param>
         * <returns>True if the player ran out of time. False if they didn't</returns>
         */
        public bool OpponentShot(Point shot)
        {
            stopwatch.Start();
            ibc.OpponentShot(shot);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        /**
         * <summary>Notifys the player that their shot hit a ship at the specified point</summary>
         * <param name="shot">The point at which the players shot hit a ship</param>
         * <param name="sunk">If the shot sunk a ship</param>
         * <returns>True if the player ran out of time. False if they didn't</returns>
         */
        public bool ShotHit(Point shot, bool sunk)
        {
            stopwatch.Start();
            ibc.ShotHit(shot, sunk);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        /**
         * <summary>Notifys the player that their shot missed at the specified point</summary>
         * <param name="shot">The point the player shot at but missed</param>
         * <returns>True if the player ran out of time. False if they didn't</returns>
         */
        public bool ShotMiss(Point shot)
        {
            stopwatch.Start();
            ibc.ShotMiss(shot);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        /**
         * <summary>Notifys the player that a game has been won, and increments this player's score.</summary>
         */
        public void GameWon()
        {
            info.score++;
            ibc.GameWon();
        }

        /**
         * <summary>Notifys the player that a game has been lost</summary>
         */
        public void GameLost()
        {
            ibc.GameLost();
        }

        /**
         * <summary>Notifys the player that a matchup is over</summary>
         */
        public void MatchOver()
        {
            ibc.MatchOver();
        }

        /**
         * <summary>Generates a string containing the name and version of the encapsulated IBattleshipOpponent</summary>
         */
        public override string ToString()
        {
            return ibc.Name + " " + ibc.Version.ToString();
        }

    }
}
