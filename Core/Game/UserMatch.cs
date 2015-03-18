using MBC.Core.Util;
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
        private Player computer;
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
            OnEvent += HandleRoundBegin;
            OnEvent += HandleEndTurn;
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
            if (!IsStarted)
            {
                IsStarted = true;
                Begin();
                RoundBegin();
            }
            else
                PlayLogic();
        }

        /// <summary>
        /// 
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
                    ComputerTurn();
                    break;
                case Phase.End:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Starts the Users turn, shots and ends the turn.
        /// </summary>
        /// <param name="shot"></param>
        public void UserShoot(Shot shot)
        {
            if (CurrentPhase == Phase.PlayerTurn)
            {
                User.BeginTurn();
                User.Shoot(shot);
                User.EndTurn();
            }
        }

        /// <summary>
        /// Computer begins their turn.
        /// </summary>
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
           
        }

        /// <summary>
        /// When the match is done and there is a winner.
        /// </summary>
        /// <param name="ev">Event information</param>
        [EventFilter(typeof(PlayerWonEvent))]
        private void HandleWinner(Event ev)
        {
            CurrentPhase = Phase.End;
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
            if (plr != User)
                computer = plr;

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
                    if (CurrentPlayer == User)
                        CurrentPhase = Phase.PlayerTurn;
                    else
                        CurrentPhase = Phase.ComputerTurn;
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
            CurrentPhase = Phase.End;
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

        [EventFilter(typeof(PlayerTurnEndEvent))]
        private void HandleEndTurn(Event ev)
        {
            var endTurn = (PlayerTurnEndEvent)ev;   
            if (CurrentPhase == Phase.End)
                CurrentPlayer.Win();
        }
    }
}
