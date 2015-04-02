using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Controllers.DaveBot_1_4_5 {
    class HistoryMap : ProbabilityMap {
        public double average;
        public double[][] variations;

        public HistoryMap(int size, int[] shipLengths, CellStateMap cellStateMap)
            : base(size, shipLengths, cellStateMap) {
            this.variations = new double[size][];

            for (int i = 0; i < this.variations.Length; i++) {
                this.variations[i] = new double[size];
            };
        }

        public void calculateVariations() {
            for (int x = 0; x < this.variations.Length; x++) {
                for (int y = 0; y < this.variations[x].Length; y++) {
                    this.variations[x][y] = this.getValue(x, y) / this.average;
                }
            }
        }

        public void calculateAverage() {
            int total = 0;
            int count = 0;

            for (int x = 0; x < this.map.Length; x++) {
                for (int y = 0; y < this.map[x].Length; y++) {
                    total += this.getValue(x, y);
                    count++;
                }
            }
            this.average = total / count;
        }

        public double getVariation(int x, int y) {
            return this.variations[x][y];
        }

        public double getVariation(Coordinates coords) {
            return this.getVariation(coords.X, coords.Y);
        }

    }
}
