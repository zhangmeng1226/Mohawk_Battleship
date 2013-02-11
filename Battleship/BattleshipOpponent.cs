using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Collections.ObjectModel;

namespace Battleship
{
    /**
     * <summary>A BattleshipOpponent is an object that contains various data pertaining to
     * an IBattleshipOpponent class. BattleshipOpponent contains various methods
     * to aid in game logic.</summary>
     * 
     */
    public class BattleshipOpponent
    {
        public const int LOSE_MAGIC_NUMBER = -50; //Number used in Point objects to signify a loss (timed out).
        public int score; //The number of wins in a match
        public IBattleshipOpponent iOpponent; //The class implementing the IBattleshipOpponent interface playing the game.
        public List<Point> shots;
        private List<Ship> ships;
        private Stopwatch stopwatch; //Used to time each call to the iOpponent
        private TimeSpan timeAllowed; //Reference to the BattleshipCompetition's defined max time.
        private Size gameSize; //Reference to the BattleshipCompetition's defined game size.

        /**
         * <summary>Sets up this BattleshipOpponent</summary>
         * <param name="op">The object that implements the IBattleshipOpponent interface to play the game.</param>
         * <param name="size">A reference to the board size in the competition</param>
         * <param name="timeout">A reference to the maximum alloted time in the competition</param>
         * 
         */
        public BattleshipOpponent(IBattleshipOpponent op, ref Size size, ref TimeSpan timeout)
        {
            score = 0;
            stopwatch = new Stopwatch();
            shots = new List<Point>();
            timeAllowed = timeout; //Passed in as a reference, not a copy
            gameSize = size; //Passed in as a reference, not a copy
            iOpponent = op;
        }

        /**
         * <summary>Checks whether all of the ships have been placed in valid locations</summary>
         * <returns>True if all ships are placed, not conflicting each other, and in valid locations.
         * False if otherwise</returns>
         */
        public bool ShipsReady()
        {
            foreach(Ship s1 in ships) {

                if (!s1.IsPlaced || !s1.IsValid(gameSize))
                    return false;

                foreach (Ship s2 in ships)
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
            iOpponent.NewMatch(opponent);
        }

        /**
         * <summary>Determines if the stopwatch time passed the maximum time allowed.</summary>
         * <returns>True if the player ran out of time. False if they didn't</returns>
         */
        public bool RanOutOfTime()
        {
            if (stopwatch.Elapsed > timeAllowed)
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
            shots.Clear();
            stopwatch.Start();

            iOpponent.NewGame(gameSize, timeAllowed);

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
            ships = newShips;
            stopwatch.Start();
            iOpponent.PlaceShips(ships.AsReadOnly());
            stopwatch.Stop();
            return RanOutOfTime();
        }

        public Ship GetShipAtPoint(Point p)
        {
            foreach (Ship s in ships)
                if (s.IsAt(p))
                    return s;
            return null;
        }

        public bool IsAlive(List<Point> shots)
        {
            foreach (Ship s in ships)
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
        public Point ShootAt(BattleshipOpponent opponent)
        {
            stopwatch.Start();
            Point shot = iOpponent.GetShot();
            stopwatch.Stop();

            if (shot.X < 0)
                shot.X = 0;
            if (shot.Y < 0)
                shot.Y = 0;

            if (RanOutOfTime())
                return new Point(LOSE_MAGIC_NUMBER, LOSE_MAGIC_NUMBER);

            if (shots.Where(s => s.X == shot.X && s.Y == shot.Y).Any())
                return ShootAt(opponent);

            shots.Add(shot);
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
            iOpponent.OpponentShot(shot);
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
            iOpponent.ShotHit(shot, sunk);
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
            iOpponent.ShotMiss(shot);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        /**
         * <summary>Notifys the player that a game has been won, and increments this player's score.</summary>
         */
        public void GameWon()
        {
            score++;
            iOpponent.GameWon();
        }

        /**
         * <summary>Notifys the player that a game has been lost</summary>
         */
        public void GameLost()
        {
            iOpponent.GameLost();
        }

        /**
         * <summary>Notifys the player that a matchup is over</summary>
         */
        public void MatchOver()
        {
            iOpponent.MatchOver();
        }

        /**
         * <summary>Generates a string containing the name and version of the encapsulated IBattleshipOpponent</summary>
         */
        public string GetInfo()
        {
            return iOpponent.Name + " " + iOpponent.Version.ToString();
        }

    }
}
