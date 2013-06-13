using System;
using System.Collections.ObjectModel;
using MBC.Core;
using MBC.Core.Attributes;

namespace MBC.Controllers
{
    [Name("RandomBot")]
    [Version(1, 0)]
    [Description("A controller that uses a random number generator to make all of its decisions.")]
    [Capabilities(GameMode.Classic, GameMode.ClassicMulti)]
    public class RandomBot : IBattleshipController
    {
        Random rand;
        MatchInfo matchInfo;
        ShotList shotsMade;

        public void NewMatch(MatchInfo matchInfo)
        {
            this.matchInfo = matchInfo;
            this.shotsMade = new ShotList();

            rand = new Random(Environment.TickCount);
        }

        private ShipOrientation RandomShipOrientation()
        {
            var orientations = new ShipOrientation[] { ShipOrientation.Horizontal, ShipOrientation.Vertical };
            return orientations[rand.Next(2)];
        }

        private Coordinates RandomCoordinates()
        {
            var xCoord = rand.Next(matchInfo.FieldSize.X);
            var yCoord = rand.Next(matchInfo.FieldSize.Y);
            return new Coordinates(xCoord, yCoord);
        }

        public void PlaceShips(ReadOnlyCollection<Ship> ships)
        {
            var myShips = new ShipList(ships);

            while (!myShips.ShipsPlaced)
            {
                var randomCoords = RandomCoordinates();
                var orientation = RandomShipOrientation();

                myShips.PlaceShip(randomCoords, orientation);
            }
        }

        public void MakeShot(Shot shot)
        {
            shot.Coordinates = RandomCoordinates();
        }

        public void NewRound() { }
        public void OpponentShot(Shot shot) { }
        public void ShotHit(Shot shot, bool sunk) { }
        public void ShotMiss(Shot shot) { }
        public void RoundWon() { }
        public void RoundLost() { }
        public void MatchOver() { }
    }
}
