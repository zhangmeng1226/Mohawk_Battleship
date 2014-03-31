using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MBC.Shared
{
    public delegate void MatchEventHandler(MatchEvent matchEvent);

    /// <summary>
    /// Provides the modifications, events, and properties that are necessary to operate
    /// a game of battleship. All events must be subscribed to.
    /// </summary>
    public abstract class Match
    {
        private EventFilter<MatchEventHandler> filter = new EventFilter<MatchEventHandler>();

        public event MatchEventHandler OnMatchEvent
        {
            add
            {
                filter.AddEventHandler(value);
            }
            remove
            {
                filter.RemoveEventHandler(value);
            }
        }

        /// <summary>
        /// Gets a boolean value indicating whether or not the match is at the end and cannot
        /// progress further.
        /// </summary>
        public bool AtEnd
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets the current round in progress in the match.
        /// </summary>
        public int CurrentRound
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets or sets the field size parameter of the match.
        /// </summary>
        public Coordinates FieldSize
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets or sets the number of rounds to satisfy in the match.
        /// </summary>
        public int NumberOfRounds
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets the players involved with the match.
        /// </summary>
        public HashSet<Player> Players
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets the random number generator available to the match.
        /// </summary>
        public Random Random
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets or sets the round mode for the match.
        /// </summary>
        public RoundMode RoundMode
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets a generated list of ships that define the ships that are to be placed at the
        /// start of a round.
        /// </summary>
        public HashSet<Ship> StartingShips
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets the teams that exist within the match.
        /// </summary>
        public HashSet<Team> Teams
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets or sets the time limit allowed for each decision in the match.
        /// </summary>
        public int TimeLimit
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Gets or sets the order in which players take turns in the match.
        /// </summary>
        public IList<Player> TurnOrder
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Checks if a player's ship are placed, and are valid within the parameters of the match.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual bool AreShipsValid(Player player)
        {
            return player.Ships != null &&
                ShipList.AreEquivalentLengths(player.Ships, StartingShips) &&
                ShipList.AreShipsValid(player.Ships, FieldSize);
        }

        /// <summary>
        /// Begins the match.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        protected internal virtual void Begin()
        {
            filter.InvokeEvent(new MatchBeginEvent(this));
        }

        /// <summary>
        /// Ends the match.
        /// </summary>
        /// <param name="player"></param>
        protected internal virtual void End()
        {
            filter.InvokeEvent(new MatchEndEvent(this));
        }

        /// <summary>
        /// Creates a MatchAddPlayerEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerAdd.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        protected internal virtual void PlayerAdd(Player player)
        {
            filter.InvokeEvent(new MatchAddPlayerEvent(this, player));
        }

        /// <summary>
        /// Creates a MatchRemovePlayerEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerRemove.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        protected internal virtual void PlayerRemove(Player player)
        {
            filter.InvokeEvent(new MatchRemovePlayerEvent(this, player));
        }

        /// <summary>
        /// Creates a RoundBeginEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnRoundBegin.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        protected internal virtual void RoundBegin(int roundBeginNumber)
        {
            filter.InvokeEvent(new RoundBeginEvent(this, roundBeginNumber));
        }

        /// <summary>
        /// Creates a RoundEndEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnRoundEnd.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        protected internal virtual void RoundEnd(int roundNumber)
        {
            filter.InvokeEvent(new RoundEndEvent(this, roundNumber));
        }

        /// <summary>
        /// Creates a MatchTeamAddEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnTeamAdd.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        protected internal virtual void TeamAdd(Team team)
        {
            filter.InvokeEvent(new MatchTeamAddEvent(this, team));
        }

        /// <summary>
        /// Creates a MatchTeamRemoveEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnTeamRemove.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        protected internal virtual void TeamRemove(Team team)
        {
            filter.InvokeEvent(new MatchTeamRemoveEvent(this, team));
        }
    }
}