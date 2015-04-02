using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Controllers.DaveBot_1_4_5 {
    class ProbabilityMap {
        public int[][] map;
        protected int size;
        protected int[] shipLengths;

        protected CellStateMap cellStateMap;

        protected int weighted; //Multiplies the impact of adding this map to other maps

        public ProbabilityMap(int size, int[] shipLengths, CellStateMap cellStateMap) {
            this.size = size;
            this.shipLengths = shipLengths;
            this.cellStateMap = cellStateMap;
            this.weighted = 1;

            this.map = new int[size][];

            for (int i = 0; i < map.Length; i++) {
                map[i] = new int[size];
            };
        }

        public void reset() {
            for (int x = 0; x < this.map.Length; x++) {
                for (int y = 0; y < this.map[x].Length; y++) {
                    this.setValue(x, y, 0);
                }
            }
        }

        public Coordinates getHighest() {
            int maxValue = 0;
            int maxX = 0;
            int maxY = 0;

            for (int x = 0; x < this.map.Length; x++) {
                for (int y = 0; y < this.map[x].Length; y++) {

                    //if (cellStateMap.getState(x, y) != cellState.UNKNOWN && value > 0) {
                    //    continue;
                    // should never happen, if it does, there's a bug adding mis-adding probabiliy somewhere
                    //}

                    if (cellStateMap.getState(x, y) != cellState.UNKNOWN) {
                        continue;
                    }

                    int value = this.getValue(x, y);

                    if (value > maxValue) {
                        maxX = x;
                        maxY = y;
                        maxValue = value;
                    }
                }
            }

            return new Coordinates(maxX, maxY);
        }

        protected void addProbabilitiy(Coordinates startPoint, List<Coordinates> coordList, int maxLength) {
            if (maxLength < shipLengths.Min()) { // No ship can fit here
                return;
            }

            foreach (int length in shipLengths) // loop through each ships - ints representing length
            {
                if (length <= maxLength) // could this ship fit in these spaces?
                {
                    this.increment(startPoint);

                    for (int j = 0; j < length - 1; j++) // minus one to pretend ships are one point shorter, as starting point is already added
                    {
                        this.increment(coordList[j]);
                    }
                }
            }

            if (cellStateMap.getState(startPoint) != cellState.UNKNOWN) {
                this.setValue(startPoint, 0);
            }
        }

        protected void removeProbabilitiy(List<Coordinates> coordList, int maxLength) {
            if (maxLength == 0) { return; }

            foreach (int length in shipLengths) // loop through each ships - ints representing length
            {
                if (length <= maxLength) // could this ship fit in these spaces?
                {
                    for (int j = 0; j < length - 1; j++) {
                        if (this.getValue(coordList[j]) > 0) {
                            this.decrement(coordList[j]);
                        }
                    }
                }
            }
        }

        protected int howManyFree(List<Coordinates> coords) {
            // Takes a list of coordinates and returns a count of longest sequence of available shots
            int count = 0;

            foreach (Coordinates coord in coords) {
                if (cellStateMap.getState(coord) == cellState.SINK ||
                    cellStateMap.getState(coord) == cellState.IMPOSSIBLE ||
                    cellStateMap.getState(coord) == cellState.MISS) {
                    return count;
                }
                count++;
            }
            return count;

        }

        public void setValue(int x, int y, int val) {
            map[x][y] = val;
        }

        public void setValue(Coordinates coords, int val) {
            setValue(coords.X, coords.Y, val);
        }

        public int getValue(int x, int y) {
            return map[x][y];
        }

        public int getValue(Coordinates coords) {
            return getValue(coords.X, coords.Y);
        }

        public void increment(int x, int y) {
            this.map[x][y]++;
        }

        public void increment(Coordinates coords) {
            increment(coords.X, coords.Y);
        }

        public void increment(int x, int y, int amount) {
            this.map[x][y] += amount;
        }

        public void increment(Coordinates coords, int amount) {
            increment(coords.X, coords.Y, amount);
        }

        public void decrement(int x, int y) {
            this.map[x][y]--;
        }

        public void decrement(Coordinates coords) {
            decrement(coords.X, coords.Y);
        }

        public void decrement(int x, int y, int amount) {
            this.map[x][y] -= amount;
        }

        public void decrement(Coordinates coords, int amount) {
            decrement(coords.X, coords.Y, amount);
        }

        public static ProbabilityMap operator +(ProbabilityMap map1, ProbabilityMap map2) {

            ProbabilityMap addedMap = new ProbabilityMap(map1.size, map1.shipLengths, map1.cellStateMap);

            for (int x = 0; x < map1.map.Length; x++) {
                for (int y = 0; y < map1.map[x].Length; y++) {
                    int val1 = map1.getValue(x, y) * map1.weighted;
                    int val2 = map2.getValue(x, y) * map2.weighted;
                    addedMap.setValue(x, y, (val1 + val2));
                }
            }
            return addedMap;
        }
    }
}
