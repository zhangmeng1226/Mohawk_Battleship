using MBC.Core.Threading;
using MBC.Core.Util;
using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Game
{
    /// <summary>
    /// A type of match that uses the standard rules of battleship as logic.
    /// </summary>
    public class ClassicMatch : Match
    {
        private const string REASON_PLACEMENT = "The player controller placed an invalid ship formation.";
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
            NewRoundNotify,
            Placement,
            Turn
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
        /// Adds a player to the match, and ensures that it is inactive until the next round.
        /// </summary>
        /// <param name="plr"></param>
        /// <returns></returns>
        public override bool AddPlayer(Player plr)
        {
            plr.Active = false;
            return base.AddPlayer(plr);
        }

        /// <summary>
        /// Switches the current player to the next player in the iteration.
        /// </summary>
        /// <returns></returns>
        public bool SwitchNextPlayer()
        {
            if (activePlayers.Count > 0)
            {
                if (CurrentPlayer == activePlayers[currentIteration])
                {
                    currentIteration = ++currentIteration % activePlayers.Count;
                }
                CurrentPlayer = activePlayers[currentIteration];
                return true;
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

                    case Phase.NewRoundNotify:
                        CurrentPlayer.Controller.NewRound();
                        return SwitchNextPlayer();

                    case Phase.Placement:
                        return Placement();

                    case Phase.Turn:
                        return Turn();
                }
                return false;
            }
            catch (ControllerTimeoutException ex)
            {
                PlayerDisqualified(FindPlayerFromController(ex.Controller), REASON_TIMEOUT);
                return SwitchNextPlayer();
            }
        }

        /// <summary>
        /// Finds the player with a specific controller interface.
        /// </summary>
        /// <param name="playerController"></param>
        /// <returns></returns>
        private Player FindPlayerFromController(IController playerController)
        {
            foreach (var player in Players)
            {
                if (player.Controller == playerController)
                {
                    return player;
                }
            }
            return null;
        }

        /// <summary>
        /// The initialization phase.
        /// </summary>
        /// <returns></returns>
        private bool Initialization()
        {
            if (Players.Count() < 2)
            {
                return false;
            }
            currentIteration = 0;
            activePlayers = new List<Player>();
            foreach (var plr in Players)
            {
                plr.Active = true;
                plr.Ships = StartingShips;
                activePlayers.Add(plr);
            }
            CollectionUtils.RandomizeList(activePlayers);
            CurrentPhase = Phase.Placement;
            return SwitchNextPlayer();
        }

        /// <summary>
        /// The ship placement phase for one iteration of a player.
        /// </summary>
        /// <returns></returns>
        private bool Placement()
        {
            CurrentPlayer.Ships = CurrentPlayer.Controller.PlaceShips().ToList();
            if (!ShipsValid(CurrentPlayer))
            {
                PlayerDisqualified(CurrentPlayer, REASON_PLACEMENT);
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
            return true;
        }
    }
}