namespace Battleship
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Diagnostics;
    using System.Collections.Generic;

    public abstract class IBattleshipOpponent
    {
        private Random rand;
        public abstract string Name
        {
            get;
        }

        public abstract Version Version
        {
            get;
        }

        protected Random GetRandomGenerator()
        {
            return rand;
        }

        public abstract void NewMatch(string opponent);

        public abstract void NewGame(Size size, TimeSpan timeSpan);

        public abstract void PlaceShips(ReadOnlyCollection<Ship> ships);

        public abstract Point GetShot();

        public abstract void OpponentShot(Point shot);

        public abstract void ShotHit(Point shot, bool sunk);

        public abstract void ShotMiss(Point shot);

        public abstract void GameWon();

        public abstract void GameLost();

        public abstract void MatchOver();
    }
}
