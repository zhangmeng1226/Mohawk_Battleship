﻿using MBC.Shared;
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
    ///     A simple bot which shoots randomly
    ///     Details of this bots logic:
    ///     1. This bot always places it's ships in the same locations
    ///     2. This bot shoots randomly           
    /// </summary>
    [Name("SimpleBot 2.1 - Random Fire")]
    [Version(2, 1)]
    [Capabilities(GameMode.Classic)]
    [Description("A simple bot which fires randomly")]
    [Author(FirstName = "David", LastName = "Miller")]
    [AcademicInfo("Mohawk College", "Software Development", 4)]
    public class SimpleBot_2_1_0 : Controller2 {
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
            this.newMatch();
        }

        /// <summary>
        ///      Handles the RoundBeginEvent, indicating a new round has begun
        ///      A round consists of a standard game of battleship between two bots
        /// </summary>
        /// <param name="ev">An object of type RoundBeginEvent</param>
        [EventFilter(typeof(RoundBeginEvent))]
        private void roundBeginEvent(Event ev) {
            this.newRound();
        }

        /// <summary>
        ///     Handles the PlayerTurnBeginEvent, indicating a player's turn has begun 
        /// </summary>
        /// <param name="ev">An object of type PlayerTurnBeginEvent</param>
        [EventFilter(typeof(PlayerTurnBeginEvent))]
        private void playerTurnBeginEvent(Event ev) {
            this.turnStart();
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
        }

        /// <summary>
        /// It is our turn, fire!
        /// </summary>
        private void turnStart() {
            this.shoot();
        }

        /*******************************************
         ********** Action Methods *****************
         *******************************************/

        private List<Coordinates> shots;

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
        /// Takes coordinates and shoots at them
        /// </summary>
        /// <param name="shot"></param>
        private void shoot() {
            int shotIndex = rand.Next(shots.Count);
            Coordinates shot = shots[shotIndex];

            Player.Shoot(shot.X, shot.Y);

            shots.RemoveAt(shotIndex);           
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

    }
}