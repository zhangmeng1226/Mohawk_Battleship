using MBC.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    public class ControllerRegister
    {
        public MatchInfo Match { get; set; }
        public int Score { get; set; }

        public ShipList Ships { get; set; }
        public ShotList Shots { get; set; }

        public ControllerID ID { get; set; }
        public List<ControllerID> Opponents { get; set; }

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

        public ControllerRegister MakeCompleteCopy()
        {
            ControllerRegister copy = new ControllerRegister(this);
            copy.Ships = new ShipList(Ships);
            copy.Shots = Shots.DeepCopy();
            return copy;
        }
    }
}
