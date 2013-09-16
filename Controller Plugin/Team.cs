using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public class Team
    {

        public Team(Team copy)
        {
            Name = copy.Name;
            Members = new List<IDNumber>(Members);
        }

        public string Name
        {
            get;
            set;
        }

        public List<IDNumber> Members
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
