using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Entities;
using MBC.Shared.Events;
using MBC.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Controllers
{
    /// <summary>
    /// An MBC battleship contender. There is no strategy behind the RandomBot besides randomly placing ships and randomly placing shots.
    /// </summary>
    [Name("RandomBot C#")]
    [Version(2, 0)]
    [Capabilities(GameMode.Classic, GameMode.Salvo, GameMode.Multi)]
    [Description("A controller that uses a random number generator to make all of its decisions.")]
    [Author(FirstName = "Ryan", LastName = "Albert", AKAName = "piknik",
        Biography = "I assisted in the development of the framework =]"
        )]
    [AcademicInfo("Mohawk College", "Software Development", 3)]
    public class RandomBot : Controller2
    {
        private Random rand;
        private List<Shot> shotsRemain;

        /// <summary>
        /// Creates the random generator and hooks required events.
        /// </summary>
        protected override void Initialize()
        {
            rand = new Random();
            Player.OnEvent += Shoot;
            Player.Match.OnEvent += PlaceShips;
            Player.Match.OnEvent += CreateShots;
        }

        /// <summary>
        /// At the start of a round (RoundBeginEvent), populate the list of shots to shoot at.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(RoundBeginEvent))]
        private void CreateShots(Event ev)
        {
            shotsRemain = new List<Shot>();
            foreach (Player opponent in Player.Match.Players)
            {
                if (opponent == Player) continue;
                QueueShotsPlayer(opponent);
            }
        }

        /// <summary>
        /// In the beginning of a round, place all of the ships assigned to the player we're controlling
        /// (they will all be reset).
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(RoundBeginEvent))]
        private void PlaceShips(Event ev)
        {
            while (!ShipList.AreShipsPlaced(Player.Ships))
            {
                ShipList.PlaceShip(Player.Ships, RandomCoordinates(), RandomShipOrientation());
            }
        }

        /// <summary>
        /// Populates the shotRemain list with all possible shots against a Player opponent
        /// </summary>
        /// <param name="opponent"></param>
        private void QueueShotsPlayer(Player opponent)
        {
            for (int x = 0; x < Player.Match.FieldSize.X; x++)
            {
                for (int y = 0; y < Player.Match.FieldSize.Y; y++)
                {
                    shotsRemain.Add(new Shot(opponent, new Coordinates(x, y)));
                }
            }
        }

        /// <summary>
        /// This method generates a random set of coordinates within the match field boundaries.
        /// </summary>
        /// <returns>Coordinates of randomized X and Y components within this controller's match information
        /// field boundaries.</returns>
        private Coordinates RandomCoordinates()
        {
            var xCoord = rand.Next(Player.Match.FieldSize.X);

            var yCoord = rand.Next(Player.Match.FieldSize.Y);

            return new Coordinates(xCoord, yCoord);
        }

        /// <summary>
        /// This method randomly returns one of two ShipOrientation enums.
        /// </summary>
        /// <returns>A randomly selected ShipOrientation.</returns>
        private ShipOrientation RandomShipOrientation()
        {
            var orientations = new ShipOrientation[] { ShipOrientation.Horizontal, ShipOrientation.Vertical };

            return orientations[rand.Next(2)];
        }

        /// <summary>
        /// At the beginning of our Player's turn (PlayerTurnBeginEvent), shoot at a random coordinate
        /// on our opponent's battlefield.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(PlayerTurnBeginEvent))]
        private void Shoot(Event ev)
        {
            int shotIdx = rand.Next(shotsRemain.Count);
            Shot randShot = shotsRemain[shotIdx];
            shotsRemain.RemoveAt(shotIdx);
            Player.Shoot(randShot);
        }
    }
}