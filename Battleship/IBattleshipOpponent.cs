namespace Battleship
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Diagnostics;
    using System.Collections.Generic;

    public interface IBattleshipOpponent
    {
        public string Name
        {
            get;
        }

        public Version Version
        {
            get;
        }

        void NewMatch(string opponent);

        void NewGame(Size size, TimeSpan timeSpan);

        void PlaceShips(ReadOnlyCollection<Ship> ships);

        Point GetShot();

        void OpponentShot(Point shot);

        void ShotHit(Point shot, bool sunk);

        void ShotMiss(Point shot);

        void GameWon();

        void GameLost();

        void MatchOver();
    }
}
