using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public class ControllerRegister : IEquatable<ControllerRegister>
    {
        public ControllerRegister(ControllerRegister copy)
        {
            Match = copy.Match;
            Score = copy.Score;
            Ships = copy.Ships;
            Shots = copy.Shots;
            ID = copy.ID;
        }

        public ControllerRegister(MatchInfo match, ControllerID id)
        {
            Match = match;
            ID = id;
            Score = 0;
        }

        public MatchInfo Match { get; set; }
        public int Score { get; set; }

        public ShipList Ships { get; set; }
        public ShotList Shots { get; set; }

        public ControllerID ID { get; set; }
        public List<ControllerID> Opponents { get; set; }

        public ControllerRegister MakeCompleteCopy()
        {
            ControllerRegister copy = new ControllerRegister(this);
            copy.Ships = new ShipList(Ships);
            copy.Shots = Shots.DeepCopy();
            return copy;
        }

        public override string ToString()
        {
            return Match.Players[ID];
        }

        public bool Equals(ControllerRegister register)
        {
            if (register == null)
            {
                return false;
            }
            return (ID == register.ID) && (Match == register.Match);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash * 37 + ID;
            hash = hash * 37 + Match.GetHashCode();
            return hash;
        }
    }
}
