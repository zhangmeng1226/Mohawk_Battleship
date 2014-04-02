using System;
using System.Collections.Generic;

namespace MBC.Shared
{
    [Serializable]
    public class Team : Entity, IEquatable<Team>
    {
        private HashSet<IDNumber> members;

        public Team(Team copy)
            : this(copy.ID, copy.Name, copy.IsInternal)
        {
            members = new HashSet<IDNumber>(copy.members);
        }

        public Team(IDNumber id, string name)
            : this(id, name, false)
        {
        }

        public Team(IDNumber id, string name, bool internalTeam)
        {
            members = new HashSet<IDNumber>();
            ID = id;
            Name = name;
            IsInternal = internalTeam;
        }

        public bool IsFriendly
        {
            get;
            set;
        }

        public bool IsInternal
        {
            get;
            set;
        }

        public HashSet<IDNumber> Members
        {
            get
            {
                return members;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public static bool operator !=(Team team1, Team team2)
        {
            return !(team1 == team2);
        }

        public static bool operator ==(Team team1, Team team2)
        {
            if (Object.ReferenceEquals(team1, team2))
            {
                return true;
            }
            if (Object.ReferenceEquals(team1, null) || Object.ReferenceEquals(team2, null))
            {
                return false;
            }
            return team1.ID == team2.ID;
        }

        /// <summary>
        /// Compares the equality of this player with another player.
        /// </summary>
        /// <param name="plr"></param>
        /// <returns></returns>
        public bool Equals(Team team)
        {
            return this == team;
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

        public override string ToString()
        {
            return string.Format("[{0}] {1}", ID, Name);
        }

        protected override Type GetEntityType()
        {
            return typeof(Team);
        }
    }
}