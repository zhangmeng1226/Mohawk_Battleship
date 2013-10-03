using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public class FieldInfo
    {
        public FieldInfo()
        {
            Ships = new ShipList();
            Shots = new ShotList();
            ShipsLeft = new ShipList();
            ShotsAgainst = new ShotList();
        }

        public FieldInfo(FieldInfo copy)
        {
            if (copy != null)
            {
                Ships = new ShipList(copy.Ships);
                Shots = new ShotList(copy.Shots);
                ShipsLeft = new ShipList(copy.ShipsLeft);
                ShotsAgainst = new ShotList(copy.ShotsAgainst);
            }
        }

        public ShipList Ships { get; internal set; }
        public ShotList Shots { get; internal set; }
        public ShipList ShipsLeft { get; internal set; }
        public ShotList ShotsAgainst { get; internal set; }
    }
}
