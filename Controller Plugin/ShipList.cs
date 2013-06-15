using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    /// <summary>
    /// A ShipList is a specialized collection of Ship objects.
    /// </summary>
    public class ShipList : ICollection<Ship>
    {
        private List<Ship> shipList;
        private int maxLength;
        private int minLength;

        /// <summary>
        /// Copies a ShipList and its containing Ship objects.
        /// </summary>
        /// <param name="copyList"></param>
        public ShipList(ShipList copyList)
        {
            shipList = new List<Ship>();
            if (copyList != null)
            {
                minLength = copyList.shipList[0].Length;
                maxLength = copyList.shipList[0].Length;
                foreach (var ship in copyList.shipList)
                {
                    Add(new Ship(ship));
                }
            }
        }

        /// <summary>
        /// Contructs a new ShipList with existing Ship objects contained in a collection.
        /// </summary>
        /// <param name="ships">The collection of Ship objects to store in this ShipList.</param>
        public ShipList(IEnumerable<Ship> ships)
        {
            minLength = ships.First().Length;
            maxLength = minLength;
            foreach (var ship in ships)
            {
                Add(ship);
            }
        }

        /// <summary>
        /// Initializes a new ShipList object with unplaced Ship objects according to the given ship lengths.
        /// </summary>
        /// <param name="shipLengths"></param>
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
        /// Initializes an empty ShipList.
        /// </summary>
        public ShipList()
        {
            shipList = new List<Ship>();
            minLength = int.MaxValue;
            maxLength = int.MinValue;
        }

        /// <summary>
        /// Adds a Ship object to this ShipList.
        /// </summary>
        /// <param name="ship">The Ship to add.</param>
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
        /// Adds the contents of the given ShipList to this ShipList.
        /// </summary>
        /// <param name="list">The ShipList to get the Ship objects from.</param>
        public void Add(ShipList list)
        {
            foreach (var ship in list)
            {
                shipList.Add(ship);
            }
        }

        /// <summary>
        /// Determines whether or not this ShipList contains a Ship.
        /// </summary>
        /// <param name="ship">The Ship to find.</param>
        /// <returns></returns>
        public bool Contains(Ship ship)
        {
            return shipList.Contains(ship);
        }

        /// <summary>
        /// Removes a Ship from this ShipList. Note that the Ship being removed is not necessarily the object itself, but
        /// a Ship object that is equivalent to a Ship object in this ShipList.
        /// </summary>
        /// <param name="ship">The Ship to remove.</param>
        /// <returns></returns>
        public bool Remove(Ship ship)
        {
            return shipList.Remove(ship);
        }

        /// <summary>
        /// Removes all of the contained Ship objects in this ShipList.
        /// </summary>
        public void Clear()
        {
            shipList.Clear();
        }

        /// <summary>
        /// Gets the enumerator for this ShipList.
        /// </summary>
        /// <returns>The ShipListEnumerator.</returns>
        public IEnumerator<Ship> GetEnumerator()
        {
            return new ShipListEnumerator(this);
        }

        /// <summary>
        /// Copies the contents of this ShipList to the given Ship array, starting at the specified index within
        /// that array.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The index of the array to start copying to.</param>
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
        /// Generates a ReadOnlyCollection of the Ship objects that are contained in this ShipList.
        /// </summary>
        /// <returns>A ReadOnlyCollection of Ship objects contained in this ShipList.</returns>
        public ReadOnlyCollection<Ship> ToReadOnlyCollection()
        {
            return new ReadOnlyCollection<Ship>(shipList);
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
        public List<Ship> Ships
        {
            get
            {
                return new List<Ship>(shipList);
            }
        }

        /// <summary>
        /// Gets the Ship at the given Coordinates, if it exists.
        /// </summary>
        /// <param name="coord">The Coordinates to search a Ship at for.</param>
        /// <returns>A Ship object that exists at the given Coordinates. null if no ship was found.</returns>
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

        /// <summary>
        /// Generates a ShipList object containing a list of Ship objects that have been sunk according
        /// to the given ShotList.
        /// </summary>
        /// <param name="shots">The ShotList containing shots made by a single controller.</param>
        /// <returns>A ShipList of Ship objects whos Coordinates are completely occupied by the given
        /// ShotList.</returns>
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
        /// Generates a ShipList object containing a list of Ship objects that are conflicting with each
        /// other.
        /// </summary>
        /// <returns>A ShipList object of conflicting ships.</returns>
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
                        break;
                    }
                }
            }
            return conflictingShips;
        }

        /// <summary>
        /// Places a Ship in this ShipList at the specified Coordinates with the specified ShipOrientation.
        /// The method will iterate through the ShipList in the order of ship lengths starting from lowest to
        /// highest length to attempt placement at the Coordinates. This will ignore boundaries.
        /// </summary>
        /// <param name="coord">The Coordinates to place a Ship at.</param>
        /// <param name="orientation">The ShipOrientation to make the Ship.</param>
        /// <returns>true if a Ship was placed. false if there was no ship that was able to fit at the specific
        /// Coordinates or if all ships have been placed.</returns>
        public bool PlaceShip(Coordinates coord, ShipOrientation orientation, Coordinates maxCoords)
        {
            //First determine the largest possible length here...
            int targetLength = 0;
            Coordinates searchCoord = coord;
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
                    ship.Place(coord, orientation);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the enumerator for this ShipList.
        /// </summary>
        /// <returns>The ShipListEnumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ShipListEnumerator(this);
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

            public bool MoveNext()
            {
                return (++currentIdx >= collection.Count);
            }

            public void Reset()
            {
                currentIdx = -1;
            }

            void IDisposable.Dispose() { }
        }
    }
}
