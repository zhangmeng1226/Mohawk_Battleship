using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MBC.Shared
{
    public class Player : INotifyPropertyChanged
    {
        public const string PROPERTY_ACTIVE = "Active";
        public const string PROPERTY_LOSSES = "Losses";
        public const string PROPERTY_SCORE = "Score";
        public const string PROPERTY_SHIPS = "Ships";
        public const string PROPERTY_SHOTHITS = "ShotHits";
        public const string PROPERTY_SHOTMISSES = "ShotMisses";
        public const string PROPERTY_TEAM = "Team";
        public const string PROPERTY_TIMEOUTS = "Timeouts";
        public const string PROPERTY_WINS = "Wins";
        private Boolean active;

        private IController controller;

        private IDNumber id;

        private int losses;

        private string name;

        private int score;

        private BindingList<Ship> ships;

        private int shotHits;

        private int shotMisses;

        private Team team;

        private int timeouts;
        private int wins;

        /// <summary>
        /// Constructs a Player with an ID and a name. This player will not have a controller.
        /// </summary>
        /// <param name="newId">The ID number in the match</param>
        /// <param name="newName">The name of the player</param>
        public Player(IDNumber newId, string newName)
        {
            id = newId;
            name = newName;

            ships = new BindingList<Ship>();
            ships.ListChanged += ShipsListChanged;
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
            controller = cont;
        }

        /// <summary>
        /// Called when a property in the player changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a boolean value indicating whether or not this player is
        /// actively participating at the moment.
        /// </summary>
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                NotifyPropertyChanged(PROPERTY_ACTIVE);
            }
        }

        /// <summary>
        /// Gets the controller that makes decisions for this player.
        /// </summary>
        public IController Controller
        {
            get
            {
                return controller;
            }
        }

        /// <summary>
        /// Gets the id number of this player.
        /// </summary>
        public IDNumber ID
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// Gets or sets the number of losses
        /// </summary>
        public int Losses
        {
            get
            {
                return losses;
            }
            set
            {
                losses = value;
                NotifyPropertyChanged(PROPERTY_LOSSES);
            }
        }

        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets or sets the score
        /// </summary>
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                NotifyPropertyChanged(PROPERTY_SCORE);
            }
        }

        /// <summary>
        /// Gets the list of ships this player currently has.
        /// </summary>
        public IList<Ship> Ships
        {
            get
            {
                return ships;
            }
        }

        /// <summary>
        /// Gets or sets the number of shot hits
        /// </summary>
        public int ShotHits
        {
            get
            {
                return shotHits;
            }
            set
            {
                shotHits = value;
                NotifyPropertyChanged(PROPERTY_SHOTHITS);
            }
        }

        /// <summary>
        /// Gets or sets the number of shot misses
        /// </summary>
        public int ShotMisses
        {
            get
            {
                return shotMisses;
            }
            set
            {
                shotMisses = value;
                NotifyPropertyChanged(PROPERTY_SHOTMISSES);
            }
        }

        /// <summary>
        /// Gets or sets the team that this player is on.
        /// </summary>
        public Team Team
        {
            get
            {
                return team;
            }
            set
            {
                team = value;
                NotifyPropertyChanged(PROPERTY_TEAM);
            }
        }

        /// <summary>
        /// Gets or sets the number of timeouts the controller encountered in the match.
        /// </summary>
        public int Timeouts
        {
            get
            {
                return timeouts;
            }
            set
            {
                timeouts = value;
                NotifyPropertyChanged(PROPERTY_TIMEOUTS);
            }
        }

        /// <summary>
        /// Gets or sets the number of wins
        /// </summary>
        public int Wins
        {
            get
            {
                return wins;
            }
            set
            {
                wins = value;
                NotifyPropertyChanged(PROPERTY_WINS);
            }
        }

        private void NotifyPropertyChanged(String property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void ShipsListChanged(object list, ListChangedEventArgs eventArgs)
        {
            switch (eventArgs.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                case ListChangedType.ItemDeleted:
                    NotifyPropertyChanged(PROPERTY_SHIPS);
                    break;
            }
        }
    }
}