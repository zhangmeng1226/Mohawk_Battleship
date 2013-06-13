using System;
using System.Collections.ObjectModel;
using MBC.Core;
using MBC.Core.Attributes;

namespace MBC.Controllers
{
    /// <summary>
    /// This is a controller that uses a pseudo-random number generator to make all of its decisions. This
    /// controller is highly documented and gives a good idea of how to develop a controller for use in MBC.
    /// 
    /// Every controller must implement the IBattleshipController interface from the shared framework in
    /// order to be detected by the MBC core. See <see cref="IBattleshipController"/> for information about
    /// when each of the methods are called during a competition.
    /// 
    /// Then, each controller must use at least three attributes to describe itself, which are the NameAttribute
    /// VersionAttribute, and CapabilitiesAttribute. They are simple to use; look at the attributes set to the RandomBot below to
    /// get an idea of how to set attributes. You can see which attributes are available by looking in the
    /// "Controller Plugin" project and opening the "Attributes" folder. Note that you do not need to
    /// type out the word "Attribute" after the attribute you wish to use.
    /// 
    /// Note that each controller has a time limit before they lose the round, defined in the <see cref="MatchInfo"/>
    /// that is given at the beginning of a match. There is even a second time limit that aborts the call
    /// to a controller if they take much longer.
    /// </summary>
    /// 

    //Here, a NameAttribute is defined, stating the name of this controller.
    [Name("RandomBot")]

    //Here, a VersionAttribute is defined, stating the version information for this controller.
    [Version(1, 0)]

    //Here, a CapabilitiesAttribute is defined, which defines the game modes that this controller is
    //able to participate in. See the GameMode.cs for information.
    [Capabilities(GameMode.Classic, GameMode.Salvo, GameMode.Multi)]

    //Here, a DescriptionAttribute is defined, giving a short description of this controller about what it does
    //or how it works. This attribute is optional and can be omitted.
    [Description("A controller that uses a random number generator to make all of its decisions.")]

    //Here, an AuthorAttribute is defined, which provides information about the author of this controller.
    //This attribute is optional and can be omitted.
    [Author(FirstName = "Ryan", LastName = "Albert", AKAName = "piknik",
        Biography = "I assisted in the development of the framework =]"
        )]

    //Here, an AcademicInfoAttribute is defined, which provides information about the educational institution
    //and program that the developer of this controller is in. This attribute is optional and can be omitted.
    [AcademicInfo("Mohawk College", "Software Development", 1)]
    public class RandomBot : IBattleshipController
    {
        /// <summary>
        /// This is a Random object that this controller will be using through each match to generate
        /// random numbers.
        /// </summary>
        Random rand;

        /// <summary>
        /// This is the ControllerID that was given to this controller for a match.
        /// </summary>
        ControllerID myID;

        /// <summary>
        /// This is the match information that was given to this controller that describes the match parameters.
        /// </summary>
        MatchInfo matchInfo;

        /// <summary>
        /// This is a list of shots that this controller has against another controller or controllers.
        /// It will start out being filled with every possible shot made.
        /// </summary>
        ShotList shotQueue;

        /// <summary>
        /// The ControllerMessageEvent event must be implemented by this controller, as defined in
        /// the IBattleshipController interface. It provides this controller a way to output information
        /// in the form of strings. It is called within this controller just like any other method. There
        /// is no need to check if the event is null because the competition will attach itself to this method
        /// immediately.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        /// <summary>
        /// This method will create a new ShotList in the shotQueue field of this controller. It will
        /// fill this ShotList with shots that this controller has available.
        /// </summary>
        private void SetShots()
        {
            //Construct a new ShotList object.
            shotQueue = new ShotList();

            //Start a loop from 0 to the number of players - 1. These numbers correspond to the
            //ControllerID numbers of each controller involved in a match. We want to add the
            //ControllerID numbers of every controller that is not this controller's to make
            //shots against.
            for (var ids = 0; ids < matchInfo.Players.Count; ids++)
            {

                //Check if an ID is equal to this controller's ID.
                if (ids != myID)
                {
                    //The ID is not this controller's so add it to the ShotList.
                    shotQueue.MakeReceiver(ids);
                }
            }

            //Initially, a ShotList is empty when it is constructed, so the ShotList can be filled
            //easily by inverting it up to the size of the field in the game.
            shotQueue.Invert(matchInfo.FieldSize);
        }

