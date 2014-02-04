using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    public static class CollectionUtils
    {
        /// <summary>
        /// Randomizes the order of a list of elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RandomizeList<T>(IList<T> list)
        {
            Random rand = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int randIdx = rand.Next(i + 1);
                T value = list[randIdx];
                list[randIdx] = list[i];
                list[i] = value;
            }
        }
    }
}