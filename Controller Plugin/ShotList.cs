using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core
{
    /// <summary>
    /// A ShotList object contains a list of Shot objects and provides various functions to operate
    /// on them. 
    /// </summary>
    public class ShotList : ICollection<Shot>
    {
        private List<Shot> shotHistory;
        private Dictionary<ControllerID, List<Shot>> shotsOrganizedSender;
        private Dictionary<ControllerID, List<Shot>> shotsOrganizedReceiver;

        /// <summary>
        /// Contructs a new ShotList copying the contents of an existing ShotList.
        /// </summary>
        /// <param name="copyList">The ShotList to copy.</param>
        public ShotList(ShotList copyList)
        {
            shotsOrganizedSender = new Dictionary<ControllerID, List<Shot>>();
            shotsOrganizedReceiver = new Dictionary<ControllerID, List<Shot>>();

            shotHistory = new List<Shot>();

            Add(copyList);
        }

        /// <summary>
        /// Constructs a new empty ShotList object.
        /// </summary>
        public ShotList()
        {
            shotHistory = new List<Shot>();
            shotsOrganizedSender = new Dictionary<ControllerID, List<Shot>>();
            shotsOrganizedReceiver = new Dictionary<ControllerID, List<Shot>>();
        }

        /// <summary>
        /// Adds a Shot to this ShotList.
        /// </summary>
        /// <param name="shot">The Shot to add.</param>
        public void Add(Shot shot)
        {
            shotHistory.Add(shot);
            shotsOrganizedSender[shot.Sender].Add(shot);
            shotsOrganizedReceiver[shot.Receiver].Add(shot);
        }

        /// <summary>
        /// Appends the contents of a ShotList to this ShotList.
        /// </summary>
        /// <param name="shots">The ShotList to add.</param>
        public void Add(ShotList shots)
        {
            foreach (var shot in shots.shotHistory)
            {
                shotHistory.Add(shot);
                shotsOrganizedSender[shot.Sender].Add(shot);
                shotsOrganizedReceiver[shot.Receiver].Add(shot);
            }
        }

        /// <summary>
        /// Determines whether or not this ShotList contains a Shot.
        /// </summary>
        /// <param name="shot">The Shot to check for.</param>
        /// <returns>true if a Shot with the same field values as the given Shot exists in this ShipList.</returns>
        public bool Contains(Shot shot)
        {
            return shotsOrganizedSender[shot.Sender].Contains(shot) || shotsOrganizedReceiver[shot.Receiver].Contains(shot);
        }

        /// <summary>
        /// Determines whether or not this ShotList contains a Shot that has been placed at the given Coordinates.
        /// </summary>
        /// <param name="shotCoords">The Coordinates to look for.</param>
        /// <returns>true if at least one Shot in this ShotList has been placed at the given Coordinates.</returns>
        public bool Contains(Coordinates shotCoords)
        {
            foreach (var shots in shotHistory)
            {
                if (shots.Coordinates == shotCoords)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes a Shot from this ShotList.
        /// </summary>
        /// <param name="shot">The Shot to remove.</param>
        /// <returns>true if the Shot was in this ShotList and has been removed.</returns>
        public bool Remove(Shot shot)
        {
            bool result = shotHistory.Remove(shot);
            result &= shotsOrganizedReceiver[shot.Receiver].Remove(shot) || shotsOrganizedSender[shot.Sender].Remove(shot);
            return result;
        }

        /// <summary>
        /// Removes the last Shot made from this ShotList.
        /// </summary>
        public void RemoveLast()
        {
            var lastShot = shotHistory.Last();
            shotHistory.RemoveAt(shotHistory.Count - 1);

            var organizedList = shotsOrganizedReceiver[lastShot.Receiver];
            if (organizedList.Last() == lastShot)
            {
                organizedList.RemoveAt(organizedList.Count);
            }
            organizedList = shotsOrganizedSender[lastShot.Sender];
            if (organizedList.Last() == lastShot)
            {
                organizedList.RemoveAt(organizedList.Count);
            }
        }

        /// <summary>
        /// Removes all Shot objects that are in a given ShotList.
        /// </summary>
        /// <param name="shots">The ShotList of Shots to remove.</param>
        public void Remove(ShotList shots)
        {
            foreach (var shot in shots.shotHistory)
            {
                Remove(shot);
            }
        }

        /// <summary>
        /// Removes all Shot objects that are equivalent.
        /// </summary>
        public void RemoveDuplicates()
        {
            int count = 0;
            foreach (var shot1 in shotHistory)
            {
                count = 0;
                foreach (var shot2 in shotHistory)
                {
                    if (shot1 == shot2)
                    {
                        count++;
                    }
                    if (count > 1)
                    {
                        Remove(shot1);
                    }
                }
            }
        }

        /// <summary>
        /// Removes all of the shots from this ShotList.
        /// </summary>
        public void Clear()
        {
            shotHistory.Clear();
            shotsOrganizedSender.Clear();
            shotsOrganizedReceiver.Clear();
        }

        /// <summary>
        /// Gets a ShotListEnumerator for this ShotList.
        /// </summary>
        public IEnumerator<Shot> GetEnumerator()
        {
            return new ShotListEnumerator(this);
        }

        /// <summary>
        /// Gets a ShotListEnumerator for this ShotList.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ShotListEnumerator(this);
        }

        /// <summary>
        /// Copies the contents of this ShotList to a specified array.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The index of the array to begin copying.</param>
        public void CopyTo(Shot[] array, int arrayIndex)
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
                array[i + arrayIndex] = shotHistory[i];
            }
        }

        /// <summary>
        /// Gets the number of shots in this ShotList.
        /// </summary>
        public int Count
        {
            get
            {
                return shotHistory.Count;
            }
        }

        /// <summary>
        /// Every ShotList is not read-only, so this returns false.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a Shot at the given index.
        /// </summary>
        public Shot this[int idx]
        {
            get
            {
                return shotHistory[idx];
            }
        }

        /// <summary>
        /// Generates a ShotList of Shot objects whose Coordinates are equal to the given Coordinates.
        /// </summary>
        /// <param name="coords">The Coordinates to get a ShotList for.</param>
        /// <returns>A ShotList of Shot objects whose Coordinates are equal to that given.</returns>
        public ShotList ShotsFromCoordinates(Coordinates coords)
        {
            ShotList newList = new ShotList();
            foreach (var shot in shotHistory)
            {
                if (shot.Coordinates == coords)
                {
                    newList.Add(shot);
                }
            }
            return newList;
        }

        /// <summary>
        /// Generates and returns a ShotList of Shot objects that have been sent by a specific ControllerID.
        /// </summary>
        public ShotList ShotsFromSender(ControllerID idx)
        {
            ShotList newList = new ShotList();
            newList.shotHistory = shotHistory;
            newList.shotsOrganizedSender[idx] = shotsOrganizedSender[idx];
            return newList;
        }

        /// <summary>
        /// Generates and returns a ShotList of Shot objects that have been received by a specific ControllerID
        /// </summary>
        public ShotList ShotsToReceiver(ControllerID idx)
        {
            ShotList newList = new ShotList();
            newList.shotHistory = shotHistory;
            newList.shotsOrganizedReceiver[idx] = shotsOrganizedReceiver[idx];
            return newList;
        }

        /// <summary>
        /// Creates a new ShotList with Shot objects in both list1 and list2.
        /// </summary>
        /// <returns>A new ShotList object with the Coordinatess of both ShotList objects.</returns>
        public static ShotList operator +(ShotList list1, ShotList list2)
        {
            ShotList newList = new ShotList(list1);
            newList.Add(list2);
            return newList;
        }

        /// <summary>
        /// Creates a new ShotList with the Shot objects in list2 removed from list1.
        /// </summary>
        /// <param name="list1">The ShotList to remove Shot objects from.</param>
        /// <param name="list2">The ShotList containing the Shot objects to remove from list1</param>
        /// <returns>A new ShotList object.</returns>
        public static ShotList operator -(ShotList list1, ShotList list2)
        {
            ShotList newList = new ShotList(list1);
            newList.Remove(list2);
            return newList;
        }

        /// <summary>
        /// The ShotListEnumerator enumerates through a ShotList.
        /// </summary>
        private class ShotListEnumerator : IEnumerator<Shot>
        {
            private ShotList collection;
            private int currentIdx;

            public ShotListEnumerator(ShotList shots)
            {
                collection = shots;
                currentIdx = -1;
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

            public Shot Current
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
        }
    }
}
