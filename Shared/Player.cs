using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MBC.Shared
{
    public class Player
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
        /// Constructs a Player with an ID, a name, and a controller.
        /// </summary>
        /// <param name="newId">The ID number in the match</param>
        /// <param name="newName">The name of the player</param>
        /// <param name="cont">The controller associated with this player</param>
        public Player(IDNumber newId, string newName, IController2 cont)
            : this(newId, newName)
        {
            Controller = cont;
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
        /// Gets the controller that makes decisions for this player.
        /// </summary>
        public IController2 Controller
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
    }
}