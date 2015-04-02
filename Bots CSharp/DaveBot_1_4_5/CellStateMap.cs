using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Controllers.DaveBot_1_4_5 {
    class CellStateMap {

        public cellState[][] map;
        protected int size;

        public CellStateMap(int size) {
            this.size = size;
            this.map = new cellState[size][];

            for (int i = 0; i < map.Length; i++) {
                map[i] = new cellState[size];
            }
        }

        public void reset() {
            for (int x = 0; x < this.map.Length; x++) {
                for (int y = 0; y < this.map[x].Length; y++) {
                    this.setState(x, y, cellState.UNKNOWN);
                }
            }
        }

        public void setState(int x, int y, cellState state) {
            map[x][y] = state;
        }

        public void setState(Coordinates coords, cellState state) {
            setState(coords.X, coords.Y, state);
        }

        public cellState getState(int x, int y) {
            return map[x][y];
        }

        public cellState getState(Coordinates coords) {
            return getState(coords.X, coords.Y);
        }


    }
}
