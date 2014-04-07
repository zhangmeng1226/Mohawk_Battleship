using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
using MBC.Shared.Entities;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MBC.Core.Game
{
    /// <summary>
    /// A type of match that uses the standard rules of battleship as logic.
    /// </summary>
    public class ClassicMatch : MatchCore
    {
        private const string REASON_NOT_TURN = "The player controller does not have the turn.";
        private const string REASON_PLACEMENT = "The player controller placed an invalid ship formation.";
        private const string REASON_SHOT = "The player controller shot at the same cell.";
        private const string REASON_TIMEOUT = "The player controller timed out.";
        private List<Player> waitList = new List<Player>();
        private ManualResetEvent waitSignal = new ManualResetEvent(false);

        /// <summary>
        /// Constructs a match with a specific configuration.
        /// </summary>
        /// <param name="conf"></param>
        public ClassicMatch(Configuration conf)
            : base(conf)
        {
            CurrentPhase = Phase.Placement;
            OnEvent += HandleAddPlayer;
            OnEvent += HandleRemovePlayer;
            OnEvent += HandleRoundEnd;
            OnEvent += HandleRoundBegin;
        }

        /// <summary>
        /// Constructs a match with the application-wide configuration.
        /// </summary>
        public ClassicMatch()
            : this(Configuration.Global)
        {
        }

        /// <summary>
        /// Defines all of the states that are possible within the match.
        /// </summary>
        public enum Phase
        {
            Placement,
            Turn,
            End
        }

        /// <summary>
        /// Gets the current phase of this type of match.
        /// </summary>
        public Phase CurrentPhase
        {
            get;
            protected set;
        }

        /// <summary>
        /// Plays through the current state of the match, in one iteration.
        /// </summary>
        protected override bool PlayLogic()
        {
            switch (CurrentPhase)
            {
                case Phase.Placement:
                    WaitForTimeout();
                    CurrentPhase = Phase.Turn;
                    break;

                case Phase.Turn:
                    waitList.Add(CurrentPlayer);
                    CurrentPlayer.BeginTurn();
                    WaitForTimeout();
                    CurrentPlayer.EndTurn();
                    if (TurnOrder.Count == 1)
                    {
                        CurrentPlayer.Win();
                        CurrentPhase = Phase.End;
                    }
                    break;

                case Phase.End:
                    return false;
            }
            return true;
        }

        [EventFilter(typeof(MatchAddPlayerEvent))]
        private void HandleAddPlayer(Event ev)
        {
            MatchAddPlayerEvent evCasted = (MatchAddPlayerEvent)ev;
            Player plr = evCasted.Player;
            Team plrTeam = new Team(this, String.Format("{0}'s team", plr.Name));
            TeamAdd(plrTeam);
            plrTeam.PlayerAdd(plr);

            plr.OnEvent += HandlePlayerShot;
            plr.OnEvent += HandlePlayerLose;
            foreach (Ship ship in plr.Ships)
            {
                ship.OnEvent += HandleShipMove;
                ship.OnEvent += HandleShipDestroyed;
            }
        }

        [EventFilter(typeof(PlayerLostEvent))]
        private void HandlePlayerLose(Event ev)
        {
        }

        [EventFilter(typeof(PlayerShotEvent))]
        private void HandlePlayerShot(Event ev)
        {
            PlayerShotEvent evCasted = (PlayerShotEvent)ev;
            Shot shot = evCasted.Shot;
            Player plr = evCasted.Player;
            if (plr != CurrentPlayer)
            {
                throw new InvalidEventException(evCasted, REASON_NOT_TURN);
            }
            if (plr.ShotsMade.Count(x => x == shot) > 1)
            {
                plr.Disqualify(REASON_SHOT);
                plr.Lose();
                waitSignal.Set();
                throw new InvalidEventException(evCasted, REASON_SHOT);
            }
            waitList.Remove(plr);
            waitSignal.Set();
        }

        [EventFilter(typeof(MatchRemovePlayerEvent))]
        private void HandleRemovePlayer(Event ev)
        {
            MatchRemovePlayerEvent evCasted = (MatchRemovePlayerEvent)ev;
            evCasted.Player.OnEvent -= HandlePlayerShot;
            foreach (Ship ship in evCasted.Player.Ships)
            {
                ship.OnEvent -= HandleShipMove;
                ship.OnEvent -= HandleShipDestroyed;
            }
        }

        [EventFilter(typeof(RoundBeginEvent))]
        private void HandleRoundBegin(Event ev)
        {
            CurrentPhase = Phase.Placement;
            waitList.Clear();
            waitList.AddRange(Players);
        }

        [EventFilter(typeof(RoundEndEvent))]
        private void HandleRoundEnd(Event ev)
        {
            CurrentPhase = Phase.End;
        }

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
                    waitSignal.Set();
                }
            }
        }

        private void WaitForTimeout()
        {
            waitSignal.WaitOne(TimeLimit);
            waitSignal.Reset();
            foreach (Player timeoutPlayer in waitList)
            {
                timeoutPlayer.Disqualify(REASON_TIMEOUT);
                timeoutPlayer.Lose();
            }
            waitList.Clear();
        }
    }
}