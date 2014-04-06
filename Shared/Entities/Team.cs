using MBC.Shared.Events;
using System;
using System.Collections.Generic;

namespace MBC.Shared
{
    [Serializable]
    public class Team : Entity
    {
        private HashSet<Player> members = new HashSet<Player>();

        public Team(Entity parent, Team copy)
            : this(parent, copy.Name, copy.IsInternal)
        {
            members = new HashSet<Player>(copy.members);
        }

        public Team(Entity parent, string name)
            : this(parent, name, false)
        {
        }

        [Obsolete()]
        public Team(Entity parent, string name, bool internalTeam)
            : base(parent)
        {
            Name = name;
            IsInternal = internalTeam;
        }

        [Obsolete()]
        public bool IsFriendly
        {
            get;
            set;
        }

        [Obsolete()]
        public bool IsInternal
        {
            get;
            set;
        }

        public HashSet<IDNumber> Members
        {
            get
            {
                HashSet<IDNumber> result = new HashSet<IDNumber>();
                foreach (Player plr in MembersPlr)
                {
                    result.Add(plr.ID);
                }
                return result;
            }
        }

        public HashSet<Player> MembersPlr
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

        public virtual void PlayerAdd(Player plr)
        {
            InvokeEvent(new TeamAddPlayerEvent(this, plr));
        }

        public virtual void PlayerRemove(Player plr)
        {
            InvokeEvent(new TeamRemovePlayerEvent(this, plr));
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1}", ID, Name);
        }
    }
}