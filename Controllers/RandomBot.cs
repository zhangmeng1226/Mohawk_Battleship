using System;
using System.Collections.ObjectModel;
using MBC.Core;
using MBC.Core.Attributes;

namespace MBC.Controllers
{
    [Name("RandomBot")]
    [Version(1, 0)]
    [Description("A controller that uses a random number generator to make all of its decisions.")]
    public class RandomBot : IBattleshipController
    {
        Random rand;
        Size gameSize;

        public void NewRound(Size size, TimeSpan timeSpan)
        {
            rand = new Random();
            this.gameSize = size;
        }

        public void PlaceShips(ReadOnlyCollection<Ship> ships)
        {
            foreach (Ship s in ships)
            {
                s.Place(
                    new Coordinates(
                        rand.Next(this.gameSize.Width),
                        rand.Next(this.gameSize.Height)),
                    (ShipOrientation)rand.Next(2));
            }
        }

        public Coordinates MakeShot()
        {
            return new Coordinates(
                rand.Next(this.gameSize.Width),
                rand.Next(this.gameSize.Height));
        }

        public void NewMatch(string opponent) { }
        public void OpponentShot(Coordinates shot) { }
        public void ShotHit(Coordinates shot, bool sunk) { }
        public void ShotMiss(Coordinates shot) { }
        public void RoundWon() { }
        public void RoundLost() { }
        public void MatchOver() { }
    }
}
