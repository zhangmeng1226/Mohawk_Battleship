namespace MBC.Core
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Diagnostics;
    using System.Collections.Generic;

    public interface IBattleshipController
    {
        string Name
        {
            get;
        }

        Version Version
        {
            get;
        }

        void NewMatch(string opponent);

        void NewGame(Size size, TimeSpan timeSpan, Random rand);

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
