using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MBC.Shared
{
    /// <summary>
    /// Provides the modifications, events, and properties that are necessary to operate
    /// a game of battleship. All events must be subscribed to.
    /// </summary>
    public abstract class Match
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
        /// Creates a MatchBeginEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnMatchBegin.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        public virtual MatchBeginEvent Begin()
        {
            var ev = new MatchBeginEvent(this);
            OnMatchBegin(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a MatchEndEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnMatchEnd.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        public virtual MatchEndEvent End()
        {
            var ev = new MatchEndEvent();
            OnMatchEnd(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a MatchAddPlayerEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerAdd.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual MatchAddPlayerEvent PlayerAdd(Player player)
        {
            var ev = new MatchAddPlayerEvent(player);
            if (!Players.Add(player))
            {
                throw new InvalidEventException(ev, "Player already exists within match.");
            }
            OnPlayerAdd(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerDisqualifiedEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerDisqualified.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        public virtual PlayerDisqualifiedEvent PlayerDisqualify(Player player, String reason)
        {
            var ev = new PlayerDisqualifiedEvent(player, reason);
            ev.Player.Disqualifications++;
            OnPlayerDisqualified(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerLostEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerLose.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerLostEvent PlayerLose(Player player)
        {
            var ev = new PlayerLostEvent(player);
            player.Losses++;
            OnPlayerLose(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerMessageEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerMessage.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        public virtual PlayerMessageEvent PlayerMessage(Player player, String message)
        {
            var ev = new PlayerMessageEvent(player, message);
            OnPlayerMessage(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerShipsPlacedEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerShipsPlaced.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerShipsPlacedEvent PlayerPlaceShips(Player player, ISet<Ship> placedShips)
        {
            var ev = new PlayerShipsPlacedEvent(player, placedShips);
            player.Ships = placedShips;
            OnPlayerShipsPlaced(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a MatchRemovePlayerEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerRemove.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual MatchRemovePlayerEvent PlayerRemove(Player player)
        {
            var ev = new MatchRemovePlayerEvent(player);
            if (!Players.Remove(player))
            {
                throw new InvalidEventException(ev, "Player does not exist within the match.");
            }
            OnPlayerRemove(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerShipDestroyedEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerShipDestruction.
        /// </summary>
        /// <param name="shipOwner"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerShipDestroyedEvent PlayerShipDestroy(Player shipOwner, Ship destroyedShip)
        {
            var ev = new PlayerShipDestroyedEvent(shipOwner, destroyedShip);
            if (!destroyedShip.IsSunk())
            {
                throw new InvalidEventException(ev, "The ship in the event is not sunken.");
            }
            OnPlayerShipDestruction(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerShotEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerShot.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerShotEvent PlayerShot(Player player, Shot shot)
        {
            return PlayerShot(player, shot, null);
        }

        /// <summary>
        /// Creates a PlayerShotEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerShot.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerShotEvent PlayerShot(Player player, Shot shot, Ship shipHit)
        {
            var ev = new PlayerShotEvent(player, shot, shipHit);
            player.ShotsMade.Add(shot);
            OnPlayerShot(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerTeamAssignEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerTeamAssign.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerTeamAssignEvent PlayerTeamAssign(Player player, Team team)
        {
            var ev = new PlayerTeamAssignEvent(player, team);
            if (player.Team != team)
            {
                player.Team = team;
            }
            else
            {
                throw new InvalidEventException(ev, "The player is already on the team.");
            }
            OnPlayerTeamAssign(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerTimeoutEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerTimeout.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerTimeoutEvent PlayerTimeout(Player player, String method)
        {
            var ev = new PlayerTimeoutEvent(player, method);
            player.Timeouts++;
            OnPlayerTimeout(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerTurnSwitchEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerTurnSwitch.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerTurnSwitchEvent PlayerTurnSwitch(Player prevPlayer, Player nextPlayer)
        {
            var ev = new PlayerTurnSwitchEvent(prevPlayer, nextPlayer);
            OnPlayerTurnSwitch(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a PlayerWonEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnPlayerWin.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual PlayerWonEvent PlayerWin(Player player)
        {
            var ev = new PlayerWonEvent(player);
            player.Wins++;
            OnPlayerWin(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a RoundBeginEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnRoundBegin.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual RoundBeginEvent RoundBegin(int roundBeginNumber)
        {
            var ev = new RoundBeginEvent(roundBeginNumber);
            if ((CurrentRound + 1) == roundBeginNumber)
            {
                CurrentRound++;
                foreach (var plr in Players)
                {
                    plr.Active = true;
                }
            }
            else
            {
                throw new InvalidEventException(ev, "The round that is beginning is not correct.");
            }
            OnRoundBegin(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a RoundEndEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnRoundEnd.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual RoundEndEvent RoundEnd(int roundNumber)
        {
            var ev = new RoundEndEvent(roundNumber);
            if (!(CurrentRound > -1))
            {
                throw new InvalidEventException(ev, "There is no round that has started.");
            }
            OnRoundEnd(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a MatchTeamAddEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnTeamAdd.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual MatchTeamAddEvent TeamAdd(Team team)
        {
            var ev = new MatchTeamAddEvent(team);
            if (!Teams.Add(team))
            {
                throw new InvalidEventException(ev, "Team already exists within match.");
            }
            OnTeamAdd(this, ev);
            return ev;
        }

        /// <summary>
        /// Creates a MatchTeamRemoveEvent from within the match and returns it. Invokes any event
        /// subscriptions to OnTeamRemove.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the match.</exception>
        public virtual MatchTeamRemoveEvent TeamRemove(Team team)
        {
            var ev = new MatchTeamRemoveEvent(team);
            if (!Teams.Remove(team))
            {
                throw new InvalidEventException(ev, "Team does not existin within the match.");
            }
            OnTeamRemove(this, ev);
            return ev;
        }
    }
}