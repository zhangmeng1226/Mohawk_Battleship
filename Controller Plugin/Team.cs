using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public class Team
    {

        public List<IDNumber> members;

        public Team(Team copy)
        {
            members = copy.members;
            ID = copy.ID;
            Name = copy.Name;
        }

        public Team()
        {
            members = new List<IDNumber>();
        }

        public IDNumber ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public IList<IDNumber> Members
        {
            get
            {
                return members.AsReadOnly();
            }
        }
    }
}
