using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Attributes;
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
        private const string REASON_PLACEMENT = "The player controller placed an invalid ship formation.";
        private const string REASON_SHOT = "The player controller made an invalid shot.";
        private const string REASON_TIMEOUT = "The player controller timed out.";
        private List<Player> activePlayers;
        private int currentIteration;
        private List<Player> waitList = new List<Player>();
        private AutoResetEvent waitSignal = new AutoResetEvent(false);

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

        public IEnumerable<Player> ActivePlayers
        {
            get
            {
                return activePlayers;
            }
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
        /// Gets the player that has the turn.
        /// </summary>
        public Player CurrentPlayer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Switches the current player to the next player in the iteration. Returns true if the
        /// round may continue, false if there is a winner, or no active players left.
        /// </summary>
        /// <returns></returns>
        public bool SwitchNextPlayer()
        {
            if (activePlayers.Count > 1)
            {
                if (CurrentPlayer == activePlayers[currentIteration])
                {
                    currentIteration = ++currentIteration % activePlayers.Count;
                }
                CurrentPlayer.TurnSwitchTo(activePlayers[currentIteration]);
                CurrentPlayer = activePlayers[currentIteration];
                return true;
            }
            else if (activePlayers.Count == 1)
            {
                CurrentPhase = Phase.End;
            }
            return false;
        }

        /// <summary>
        /// Plays through the current state of the match, in one iteration.
        /// </summary>
        protected override bool PlayLogic()
        {
            try
            {
                switch (CurrentPhase)
                {
                    case Phase.Placement:
                        return Placement();

                    case Phase.Turn:
                        return Turn();

                    case Phase.End:
                        return End();
                }
                return false;
            }
            catch (ControllerTimeoutException ex)
            {
                return SwitchNextPlayer();
            }
        }

        /// <summary>
        /// Logic that deals with ending the round due to the number of players reaching 1 or 0.
        /// </summary>
        /// <returns></returns>
        private bool End()
        {
            if (activePlayers.Count == 1)
            {
                Player winner = activePlayers[0];
                activePlayers.Clear();
                winner.Win();
            }
            return false;
        }

        [EventFilter(typeof(MatchAddPlayerEvent))]
        private void HandleAddPlayer(Event ev)
        {
            MatchAddPlayerEvent evCasted = (MatchAddPlayerEvent)ev;
            evCasted.Player.OnEvent += HandlePlayerLose;
            foreach (Ship ship in evCasted.Player.Ships)
            {
                ship.OnEvent += HandleShipMove;
            }
        }

        [EventFilter(typeof(PlayerLostEvent))]
        private void HandlePlayerLose(Event ev)
        {
            PlayerLostEvent evCasted = (PlayerLostEvent)ev;
            activePlayers.Remove(evCasted.Player);
        }

        [EventFilter(typeof(MatchRemovePlayerEvent))]
        private void HandleRemovePlayer(Event ev)
        {
            MatchRemovePlayerEvent evCasted = (MatchRemovePlayerEvent)ev;
            evCasted.Player.OnEvent -= HandleRemovePlayer;
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

        [EventFilter(typeof(ShipMovedEvent))]
        private void HandleShipMove(Event ev)
        {
            ShipMovedEvent evCasted = (ShipMovedEvent)ev;
            Ship ship = evCasted.Ship;
            Player player = ship.Owner;

            if (ShipList.AreShipsPlaced(player.Ships) && ShipList.AreShipsValid(player.Ships, FieldSize))
            {
                waitList.Remove(player);
                if (waitList.Count == 0)
                {
                    waitSignal.Set();
                }
            }
        }

        /// <summary>
        /// The ship placement phase for one iteration of a player.
        /// </summary>
        /// <returns></returns>
        private bool Placement()
        {
            WaitForTimeout();
            CurrentPhase = Phase.Turn;
            return SwitchNextPlayer();
        }

        /// <summary>
        /// The player shot phase for one iteration of a player.
        /// </summary>
        /// <returns></returns>
        private bool Turn()
        {
            var shotMade = CurrentPlayer.Controller.MakeShot();
            var shipHit = ShipList.GetShipAt(shotMade.ReceiverPlr.Ships, shotMade.Coordinates);
            PlayerShot(CurrentPlayer, shotMade, shipHit);
            if (!IsShotValid(CurrentPlayer, shotMade))
            {
                PlayerDisqualify(CurrentPlayer, REASON_SHOT);
                return SwitchNextPlayer();
            }

            shipHit.SetShotHit(shotMade.Coordinates, true);

            if (shipHit != null)
            {
                var sunk = shipHit.IsSunk();
                if (sunk)
                {
                    PlayerShipDestroy(shotMade.ReceiverPlr, shipHit);
                    if (shotMade.ReceiverPlr.Ships.All(ship => ship.IsSunk()))
                    {
                        PlayerLose(shotMade.ReceiverPlr);
                    }
                }
            }
            return SwitchNextPlayer();
        }

        private void WaitForTimeout()
        {
            waitSignal.WaitOne(TimeLimit);
            foreach (Player timeoutPlayer in waitList)
            {
                timeoutPlayer.Lose();
            }
        }
    }
}