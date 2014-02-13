using MBC.Core.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MBC.Shared
{
    /// <summary>
    /// This is the new framework part of a match.
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Called when the match begins. Called before any rounds begin.
        /// </summary>
        public event EventHandler<MatchBeginEvent> OnMatchBegin;

        /// <summary>
        /// Called when the match ends. Called after all rounds have ended and the
        /// match conditions have been met.
        /// </summary>
        public event EventHandler<MatchEndEvent> OnMatchEnd;

        /// <summary>
        /// Called when a player is added to the match.
        /// </summary>
        public event EventHandler<MatchAddPlayerEvent> OnPlayerAdd;

        /// <summary>
        /// Called when a player is disqualified for some reason.
        /// </summary>
        public event EventHandler<PlayerDisqualifiedEvent> OnPlayerDisqualified;

        /// <summary>
        /// Called when a players loses during the match.
        /// </summary>
        public event EventHandler<PlayerLostEvent> OnPlayerLose;

        /// <summary>
        /// Called when a player outputs a message.
        /// </summary>
        public event EventHandler<PlayerMessageEvent> OnPlayerMessage;

        /// <summary>
        /// Called when a player is removed from the match.
        /// </summary>
        public event EventHandler<MatchRemovePlayerEvent> OnPlayerRemove;

        /// <summary>
        /// Called when a player's ship has been hit completely, and has been
        /// removed from the player's list of ships.
        /// </summary>
        public event EventHandler<PlayerShipDestroyedEvent> OnPlayerShipDestruction;

        /// <summary>
        /// Called when a player finalizes their ship placement in the beginning of a round,
        /// or when the position of any of the ships move during a round.
        /// </summary>
        public event EventHandler<PlayerShipsPlacedEvent> OnPlayerShipsPlaced;

        /// <summary>
        /// Called when a player makes a shot against an opponent.
        /// </summary>
        public event EventHandler<PlayerShotEvent> OnPlayerShot;

        /// <summary>
        /// Called when a player is assigned to a team.
        /// </summary>
        public event EventHandler<PlayerTeamAssignEvent> OnPlayerTeamAssign;

        /// <summary>
        /// Called when a player times out during their turn.
        /// </summary>
        public event EventHandler<PlayerTimeoutEvent> OnPlayerTimeout;

        /// <summary>
        /// Called when a player turn switches to another player.
        /// </summary>
        public event EventHandler<PlayerTurnSwitchEvent> OnPlayerTurnSwitch;

        /// <summary>
        /// Called when a player wins a round.
        /// </summary>
        public event EventHandler<PlayerWonEvent> OnPlayerWin;

        /// <summary>
        /// Called when a round begins.
        /// </summary>
        public event EventHandler<RoundBeginEvent> OnRoundBegin;

        /// <summary>
        /// Called when a round ends.
        /// </summary>
        public event EventHandler<RoundEndEvent> OnRoundEnd;

        /// <summary>
        /// Called when a team has been added to the match.
        /// </summary>
        public event EventHandler<MatchTeamAddEvent> OnTeamAdd;

        /// <summary>
        /// Called when a team has been removed from the match.
        /// </summary>
        public event EventHandler<MatchTeamRemoveEvent> OnTeamRemove;

        /// <summary>
        /// Gets a boolean value indicating whether or not the match is at the end and cannot
        /// progress further.
        /// </summary>
        public bool AtEnd
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the current round in progress in the match.
        /// </summary>
        public int CurrentRound
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the events that have been generated within the match.
        /// </summary>
        public List<Event> Events
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the field size parameter of the match.
        /// </summary>
        public Coordinates FieldSize
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the number of rounds to satisfy in the match.
        /// </summary>
        public int NumberOfRounds
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the players involved with the match.
        /// </summary>
        public HashSet<Player> Players
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the random number generator available to the match.
        /// </summary>
        public Random Random
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the round mode for the match.
        /// </summary>
        public RoundMode RoundMode
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a generated list of ships that define the ships that are to be placed at the
        /// start of a round.
        /// </summary>
        public HashSet<Ship> StartingShips
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the teams that exist within the match.
        /// </summary>
        public HashSet<Team> Teams
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the time limit allowed for each decision in the match.
        /// </summary>
        public int TimeLimit
        {
            get;
            protected set;
        }

        /// <summary>
        /// Checks if a player's ship are placed, and are valid within the parameters of the match.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool AreShipsValid(Player player)
        {
            return player.Ships != null &&
                ShipList.AreEquivalentLengths(player.Ships, StartingShips) &&
                ShipList.AreShipsValid(player.Ships, FieldSize) &&
                ShipList.GetConflictingShips(player.Ships).Count() == 0;
        }

        protected virtual void AppendEvent(Event e)
        {
            Events.Add(e);
        }

        protected virtual bool ApplyEvent(Event e)
        {
            bool result = false;
            if (e is MatchAddPlayerEvent)
            {
                var evCast = (MatchAddPlayerEvent)e;
                result = Players.Add(evCast.Player);
                if (OnPlayerAdd != null)
                {
                    OnPlayerAdd(this, evCast);
                }
            }
            else if (e is MatchBeginEvent)
            {
                var evCast = (MatchBeginEvent)e;
                if (OnMatchBegin != null)
                {
                    OnMatchBegin(this, (MatchBeginEvent)e);
                }
            }
            else if (e is MatchEndEvent)
            {
                if (OnMatchEnd != null)
                {
                    OnMatchEnd(this, (MatchEndEvent)e);
                }
            }
            else if (e is MatchRemovePlayerEvent)
            {
                if (OnPlayerRemove != null)
                {
                    OnPlayerRemove(this, (MatchRemovePlayerEvent)e);
                }
            }
            else if (e is MatchTeamAddEvent)
            {
                if (OnTeamAdd != null)
                {
                    OnTeamAdd(this, (MatchTeamAddEvent)e);
                }
            }
            else if (e is MatchTeamRemoveEvent)
            {
                if (OnTeamRemove != null)
                {
                    OnTeamRemove(this, (MatchTeamRemoveEvent)e);
                }
            }
            else if (e is PlayerDisqualifiedEvent)
            {
                if (OnPlayerDisqualified != null)
                {
                    OnPlayerDisqualified(this, (PlayerDisqualifiedEvent)e);
                }
            }
            else if (e is PlayerLostEvent)
            {
                if (OnPlayerLose != null)
                {
                    OnPlayerLose(this, (PlayerLostEvent)e);
                }
            }
            else if (e is PlayerMessageEvent)
            {
                if (OnPlayerMessage != null)
                {
                    OnPlayerMessage(this, (PlayerMessageEvent)e);
                }
            }
            else if (e is PlayerShipDestroyedEvent)
            {
                if (OnPlayerShipDestruction != null)
                {
                    OnPlayerShipDestruction(this, (PlayerShipDestroyedEvent)e);
                }
            }
            else if (e is PlayerShipsPlacedEvent)
            {
                if (OnPlayerShipsPlaced != null)
                {
                    OnPlayerShipsPlaced(this, (PlayerShipsPlacedEvent)e);
                }
            }
            else if (e is PlayerShotEvent)
            {
                if (OnPlayerShot != null)
                {
                    OnPlayerShot(this, (PlayerShotEvent)e);
                }
            }
            else if (e is PlayerTeamAssignEvent)
            {
                if (OnPlayerTeamAssign != null)
                {
                    OnPlayerTeamAssign(this, (PlayerTeamAssignEvent)e);
                }
            }
            else if (e is PlayerTimeoutEvent)
            {
                if (OnPlayerTimeout != null)
                {
                    OnPlayerTimeout(this, (PlayerTimeoutEvent)e);
                }
            }
            else if (e is PlayerTurnSwitchEvent)
            {
                if (OnPlayerTurnSwitch != null)
                {
                    OnPlayerTurnSwitch(this, (PlayerTurnSwitchEvent)e);
                }
            }
            else if (e is PlayerWonEvent)
            {
                if (OnPlayerWin != null)
                {
                    OnPlayerWin(this, (PlayerWonEvent)e);
                }
            }
            else if (e is RoundBeginEvent)
            {
                if (OnRoundBegin != null)
                {
                    OnRoundBegin(this, (RoundBeginEvent)e);
                }
            }
            else if (e is RoundEndEvent)
            {
                if (OnRoundEnd != null)
                {
                    OnRoundEnd(this, (RoundEndEvent)e);
                }
            }
            return result;
        }
    }
}