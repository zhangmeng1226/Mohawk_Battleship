using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Collections.ObjectModel;

namespace Battleship
{
    public class BattleshipOpponent
    {
        public static const int LOSE_MAGIC_NUMBER = -50;
        public int score;
        private Stopwatch stopwatch;
        public List<Ship> ships;
        public List<Point> shots;
        public IBattleshipOpponent iOpponent;
        private TimeSpan timeAllowed;
        private Size gameSize;

        public BattleshipOpponent(IBattleshipOpponent op, Size size, TimeSpan timeout)
        {
            score = 0;
            stopwatch = new Stopwatch();
            shots = new List<Point>();
            timeAllowed = timeout; //Passed in as a reference, not a copy
            gameSize = size; //Passed in as a reference, not a copy
            iOpponent = op;
        }

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

        public void NewMatch(string opponent)
        {
            iOpponent.NewMatch(opponent);
        }

        public bool RanOutOfTime()
        {
            if (stopwatch.Elapsed > timeAllowed)
                return true;
            return false;
        }

        public bool NewGame()
        {
            stopwatch.Reset();
            shots.Clear();
            stopwatch.Start();

            iOpponent.NewGame(gameSize, timeAllowed);

            stopwatch.Stop();
            return RanOutOfTime();
        }

        public bool PlaceShips(List<Ship> newShips)
        {
            ships = newShips;
            stopwatch.Start();
            iOpponent.PlaceShips(ships.AsReadOnly());
            stopwatch.Stop();
            return RanOutOfTime();
        }

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

        public bool OpponentShot(Point shot)
        {
            stopwatch.Start();
            iOpponent.OpponentShot(shot);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        public bool ShotHit(Point shot, bool sunk)
        {
            stopwatch.Start();
            iOpponent.ShotHit(shot, sunk);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        public bool ShotMiss(Point shot)
        {
            stopwatch.Start();
            iOpponent.ShotMiss(shot);
            stopwatch.Stop();
            return RanOutOfTime();
        }

        public void GameWon()
        {
            score++;
            iOpponent.GameWon();
        }

        public void GameLost()
        {
            iOpponent.GameLost();
        }

        public void MatchOver()
        {
            iOpponent.MatchOver();
        }

        public string GetInfo()
        {
            return iOpponent.Name + " " + iOpponent.Version.ToString();
        }

    }
}
