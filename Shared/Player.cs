using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MBC.Shared
{
    [Serializable]
    public class Player : Entity, IEquatable<Player>
    {
        /// <summary>
        /// Constructs a Player with an ID and a name. This player will not have a controller.
        /// </summary>
        /// <param name="newId">The ID number in the match</param>
        /// <param name="newName">The name of the player</param>
        public Player(IDNumber newId, string newName)
        {
            ID = newId;
            Name = newName;
            Active = false;
        }

        /// <summary>
        /// Gets or sets a boolean value indicating whether or not this player is
        /// actively participating at the moment.
        /// </summary>
        public bool Active
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the number of disqualifications this player accumulated.
        /// </summary>
        public int Disqualifications
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the id number of this player.
        /// </summary>
        public IDNumber ID
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the number of losses
        /// </summary>
        public int Losses
        {
            get;
            internal set;
        }

        public Match Match
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the score
        /// </summary>
        public int Score
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the list of ships this player currently has.
        /// </summary>
        public ISet<Ship> Ships
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the number of shot hits
        /// </summary>
        public int ShotHits
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the number of shot misses
        /// </summary>
        public int ShotMisses
        {
            get;
            internal set;
        }

        public IList<Shot> ShotsMade
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the team that this player is on.
        /// </summary>
        public Team Team
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the number of timeouts the controller encountered in the match.
        /// </summary>
        public int Timeouts
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the number of wins
        /// </summary>
        public int Wins
        {
            get;
            internal set;
        }

        /// <summary>
        /// Assigns this player to a team
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the player.</exception>
        public virtual void AssignToTeam(Team team)
        {
            InvokeEvent(new PlayerTeamAssignEvent(this, team));
        }

        public virtual void BeginTurn()
        {
            InvokeEvent(new PlayerBeginTurnEvent(this));
        }

        /// <summary>
        /// Disqualifies the player
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        public virtual void Disqualify(String reason)
        {
            InvokeEvent(new PlayerDisqualifiedEvent(this, reason));
        }

        /// <summary>
        /// Switches the turn from this player to another.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the player.</exception>
        public virtual void EndTurn()
        {
            InvokeEvent(new PlayerEndTurnEvent(this));
        }

        /// <summary>
        /// Compares the equality of this player with another player.
        /// </summary>
        /// <param name="plr"></param>
        /// <returns></returns>
        public bool Equals(Player plr)
        {
            return GetHashCode() == plr.GetHashCode();
        }

        /// <summary>
        /// Compares the equality of this player with another object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the unique identifier for this player.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ID;
        }

        /// <summary>
        /// Makes the player lose
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the player.</exception>
        public virtual void Lose()
        {
            InvokeEvent(new PlayerLostEvent(this));
        }

        /// <summary>
        /// Makes the player send a message.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        public virtual void Message(String message)
        {
            InvokeEvent(new PlayerMessageEvent(this, message));
        }

        /// <summary>
        /// Shoots a shot for the player.
        /// </summary>
        /// <param name="shot"></param>
        public virtual void Shoot(Shot shot)
        {
            InvokeEvent(new PlayerShotEvent(this, shot));
        }

        /// <summary>
        /// Shoots against a player opponent at the specified X and Y coordinates.
        /// </summary>
        /// <param name="opponent"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public virtual void Shoot(Player opponent, int x, int y)
        {
            Shoot(new Shot(opponent, new Coordinates(x, y)));
        }

        /// <summary>
        /// Makes the player timeout.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the player.</exception>
        public virtual void Timeout(String method)
        {
            InvokeEvent(new PlayerTimeoutEvent(this, method));
        }

        /// <summary>
        /// Generates a string representation of the player.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString()
        {
            return string.Format("[{0}] {1}", ID, Name);
        }

        /// <summary>
        /// Makes the player win.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>The generated event</returns>
        /// <exception cref="InvalidEventException">Thrown when the event being created is not valid for the
        /// current state of the player.</exception>
        public virtual void Win()
        {
            InvokeEvent(new PlayerWonEvent(this));
        }
    }
}