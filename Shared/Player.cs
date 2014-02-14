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
        }

        /// <summary>
        /// Constructs a Player with an ID, a name, and a controller.
        /// </summary>
        /// <param name="newId">The ID number in the match</param>
        /// <param name="newName">The name of the player</param>
        /// <param name="cont">The controller associated with this player</param>
        public Player(IDNumber newId, string newName, IController cont)
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
            set;
        }

        /// <summary>
        /// Gets the controller that makes decisions for this player.
        /// </summary>
        public IController Controller
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of disqualifications this player accumulated.
        /// </summary>
        public int Disqualifications
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the id number of this player.
        /// </summary>
        public IDNumber ID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of losses
        /// </summary>
        public int Losses
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
            set;
        }

        /// <summary>
        /// Gets or sets the score
        /// </summary>
        public int Score
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of ships this player currently has.
        /// </summary>
        public ISet<Ship> Ships
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of shot hits
        /// </summary>
        public int ShotHits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of shot misses
        /// </summary>
        public int ShotMisses
        {
            get;
            set;
        }

        public IList<Shot> ShotsMade
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the team that this player is on.
        /// </summary>
        public Team Team
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of timeouts the controller encountered in the match.
        /// </summary>
        public int Timeouts
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of wins
        /// </summary>
        public int Wins
        {
            get;
            set;
        }
    }
}