using System;
using MBC.Shared;
using MBC.Shared.Attributes;

namespace MBC.Controllers
{
    /// <summary>
    /// This controller places ships in random locations and shoots at random cells. This is an excellent place to start learning about creating a Battleship AI
    /// 
    /// Each controller must provide some information about itself. The required attributes are Name, Version and Capabilities.
    /// These attributes provide information about this controller to the framework.
    /// 
    /// </summary>
    ///

    // The NameAttribute defines the name of this controller.
    [Name("RandomBot C#")]

    // The VersionAttribute defines the version information for this controller. Keeping multiple versions of your bot is an excellent way to track improvements or regressions.
    [Version(1, 0)]

    // The CapabilitiesAttribute defines the game modes that this controller is able to participate in. Currently the default mode is Classic.
    [Capabilities(GameMode.Classic, GameMode.Salvo, GameMode.Multi)]

    // The DescriptionAttribute is a short description of this controller about what it does
    // or how it works. This attribute is optional and can be omitted.
    [Description("A controller that uses a random number generator to make all of its decisions.")]

    // The AuthorAttribute provides information about the author of this controller.
    // This attribute is optional and can be omitted.
    [Author(FirstName = "Ryan", LastName = "Albert", AKAName = "piknik",
        Biography = "I assisted in the development of the framework =]"
        )]

    // The AcademicInfoAttribute provides information about the author's educational institution and program.
    // This attribute is optional and can be omitted.
    [AcademicInfo("Mohawk College", "Software Development", 1)]

    /// <summary>
    ///  Extending the Controller class exposes essential methods and properties.
    ///  It also allows your bot to be detected by the MBC core and be used in matches.
    /// </summary>
    public class RandomBot : Controller
    {
        // This object is used to generate random numbers.
        private Random rand;

        // This is a list of shots that this controller has against another controller or controllers.
        private ShotList shotQueue;

        /// <summary>
        ///  This method is called when a shot is available to this controller.
        /// </summary>
        /// <returns>A Shot object</returns>
        public override Shot MakeShot()
        {
            //Return the result of NextRandomShot() method
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
        /// This method is called when a new match is starting. 
        /// A match is a series of games against an opponent.
        /// By default a match consists of 1,000 rounds.
        ///
        /// You can use this method to do some initialization routines for your bot that only need to happen at
        /// the start of a match.
        /// </summary>
        public override void NewMatch()
        {
            base.NewMatch();
        }

        /// <summary>
        /// This method is called from the competition whenever a new game of battleship begins. For simplicity's sake this method can be treated similarly to a constructor
        /// for this controller.
        /// </summary>
        /// <param name="thisId">The ControllerID that this controller is given.</param>
        /// <param name="matchInfo">The information about the match-up.</param>
        public override void NewRound()
        {
            //The controller calls the SetShots() method to initialize the shotQueue field.
            SetShots();

            //Finally, the controller creates a random number generator into the "rand" field defined
            //in this class. It uses the tick count of the system as a seed.
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
        /// This method is called when the controller is required to place ships.
        /// </summary>
        /// <returns name="ships">A collection of Ship objects</returns>
        public override ShipList PlaceShips()
        {
            // First we'll refer to the ships given to us through a single variable.
            var myShips = Match.StartingShips;

            // This loop will continue until all of the Ship objects have been placed.
            while (!myShips.ShipsPlaced)
            {
                // Get a random set of coordinates by calling the RandomCoordinates() method created earlier.
                var randomCoords = RandomCoordinates();

                // Get a random orientation for a ship to place by calling the RandomShipOrientation() method.
                var orientation = RandomShipOrientation();

                // Use the function within the ShipList object "myShips" to place a ship for the controller.
                // As explained in the PlaceShip() method of the ShipList, placing a ship at the randomly
                // generated coordinates may fail.
                myShips.PlaceShip(randomCoords, orientation, Match.FieldSize);
            }

            return myShips;
        }

        /// <summary>
        /// This method is called when this controller loses a round. This method can be used to output
        /// debugging information, or to store information about the game for future analysis.
        /// </summary>
        public override void RoundLost()
        {
            // Demonstrating the use of the ControllerMessageEvent by sending a string.
            SendMessage("Unsurprisingly I lost...");
        }

        /// <summary>
        /// This method is called when this controller wins a round. This can be used method to output
        /// debugging information, or to store information about the game for future analysis.
        /// </summary>
        public override void RoundWon()
        {
            //Demonstrating the use of the ControllerMessageEvent by sending a string.
            SendMessage("Yay, I won! What are the chances of that?");
        }

        /// <summary>
        /// This method is called each time a shot made by your bot scores a hit against an enemy ship.
        /// The parameters of this method can be useful for updating your bots mapping information.
        /// NOTE: This method does not indicate which ship has been sunk.
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
        /// This method gets a random Shot from the shotQueue.
        /// </summary>
        /// <returns>A random Shot from the shotQueue.</returns>
        private Shot NextRandomShot()
        {
            // First generate a random number which will be the index of a random
            // shot from within the shotQueue.
            var randomShotIndex = rand.Next(shotQueue.Count);

            // Then get the Shot object from the shotQueue at the random index.
            Shot randomShot = shotQueue[randomShotIndex];

            // According to the rules of most game modes, a controller cannot make the same shot
            // twice, so remove this shot from the shotQueue.
            shotQueue.Remove(randomShot);

            //Then return the random shot back to the caller.
            return randomShot;
        }

        /// <summary>
        /// This method generates a random set of coordinates within the match field boundaries.
        /// </summary>
        /// <returns>Coordinates of randomized X and Y components within this controller's match information
        /// field boundaries.</returns>
        private Coordinates RandomCoordinates()
        {
            // First generate a random X coordinate.
            var xCoord = rand.Next(Match.FieldSize.X);

            // Then generate a random Y coordinate.
            var yCoord = rand.Next(Match.FieldSize.Y);

            // Then put the two coordinates together and return it.
            return new Coordinates(xCoord, yCoord);
        }

        /// <summary>
        /// This method randomly returns one of two ShipOrientation enums.
        /// </summary>
        /// <returns>A randomly selected ShipOrientation.</returns>
        private ShipOrientation RandomShipOrientation()
        {
            //The controller first makes a two-element array that contains the two possible orientations.
            var orientations = new ShipOrientation[] { ShipOrientation.Horizontal, ShipOrientation.Vertical };

            //Then the controller uses the random number generator "rand" to choose either a 0 or a 1 to
            //pick the index of a ShipOrientation randomly. The ShipOrientation is then returned back to the
            //caller.
            return orientations[rand.Next(2)];
        }

        /// <summary>
        /// This method will create a new ShotList in the shotQueue field of this controller. It will
        /// fill this ShotList with shots that this controller has available.
        /// </summary>
        private void SetShots()
        {
            // Construct a new ShotList object.
            shotQueue = new ShotList();

            // Set up our shot queue with our opponents.
            shotQueue.MakeReceivers(AllOpponents());

            // Initially, a ShotList is empty when it is constructed, so calling the Invert method will fill it with new shots.
            shotQueue.Invert(Match.FieldSize);
        }
    }
}