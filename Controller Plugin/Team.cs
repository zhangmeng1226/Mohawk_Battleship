using System.Collections.Generic;

namespace MBC.Shared
{
    public class Team
    {
        private List<IDNumber> members;

        public Team(Team copy)
        {
            members = copy.members;
            ID = copy.ID;
            Name = copy.Name;
        }

        public Team(IDNumber id, string name)
        {
            members = new List<IDNumber>();
            ID = id;
            Name = name;
        }

        public IDNumber ID
        {
            get;
            set;
        }

        public List<IDNumber> Members
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