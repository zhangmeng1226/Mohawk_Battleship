using MBC.Shared;
using MBC.Shared.Attributes;
using System;

namespace MBC.Controllers
{
    /// <summary>
    /// This is a controller that uses a pseudo-random number generator to make all of its decisions. This
    /// controller is highly documented and gives a good idea of how to develop a controller for use in MBC.
    ///
    /// Every controller must implement the IBattleshipController interface from the shared framework in
    /// order to be detected by the MBC core.
    ///
    /// Then, each controller must use at least three attributes to describe itself, which are the NameAttribute
    /// VersionAttribute, and CapabilitiesAttribute. They are simple to use; look at the attributes set to the
    /// RandomBot below to get an idea of how to set attributes. Note that you do not need to
    /// type out the word "Attribute" after the attribute you wish to use.
    ///
    /// Note that each controller has a time limit before they lose the round, by default, this time limit is 200
    /// milliseconds that is given at the beginning of a match. There is even a second time limit that aborts the call
    /// to a controller if they take much longer.
    /// </summary>
    ///

    [Name("RandomBot C# Old")]
    [Version(1, 0)]
    [Capabilities(GameMode.Classic, GameMode.Salvo, GameMode.Multi)]
    [Description("A controller that uses a random number generator to make all of its decisions.")]
    [Author(FirstName = "Ryan", LastName = "Albert", AKAName = "piknik",
        Biography = "I assisted in the development of the framework =]"
        )]
    [AcademicInfo("Mohawk College", "Software Development", 2)]
    [LoadNot]
    public class RandomBot_old : Controller
    {
        private Random rand;

        private ShotList shotQueue;

        /// <summary>
        /// This method is called when a shot is available to the controller. The Shot object is a reference
        /// to a copy held by the competition and is expected to be modified to the desired target. By default,
        /// the Shot receiver is the next controller in the turn.
        /// </summary>
        /// <param name="shot">The Shot to modify.</param>
        public override Shot MakeShot()
        {
            return NextRandomShot();
        }

        /// <summary>
        ///  This method is called when the match has completed. You can use this method in order to export
        ///  debugging information or any reporting information that your bot has been tracking across all
        ///  games here.
        /// </summary>
        public override void MatchOver()
        {
            base.MatchOver();
        }

        /// <summary>
        /// This method is called when a new match is starting. Note this is not what's called at the start
        /// of a round which is an individual game of battleship, a match consists of many rounds, by default
        /// 1,000.
        ///
        /// You can use this method to do some initialization routines for your bot that only need to happen at
        /// the start of a match.
        /// </summary>
        public override void NewMatch()
        {
            base.NewMatch();
        }

        /// <summary>
        /// This method is called from the competition whenever this controller is being involved in a new
        /// match-up against a controller, or controllers. This can be treated similarly like a constructor
        /// for this controller for simplicity's sake.
        /// </summary>
        /// <param name="thisId">The ControllerID that this controller is given.</param>
        /// <param name="matchInfo">The information about the match-up.</param>
        public override void NewRound()
        {
            SetShots();

            rand = Match.Random;
        }

        /// <summary>
        /// This method is called when an opponent controller has had all their ships destroyed, and is no longer
        /// active in the game.
        /// </summary>
        /// <param name="destroyedID"></param>
        public override void OpponentDestroyed(IDNumber destroyedID)
        {
            base.OpponentDestroyed(destroyedID);
        }

        /// <summary>
        /// This method is called each time an opponent controller fires a shot. You can use this method
        /// to record the shots your opponent is making against your bot for your own analysis. This information
        /// is particularly useful for ship placement strategies.
        /// </summary>
        /// <param name="shot"></param>
        public override void OpponentShot(Shot shot)
        {
            base.OpponentShot(shot);
        }

        /// <summary>
        /// This method is called when the controller is required to place ships. It is given a collection
        /// of Ship objects that can be accessed.
        /// </summary>
        /// <param name="ships">A collection of Ship objects to place.</param>
        public override ShipList PlaceShips()
        {
            var myShips = Match.StartingShips;

            while (!myShips.ShipsPlaced)
            {
                var randomCoords = RandomCoordinates();

                var orientation = RandomShipOrientation();

                myShips.PlaceShip(randomCoords, orientation, Match.FieldSize);
            }

            return myShips;
        }

        /// <summary>
        /// This method is called when the controller lost the round. You can use this method to output
        /// debugging information, or to store information about the game for future analysis.
        /// </summary>
        public override void RoundLost()
        {
            SendMessage("Unsurprisingly I lost...");
        }

        /// <summary>
        /// This method is called when this controller won the round. You can use this method to output
        /// debugging information, or to store information about the game for future analysis.
        /// </summary>
        public override void RoundWon()
        {
            SendMessage("Yay, I won! What are the chances of that?");
        }

        /// <summary>
        /// This method is called each time a shot made by your bot scores a hit against an enemy ship.
        /// This method is useful for updating your bots mapping information. NOTE: the sunk boolean will
        /// only let your bot know that a ship has been sunk, it does not indicate which ship has been sunk.
        /// </summary>
        /// <param name="shot">The coordinates of the shot that hit</param>
        /// <param name="sunk">Returns true if the shot caused an enemy ship to sink</param>
        public override void ShotHit(Shot shot, bool sunk)
        {
            base.ShotHit(shot, sunk);
        }

        /// <summary>
        /// This method is called each time a shot made by your bot misses. This information is useful for
        /// updating your bots mapping information.
        /// </summary>
        /// <param name="shot">The coordinates of the shot that missed.</param>
        public override void ShotMiss(Shot shot)
        {
            base.ShotMiss(shot);
        }

        /// <summary>
        /// This method gets a random Shot that remains in the shotQueue.
        /// </summary>
        /// <returns>A random Shot from the shotQueue.</returns>
        private Shot NextRandomShot()
        {
            var randomShotIndex = rand.Next(shotQueue.Count);

            Shot randomShot = shotQueue[randomShotIndex];

            shotQueue.Remove(randomShot);

            return randomShot;
        }

        /// <summary>
        /// This method generates a random set of coordinates within the match field boundaries.
        /// </summary>
        /// <returns>Coordinates of randomized X and Y components within this controller's match information
        /// field boundaries.</returns>
        private Coordinates RandomCoordinates()
        {
            var xCoord = rand.Next(Match.FieldSize.X);

            var yCoord = rand.Next(Match.FieldSize.Y);

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
        /// This method will create a new ShotList in the shotQueue field of this controller. It will
        /// fill this ShotList with shots that this controller has available.
        /// </summary>
        private void SetShots()
        {
            shotQueue = new ShotList();

            shotQueue.MakeReceivers(AllOpponents());

            shotQueue.Invert(Match.FieldSize);
        }
    }
}