﻿using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Entities;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBC.Core.Game
{
    /// <summary>
    /// A one-round match that uses the standard rules of battleship with a User verus a Bot.
    /// </summary>
    public class UserMatch : MatchCore
    {
        private List<Player> waitList = new List<Player>();
        /// <summary>
        /// Constructs a match with a specific configuration.
        /// </summary>
        /// <param name="conf">Configuration Information</param>
        public UserMatch(Configuration conf)
            : base(conf)
        {
            OnEvent += HandleAddPlayer;
            OnEvent += HandleWinner;
        }

        /// <summary>
        /// Defines all of the staes that are possible within the match.
        /// </summary>
        public enum Phase
        {
            Placement,
            PlayerTurn,
            ComputerTurn,
            End
        }

        /// <summary>
        /// Gets the current phase of this match.
        /// </summary>
        public Phase CurrentPhase
        {
            get;
            protected set;
        }
        /// <summary>
        /// Get the User player.
        /// </summary>
        public Player User
        {
            get;
            protected set;
        }

        /// <summary>
        /// Get whether the match has started.
        /// </summary>
        public bool IsStarted
        {
            get;
            protected set;
        }

        /// <summary>
        /// To know when the match is finished and there is a winner.
        /// </summary>
        public bool IsEnd
        {
            get;
            protected set;
        }

        /// <summary>
        /// To find out what player is the winner.
        /// </summary>
        public Player Winner
        {
            get;
            protected set;
        }

        /// <summary>
        /// To restart the match.
        /// </summary>
        public void Restart()
        {
            if (IsStarted)
            {
                RoundEnd(-1);
                End();
            }
            IsStarted = false;
            IsEnd = false;
        }

        /// <summary>
        /// To set the User as a player of the match.
        /// </summary>
        /// <param name="user">User controller</param>
        public void AddUser(Player user)
        {
            User = user;
            PlayerAdd(user);
        }

        /// <summary>
        /// Starts a one round match.
        /// </summary>
        public override void Play()
        {
            if (!IsRunning)
            {
                IsStarted = true;
                Begin();
                RoundBegin();
            }
            else
                PlayLogic();

        }

        /// <summary>
        /// Determines what the next action is based on what phase the match is in.
        /// </summary>
        /// <returns>Whether the game is still running.</returns>
        protected override bool PlayLogic()
        {
            switch(CurrentPhase)
            {
                case Phase.Placement:
                    break;
                case Phase.PlayerTurn:
                    break;
                case Phase.ComputerTurn:
                    break;
                case Phase.End:
                    return false;
            }
            return true;
        }

        public void ComputerTurn()
        {
            if (CurrentPhase == Phase.ComputerTurn)
            {
                foreach (Player aPlayer in Players)
                {
                    if (aPlayer == User) continue;
                    aPlayer.BeginTurn();
                    aPlayer.EndTurn();
                    CurrentPhase = Phase.PlayerTurn;
                }
            }
        }

        /// <summary>
        /// When a shot is fired, go to next player.
        /// </summary>
        /// <param name="ev">Event information</param>
        [EventFilter(typeof(PlayerShotEvent))]
        private void HandleShot(Event ev)
        {
            var playerShot = (PlayerShotEvent)ev;
            if (playerShot.Player == User)
                CurrentPhase = Phase.ComputerTurn;
            else
                CurrentPhase = Phase.PlayerTurn;

            Debug.WriteLine("HandleShot" + playerShot.Player.Name);
        }

        /// <summary>
        /// When the match is done and there is a winner.
        /// </summary>
        /// <param name="ev">Event information</param>
        [EventFilter(typeof(PlayerWonEvent))]
        private void HandleWinner(Event ev)
        {
            var winner = (PlayerWonEvent)ev;
            Winner = winner.Player;
            RoundEnd(1);
            End();
            CurrentPhase = Phase.End;
            IsEnd = true;
        }

        /// <summary>
        /// When a new player is add the event to handle their shot and the move.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(MatchAddPlayerEvent))]
        private void HandleAddPlayer(Event ev)
        {
            MatchAddPlayerEvent evCasted = (MatchAddPlayerEvent)ev;
            Player plr = evCasted.Player;

            plr.OnEvent += HandleShot;
            foreach (Ship ship in plr.Ships)
            {
                ship.OnEvent += HandleShipMove;
                ship.OnEvent += HandleShipDestroyed;
            }
        }

        /// <summary>
        /// When a ship is placed, it will check if they are all placed and if it is move to player turn.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(ShipMovedEvent))]
        private void HandleShipMove(Event ev)
        {
            ShipMovedEvent evCasted = (ShipMovedEvent)ev;
            Ship ship = evCasted.Ship;
            Player player = ship.Owner;

            if (ShipList.AreShipsPlaced(player.Ships))
            {
                waitList.Remove(player);
                if (waitList.Count == 0)
                {
                    Debug.WriteLine("yes");
                    CurrentPhase = Phase.PlayerTurn;
                    waitList.AddRange(Players);
                }
                    
            }
        }

        /// <summary>
        /// When a ship is destroyed, check if all are destroyed to determine winner.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(ShipDestroyedEvent))]
        private void HandleShipDestroyed(Event ev)
        {
            ShipDestroyedEvent evCasted = (ShipDestroyedEvent)ev;
            Ship evShip = evCasted.Ship;
            foreach (Ship ship in evShip.Owner.Ships)
            {
                if (!ship.IsSunk())
                {
                    return;
                }
            }
            evShip.Owner.Lose();
        }

        /// <summary>
        /// Handles the round begin event.
        /// </summary>
        /// <param name="ev"></param>
        [EventFilter(typeof(RoundBeginEvent))]
        private void HandleRoundBegin(Event ev)
        {
            CurrentPhase = Phase.Placement;
            waitList.Clear();
            waitList.AddRange(Players);
        }
    }
}