        /// <summary>
        /// This method is called from the competition whenever this controller is being involved in a new
        /// matchup against a controller, or controllers. This can be treated similarily like a constructor
        /// for this controller for simplicity's sake.
        /// </summary>
        /// <param name="thisId">The ControllerID that this controller is given.</param>
        /// <param name="matchInfo">The information about the matchup.</param>
        public void NewMatch(ControllerID thisId, MatchInfo matchInfo)
        {
            //The controller will begin by copying the information given by the match.
            this.matchInfo = matchInfo;
            this.myID = thisId;

            //The controller then calls the SetShots() method to initialize the shotQueue field.
            SetShots();

            //Finally, the controller creates a random number generator into the "rand" field defined
            //in this class. It uses the tick count of the system as a seed.
            rand = new Random(Environment.TickCount);
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
        /// This method generates a random set of coordinates within the match field boundaries.
        /// </summary>
        /// <returns>Coordinates of randomized X and Y components within this controller's match information
        /// field boundaries.</returns>
        private Coordinates RandomCoordinates()
        {
            //First generate a random X coordinate. Note that rand.Next() gets a random number that is
            //always less than the given value; we add one to get the full range of the field.
            var xCoord = rand.Next(matchInfo.FieldSize.X+1);

            //Then generate a random Y coordinate.
            var yCoord = rand.Next(matchInfo.FieldSize.Y+1);

            //Then put the two coordinates together and return it.
            return new Coordinates(xCoord, yCoord);
        }

        /// <summary>
        /// This method gets a random Shot that remains in the shotQueue.
        /// </summary>
        /// <returns>A random Shot from the shotQueue.</returns>
        private Shot NextRandomShot()
        {
            //First generate a random number which will be the index of a random
            //shot from within the shotQueue.
            var randomShotIndex = rand.Next(shotQueue.Count);

            //Then get the Shot object from the shotQueue at the random index.
            Shot randomShot = shotQueue[randomShotIndex];

            //According to the rules of most game modes, a controller cannot make the same shot
            //twice, so remove this shot from the shotQueue.
            shotQueue.Remove(randomShot);

            //Then return the random shot back to the caller.
            return randomShot;
        }

        /// <summary>
        /// This method is called when the controller is required to place ships. It is given a collection
        /// of Ship objects that can be accessed.
        /// </summary>
        /// <param name="ships">A collection of Ship objects to place.</param>
        public void PlaceShips(ReadOnlyCollection<Ship> ships)
        {
            //First encapsulate the ship collection into a ShipList, which is simpler to use
            //to manipulate Ship objects.
            var myShips = new ShipList(ships);

            //This loop will continue until all of the Ship objects have been placed.
            while (!myShips.ShipsPlaced)
            {
                //Get a random set of coordinates by calling the RandomCoordinates() method created earlier.
                var randomCoords = RandomCoordinates();

                //Get a random orientation for a ship to place by calling the RandomShipOrientation() method.
                var orientation = RandomShipOrientation();

                //Use the function within the ShipList object "myShips" to place a ship for the controller.
                //As explained in the PlaceShip() method of the ShipList, placing a ship at the randomly
                //generated coordinates may fail.
                myShips.PlaceShip(randomCoords, orientation);
            }

            //Since the controller had modified the Ship objects directly, there is no need to do anything
            //further.
        }

        /// <summary>
        /// This method is called when a shot is available to the controller. The Shot object is a reference
        /// to a copy held by the competition and is expected to be modified to the desired target. By default,
        /// the Shot receiver is the next controller in the turn.
        /// </summary>
        /// <param name="shot">The Shot to modify.</param>
        public void MakeShot(Shot shot)
        {
            //The controller only cares about modifying the Coordinates. The Coordinates of the next random
            //shot from the NextRandomShot() method is provided.
            shot.Coordinates = NextRandomShot().Coordinates;
        }

        /// <summary>
        /// This method is called when this controller won the round.
        /// </summary>
        public void RoundWon()
        {
            //Demonstrating the use of the ControllerMessageEvent by sending a string.
            ControllerMessageEvent("Yay, I won! What are the chances of that?");
        }

        /// <summary>
        /// This method is called when the controller lost the round.
        /// </summary>
        public void RoundLost()
        {
            //Demonstrating the use of the ControllerMessageEvent by sending a string.
            ControllerMessageEvent("Unsurprisingly I lost...");
        }

        //The rest of these methods are not used by this controller.
        public void NewRound() { }
        public void OpponentShot(Shot shot) { }
        public void ShotHit(Shot shot, bool sunk) { }
        public void ShotMiss(Shot shot) { }
        public void MatchOver() { }
        public void OpponentDestroyed(ControllerID destroyedID) { }
    }
}
