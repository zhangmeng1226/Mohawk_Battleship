
using MBC.Shared.Attributes;
using MBC.Shared.Entities;
using MBC.Shared.Events;
using MBC.Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Controllers.DaveBot_1_4_5;
using MBC.Shared;

namespace MBC.Controllers {
    [Name("DaveBot 1_4_5 C#")]
    [Version(1, 4)]
    [Capabilities(GameMode.Classic)]
    [Description("Dave's Bot.")]
    [Author(FirstName = "David", LastName = "Miller")]
    [AcademicInfo("Mohawk College", "Software Development", 4)]
    public class DaveBot_1_4_5_Interface : Controller2 {
        Bot bot;

        protected override void Initialize() {
            bot = new Bot(this);

            Player.Match.OnEvent += newMatch;
            Player.Match.OnEvent += newRound;

            Player.OnEvent += turnStart;
            Player.OnEvent += turnEnd;

            Player.Match.OnEvent += shotMade;

            Player.Match.OnEvent += shipHit;
        }

        /**
         * Event Order and Info
         * MatchBeginEvent
         * RoundBeginEvent
         * PlayerTurnBeginEvent
         *  Will only trigger when it is your bots turn
         * PlayerShotEvent
         *  Will trigger for shots by any player, not just your own
         *  ShipHitEvent
         *  Will trigger when any ship is hit - if attached to player, will only get events when own ships are hit
         * PlayerTurnEndEvent
         *  Will only trigger when your bots turn ends
         */


        [EventFilter(typeof(MatchBeginEvent))]
        private void newMatch(Event ev) {
            bot.NewMatch();
        }

        [EventFilter(typeof(RoundBeginEvent))]
        private void newRound(Event ev) {
            bot.NewRound();
        }

        [EventFilter(typeof(PlayerTurnBeginEvent))]
        private void turnStart(Event ev) {
            PlayerTurnBeginEvent playerTurnBeginEvent = (PlayerTurnBeginEvent)ev;
            bot.myTurnStarts();
        }

        [EventFilter(typeof(PlayerShotEvent))]
        private void shotMade(Event ev) {
            PlayerShotEvent playerShotEvent = (PlayerShotEvent)ev;
            if (playerShotEvent.Player == Player) {
                //I made a shot
            } else {
                //other player has made a shot
            }
        }

        [EventFilter(typeof(ShipHitEvent))]
        private void shipHit(Event ev) {
            ShipHitEvent shipHitEvent = (ShipHitEvent)ev;

            Player owner = shipHitEvent.Ship.Owner;
            if (owner != Player) {

                bool sink = shipHitEvent.Ship.IsSunk();
                Coordinates coords = shipHitEvent.HitCoords;

                bot.shipHit(coords, sink);
            } else {
                //other player has made a hit
            }
        }
        [EventFilter(typeof(PlayerTurnEndEvent))]
        private void turnEnd(Event ev) {
            PlayerTurnEndEvent playerTurnEndEvent = (PlayerTurnEndEvent)ev;
            bot.myTurnEnds();
        }
    } // End Class
} // End namespace