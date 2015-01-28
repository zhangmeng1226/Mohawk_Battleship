using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Entities;
using MBC.Shared.Events;
using MBC.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Controllers {
    /// <summary>
    ///     A simple bot with random shooting plus basic hunting logic
    ///     Details of this bots logic:
    ///     1. This bot always places it's ships in the same locations
    ///     2. This bot begins by shooting randomly
    ///     3. When a hit occurs, this bot will cease shooting randomly and target all surrounding coordinates
    /// </summary>
    [Name("SimpleBot 3.0 - Hunting")]
    [Version(3, 0)]
    [Capabilities(GameMode.Classic)]
    [Description("A simple bot with random shooting plus basic hunting logic")]
    [Author(FirstName = "David", LastName = "Miller")]
    [AcademicInfo("Mohawk College", "Software Development", 4)]
    public class SimpleBot_3_0_0 : Controller2 {
        /// <summary>
        /// Hooks required events.
        /// </summary>
        protected override void Initialize() {
            // Match events are in the context of the match
            // and will not neccarrily be specific to your bot
            //
            // For example, hooking the PlayerShotEvent event 
            // in the match context will result in the assigned
            // method executing on all shots made, while hooking
            // the event on the Player context will result in it
            // only executing on your own shots.
            Player.Match.OnEvent += matchBeginEvent;
            Player.Match.OnEvent += roundBeginEvent;

            Player.Match.OnEvent += shipHitEvent;

            // Hooking methods to Player.OnEvent events means 
            // the method will only handle events specific to 
            // your bot.

            // We bind the playerTurnBeginEvent to 
            // Player.OnEvent to ensure we only act when it is 
            // our bot's turn.
            //
            // If we wanted to catch our opponent's turn
            // as well, we would use Player.Match.OnEvent
            Player.OnEvent += playerTurnBeginEvent;
        }

        /*******************************************
         ********* Framework event handlers ********
         *******************************************/

        /// <summary>
        ///     Handles the MatchBeginEvent, indicating a new match has begun
        ///     A match consists of a series of rounds between bots
        /// </summary>
        /// <param name="ev">An object of type MatchBeginEvent</param>
        [EventFilter(typeof(MatchBeginEvent))]
        private void matchBeginEvent(Event ev) {
            MatchBeginEvent matchBeginEvent = (MatchBeginEvent)ev;
            this.newMatch();
        }

        /// <summary>
        ///      Handles the RoundBeginEvent, indicating a new round has begun
        ///      A round consists of a standard game of battleship between two bots
        /// </summary>
        /// <param name="ev">An object of type RoundBeginEvent</param>
        [EventFilter(typeof(RoundBeginEvent))]
        private void roundBeginEvent(Event ev) {
            RoundBeginEvent matchBeginEvent = (RoundBeginEvent)ev;
            this.newRound();
        }

        /// <summary>
        ///     Handles the PlayerTurnBeginEvent, indicating a bots turn has begun 
        /// </summary>
        /// <param name="ev">An object of type PlayerTurnBeginEvent</param>
        [EventFilter(typeof(PlayerTurnBeginEvent))]
        private void playerTurnBeginEvent(Event ev) {
            PlayerTurnBeginEvent playerTurnBeginEvent = (PlayerTurnBeginEvent)ev;
            this.turnStart();
        }

        /// <summary>
        /// Handles the ShipHitEvent, indicating a ship has been hit
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(ShipHitEvent))]
        private void shipHitEvent(Event ev) {
            ShipHitEvent shipHitEvent = (ShipHitEvent)ev;
            Player owner = shipHitEvent.Ship.Owner; //who owns the hit ship

            // We only care about hits on enemy ships
            if (owner != Player) {
                // An opponent's ship has been hit
                this.hit(shipHitEvent.HitCoords, shipHitEvent.Ship.IsSunk());
            }
        }

        /*******************************************
         ********** Controller Methods *************
         *******************************************/
        // A set of lightweight methods, designed as an isolating layer between
        // the obscurities of the framework/event handling and the more complicated bot logic.
        // These should do very little (and everything), existing primarily to excution bot actions 
        // in a logical order at an appropriate time.

        private Random rand;

        private int matchX; // we'll store the horizontal size of the game board here
        private int matchY; // we'll store the vertical size of the game board here

        private List<Coordinates> shots; // A list of all available target coordinates, not including those on the hunting list
        private List<Coordinates> huntingShots; // A list of coordinates surrounding locations we have hit, to be priortized as targets

        /// <summary>
        /// A new match has begun
        /// </summary>
        private void newMatch() {
            rand = new Random();
            matchX = Player.Match.FieldSize.X;
            matchY = Player.Match.FieldSize.Y;
        }

        /// <summary>
        /// A new round has begun, place our ships and prepare to fire!
        /// </summary>
        private void newRound() {
            this.placeShips();
            this.populateShotList();
            this.initializeHuntingList();
        }

        /// <summary>
        /// It is our turn, fire!
        /// </summary>
        private void turnStart() {
            Coordinates nextShot = this.getNextShot();
            this.shoot(nextShot);
        }

        /// <summary>
        /// We have hit an enemy ship
        /// </summary>
        /// <param name="coords">Coordinates of the shot</param>
        /// <param name="sunk">Did the hit sink the ship?</param>
        private void hit(Coordinates coords, bool sunk) {
            // Lets take the coordinates surrounding our hit and add them to our hunting list
            List<Coordinates> surroundingShots = this.getSurroundingShots(coords);
            moveShotToHuntingList(surroundingShots);
        }

        /*******************************************
         ********** Action Methods *****************
         *******************************************/

        /// <summary>
        /// Takes coordinates and shoots at them
        /// </summary>
        /// <param name="shot"></param>
        private void shoot(Coordinates shot) {
            Player.Shoot(shot.X, shot.Y);

            huntingShots.Remove(shot);
            shots.Remove(shot);
        }

        /// <summary>
        ///     Place ships in specific locations
        /// </summary>
        private void placeShips() {
            // Smallest to largest            
            ShipList.PlaceShip(Player.Ships, new Coordinates(9, 8), ShipOrientation.Vertical); // 2 length
            ShipList.PlaceShip(Player.Ships, new Coordinates(0, 7), ShipOrientation.Horizontal); // 3 length
            ShipList.PlaceShip(Player.Ships, new Coordinates(7, 1), ShipOrientation.Vertical); // 3 length
            ShipList.PlaceShip(Player.Ships, new Coordinates(1, 9), ShipOrientation.Horizontal); // 4 length
            ShipList.PlaceShip(Player.Ships, new Coordinates(2, 4), ShipOrientation.Horizontal); // 5 length
        }

        /*******************************************
         ********** Helper Methods *****************
         *******************************************/

        /// <summary>
        /// Populate our list of shot coordinates
        /// </summary>
        private void populateShotList() {
            shots = new List<Coordinates>();

            for (int x = 0; x < matchX; x++) {
                for (int y = 0; y < matchY; y++) {
                    shots.Add(new Coordinates(x, y));
                }
            }
        }

        /// <summary>
        /// Create a fresh hunting list
        /// </summary>
        private void initializeHuntingList() {
            huntingShots = new List<Coordinates>();
        }

        /// <summary>
        /// Creates a list of Coordinates surrounding those provided
        /// Does not validate to ensure they are within the bounds of the grid
        /// </summary>
        /// <param name="shot"></param>
        /// <returns></returns>
        private List<Coordinates> getSurroundingShots(Coordinates shot) {
            List<Coordinates> surroundingShots = new List<Coordinates>();

            surroundingShots.Add(new Coordinates(shot.X + 1, shot.Y));
            surroundingShots.Add(new Coordinates(shot.X - 1, shot.Y));
            surroundingShots.Add(new Coordinates(shot.X, shot.Y + 1));
            surroundingShots.Add(new Coordinates(shot.X, shot.Y - 1));

            return surroundingShots;
        }

        /// <summary>
        /// Finds and removes a shot from the shot List and adds it to the hunting list
        /// </summary>
        private void moveShotToHuntingList(Coordinates shot) {
            if (shots.Remove(shot)) {
                huntingShots.Add(shot);
            }
        }

        /// <summary>
        /// Remove a list of shots from the shot list and add them to the hunting list
        /// </summary>
        /// <param name="shots"></param>
        private void moveShotToHuntingList(List<Coordinates> shots) {
            shots.ForEach(moveShotToHuntingList);
        }

        /// <summary>
        /// Get the next shot, from the hunting list if available, randomly if not
        /// </summary>
        /// <returns></returns>
        private Coordinates getNextShot() {
            if (huntingShots.Count > 0) {
                return this.getNextHuntingShot();
            }

            int shotIndex = rand.Next(shots.Count);
            return shots[shotIndex];
        }

        /// <summary>
        /// Take a shot from the hunting list
        /// </summary>
        /// <returns></returns>
        private Coordinates getNextHuntingShot() {
            return huntingShots.First();
        }
    }
}