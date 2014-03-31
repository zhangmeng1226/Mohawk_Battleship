using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        /// <summary>
        /// Constructs a match with a specific configuration.
        /// </summary>
        /// <param name="conf"></param>
        public ClassicMatch(Configuration conf)
            : base(conf)
        {
            CurrentPhase = Phase.Init;
            OnMatchEvent += HandleMatchEvent;
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
            Init,
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
                    case Phase.Init:
                        return Initialization();

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
                FindPlayerFromController(ex.Controller).Disqualify(REASON_TIMEOUT);
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

        private void HandleMatchEvent(MatchEvent matchEvent)
        {
            if (matchEvent.GetType() == typeof(MatchAddPlayerEvent))
            {
                MatchAddPlayerEvent ev = (MatchAddPlayerEvent)matchEvent;
                ev.Player.OnPlayerEvent += HandlePlayerEvent;
            }
            else if (matchEvent.GetType() == typeof(MatchRemovePlayerEvent))
            {
                MatchRemovePlayerEvent ev = (MatchRemovePlayerEvent)matchEvent;
                ev.Player.OnPlayerEvent -= HandlePlayerEvent;
            }
            else if (matchEvent.GetType() == typeof(RoundBeginEvent))
            {
                CurrentPhase = Phase.Init;
            }
            else if (matchEvent.GetType() == typeof(RoundEndEvent))
            {
                CurrentPhase = Phase.End;
            }
        }

        private void HandlePlayerEvent(PlayerEvent playerEvent)
        {
            if (playerEvent.GetType() == typeof(PlayerLostEvent))
            {
                PlayerLostEvent ev = (PlayerLostEvent)playerEvent;
                activePlayers.Remove(ev.Player);
            }
        }

        /// <summary>
        /// The ship placement phase for one iteration of a player.
        /// </summary>
        /// <returns></returns>
        private bool Placement()
        {
            if (ShipList.AreShipsPlaced(CurrentPlayer.Ships))
            {
                CurrentPhase = Phase.Turn;
                return true;
            }
            else
            {
                PlayerPlaceShips(CurrentPlayer, new HashSet<Ship>(CurrentPlayer.Controller.PlaceShips().ToList()));

                if (!AreShipsValid(CurrentPlayer))
                {
                    PlayerDisqualify(CurrentPlayer, REASON_PLACEMENT);
                }
            }
            if (!AreShipsValid(CurrentPlayer))
            {
                PlayerDisqualify(CurrentPlayer, REASON_PLACEMENT);
            }
            if (activePlayers.Count > 0 && ShipList.AreShipsPlaced(activePlayers[currentIteration + 1].Ships))
            {
                CurrentPhase = Phase.Turn;
            }
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
    }
}