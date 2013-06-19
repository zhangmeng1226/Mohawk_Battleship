using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MBC.Shared
{
    /// <summary>
    /// Contains a number of <see cref="Ship"/>s and provides a number of functions to operate on all
    /// of them.
    /// </summary>
    public class ShipList : ICollection<Ship>
    {
        private int maxLength;
        private int minLength;
        private List<Ship> shipList;

        /// <summary>
        /// Copies a <see cref="ShipList"/> while making copies of its <see cref="Ship"/>s.
        /// </summary>
        /// <param name="copyList">The <see cref="ShipList"/> to make a deep copy of.</param>
        public ShipList(ShipList copyList)
        {
            shipList = new List<Ship>();
            if (copyList != null)
            {
                minLength = int.MaxValue;
                maxLength = int.MinValue;
                foreach (var ship in copyList.shipList)
                {
                    Add(new Ship(ship));
                }
            }
        }

        /// <summary>
        /// Stores the <paramref name="ships"/> and calculates the mininum and maximum lengths of the
        /// <paramref name="ships"/>.
        /// </summary>
        /// <param name="ships">The <see cref="Ship"/>s to store.</param>
        public ShipList(IEnumerable<Ship> ships)
        {
            minLength = int.MaxValue;
            maxLength = int.MinValue;
            foreach (var ship in ships)
            {
                Add(ship);
            }
        }

        /// <summary>
        /// Stores the <paramref name="shipLengths"/> as new unplaced <see cref="Ship"/>s according to the
        /// lengths specified in the <paramref name="shipLengths"/>.
        /// </summary>
        /// <param name="shipLengths">A variable array of integers representing the <see cref="Ship.Length"/>
        /// of each <see cref="Ship"/>.</param>
        public ShipList(params int[] shipLengths)
        {
            shipList = new List<Ship>();
            minLength = shipLengths[0];
            maxLength = shipLengths[0];
            foreach (var length in shipLengths)
            {
                Add(new Ship(length));
            }
        }

        /// <summary>
        /// Creates an empty list of <see cref="Ship"/>s.
        /// </summary>
        public ShipList()
        {
            shipList = new List<Ship>();
            minLength = int.MaxValue;
            maxLength = int.MinValue;
        }

        /// <summary>
        /// Gets an integer that contains the number of Ship objects in this ShipList.
        /// </summary>
        public int Count
        {
            get
            {
                return shipList.Count;
            }
        }

        /// <summary>
        /// ShipList is not read only, so IsReadOnly is always false.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a number that identifies the largest-length Ship existing in this ShipList.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return maxLength;
            }
        }

        /// <summary>
        /// Gets a number that identifies the smallest-length Ship existing in this ShipList.
        /// </summary>
        public int MinLength
        {
            get
            {
                return minLength;
            }
        }

        /// <summary>
        /// Returns a new List of Ship objects contained in this ShipList.
        /// </summary>
        public IEnumerable<Ship> Ships
        {
            get
            {
                return new List<Ship>(shipList);
            }
        }

        /// <summary>
        /// Gets a boolean value that determines if all ships in this list has been placed or not.
        /// </summary>
        public bool ShipsPlaced
        {
            get
            {
                foreach (var ship in shipList)
                {
                    if (!ship.IsPlaced)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Overloaded index operator on this class used to retrieve the Ship objects contained in this ShipList.
        /// </summary>
        /// <param name="index">The index of the internal list to get the Ship at.</param>
        /// <returns>A Ship object.</returns>
        public Ship this[int index]
        {
            get
            {
                return shipList[index];
            }
        }

        /// <summary>
        /// Adds a <see cref="Ship"/> and ensures consistency with the <see cref="ShipList.MaxLength"/> and
        /// <see cref="ShipList.MinLength"/>.
        /// </summary>
        /// <param name="ship">The <see cref="Ship"/> to add</param>
        public void Add(Ship ship)
        {
            shipList.Add(ship);
            if (ship.Length < minLength)
            {
                minLength = ship.Length;
            }
            if (ship.Length > maxLength)
            {
                maxLength = ship.Length;
            }
        }

        /// <summary>
        /// Adds the <see cref="Ship"/>s contained in the <paramref name="list"/>.
        /// </summary>
        /// <param name="list">A <see cref="ShipList"/> to add <see cref="Ship"/>s from.</param>
        public void Add(ShipList list)
        {
            foreach (var ship in list)
            {
                shipList.Add(ship);
            }
        }

        /// <summary>
        /// Removes all of the <see cref="Ship"/>s contained.
        /// </summary>
        public void Clear()
        {
            shipList.Clear();
        }

        /// <summary>
        /// Indicates the presence of a <paramref name="ship"/>.
        /// </summary>
        /// <param name="ship">The <see cref="Ship"/> to search for.</param>
        /// <returns>A value indicating the presence of an equivalent <see cref="Ship"/>.</returns>
        public bool Contains(Ship ship)
        {
            return shipList.Contains(ship);
        }

        /// <summary>
        /// Copies the entire contents to a <see cref="Ship"/> <paramref name="array"/>, beginning at the
        /// <paramref name="arrayIndex"/> of the <paramref name="array"/>.
        /// </summary>
        /// <param name="array">The <see cref="Ship"/> array to copy to.</param>
        /// <param name="arrayIndex">The begin index of the <paramref name="array"/>.</param>
        public void CopyTo(Ship[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array is null.");
            }
            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex is less than 0.");
            }
            if (Count > array.Length - arrayIndex)
            {
                throw new ArgumentException("The number of elements in the source ShipList is greater than the available space from arrayIndex to the end of the destination array.");
            }
            for (var i = 0; i < Count; i++)
            {
                array[i + arrayIndex] = shipList[i];
            }
        }

        /// <summary>
        /// Generates a <see cref="ShipList"/> of <see cref="Ship"/>s that conflict with one another.
        /// </summary>
        /// <returns>A <see cref="ShipList"/> of conflicting <see cref="Ship"/>s.</returns>
        public ShipList GetConflictingShips()
        {
            ShipList conflictingShips = new ShipList();
            foreach (var ship in shipList)
            {
                foreach (var shipCompare in shipList)
                {
                    if (ship != shipCompare && ship.ConflictsWith(shipCompare))
                    {
                        conflictingShips.Add(ship);
                    }
                }
            }
            return conflictingShips;
        }
        /// <summary>
        /// Generates a <see cref="ShipList"/> containing <see cref="Ship"/>s that have been sunk according
        /// to the <paramref name="shots"/>.
        /// </summary>
        /// <param name="shots">The <see cref="ShotList"/> containing <see cref="Shot"/>s made against
        /// the containing <see cref="Ship"/>s.</param>
        /// <returns>A <see cref="ShipList"/> of <see cref="Ship"/>s that completely occupy the spaces
        /// given by the <paramref name="shots"/>.</returns>
        public ShipList GetSunkShips(ShotList shots)
        {
            var newList = new ShipList();
            int shotCount;
            foreach (var ship in shipList)
            {
                shotCount = 0;
                foreach (var coord in ship.GetAllLocations())
                {
                    if (shots.Contains(coord))
                    {
                        shotCount++;
                    }
                }
                if (shotCount == ship.Length)
                {
                    newList.Add(ship);
                }
            }
            return newList;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An IEnumerator of <see cref="Ship"/>s.</returns>
        public IEnumerator<Ship> GetEnumerator()
        {
            return new ShipListEnumerator(this);
        }

        /// <summary>
        /// Gets the enumerator for this <see cref="ShipList"/>.
        /// </summary>
        /// <returns>An IEnumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ShipListEnumerator(this);
        }

        /// <summary>
        /// Attempts to place an unplaced <see cref="Ship"/> at the <paramref name="coords"/> with the
        /// <paramref name="orientation"/>. Checks boundaries using the <paramref name="maxCoords"/>.
        /// </summary>
        /// <param name="coords"><see cref="Coordinates"/> to place a <see cref="Ship"/>.</param>
        /// <param name="orientation">The <see cref="ShipOrientation"/> to orient a <see cref="Ship"/>.</param>
        /// <param name="maxCoords">The maximum size of a field in a match from <see cref="Coordinates"/>.</param>
        /// <returns></returns>
        public bool PlaceShip(Coordinates coords, ShipOrientation orientation, Coordinates maxCoords)
        {
            //First determine the largest possible length here...
            int targetLength = 0;
            Coordinates searchCoord = coords;
            Coordinates addCoord;
            if (orientation == ShipOrientation.Horizontal)
            {
                addCoord = new Coordinates(1, 0);
            }
            else
            {
                addCoord = new Coordinates(0, 1);
            }
            for (var i = 0; i < maxLength; i++)
            {
                if (ShipAt(searchCoord) != null || searchCoord.X > maxCoords.X || searchCoord.Y > maxCoords.Y)
                {
                    break;
                }
                searchCoord += addCoord;
                targetLength++;
            }
            if (targetLength < minLength)
            {
                //No ships can be placed.
                return false;
            }

            //Now fit a ship with a length lower than the found length.
            foreach (var ship in shipList)
            {
                if (!ship.IsPlaced && ship.Length <= targetLength)
                {
                    ship.Place(coords, orientation);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether or not the <paramref name="ships"/> have an equal amount of <see cref="Ship"/>s and identical <see cref="Ship.Length"/>s
        /// as in this <see cref="ShipList"/>.
        /// </summary>
        /// <param name="ships">A <see cref="ShipList"/> to compare.</param>
        /// <returns>A value indicating the same <see cref="Ship"/> configuration of the two <see cref="ShipList"/>s.</returns>
        public bool EqualLengthsAs(ShipList ships)
        {
            if (ships.Count != Count)
            {
                return false;
            }
            var lengthsThis = new List<int>();
            for (var i = 0; i < Count; i++)
            {
                lengthsThis.Add(shipList[i].Length);
            }
            foreach (var ship in ships)
            {
                if (lengthsThis.Contains(ship.Length))
                {
                    lengthsThis.Remove(ship.Length);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Removes an equivalent <paramref name="ship"/>.
        /// </summary>
        /// <param name="ship">The <see cref="Ship"/> to remove.</param>
        /// <returns>A value indicating the success of the removal of the <paramref name="ship"/>.</returns>
        public bool Remove(Ship ship)
        {
            bool result = shipList.Remove(ship);
            if (ship.Length == maxLength || ship.Length == minLength)
            {
                CheckShipLengths();
            }
            return shipList.Remove(ship);
        }
        /// <summary>
        /// Searches for a <see cref="Ship"/> located at the <paramref name="coord"/>.
        /// </summary>
        /// <param name="coord">The <see cref="Coordinates"/> of a <see cref="Ship"/> to search for.</param>
        /// <returns>A <see cref="Ship"/> at the <paramref name="coord"/>. null if no <see cref="Ship"/>
        /// occupies the provided space.</returns>
        public Ship ShipAt(Coordinates coord)
        {
            foreach (var ship in shipList)
            {
                if (ship.IsAt(coord))
                {
                    return ship;
                }
            }
            return null;
        }

        private void CheckShipLengths()
        {
            foreach (var ship in shipList)
            {
                if (ship.Length < minLength)
                {
                    minLength = ship.Length;
                }
                if (ship.Length > maxLength)
                {
                    maxLength = ship.Length;
                }
            }
        }

        /// <summary>
        /// The ShipListEnumerator is used to iterate through the elements of a ShipList.
        /// </summary>
        private class ShipListEnumerator : IEnumerator<Ship>
        {
            private ShipList collection;
            private int currentIdx;

            public ShipListEnumerator(ShipList ships)
            {
                collection = ships;
                currentIdx = -1;
            }

            public Ship Current
            {
                get
                {
                    return collection[currentIdx];
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            void IDisposable.Dispose()
            {
            }

            public bool MoveNext()
            {
                return (++currentIdx >= collection.Count);
            }

            public void Reset()
            {
                currentIdx = -1;
            }
        }
    }
}