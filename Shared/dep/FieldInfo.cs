using System;

namespace MBC.Shared
{
    [Obsolete("This information is available within the Player class.")]
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

        public ShipList Ships { get; set; }

        public ShipList ShipsLeft { get; set; }

        public ShotList Shots { get; set; }

        public ShotList ShotsAgainst { get; set; }
    }
}