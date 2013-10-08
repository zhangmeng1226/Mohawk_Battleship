using System.Collections.Generic;

namespace MBC.Shared
{
    public class Team
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

        public IDNumber ID
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
    }
}