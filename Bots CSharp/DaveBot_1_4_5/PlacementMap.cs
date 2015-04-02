using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Controllers.DaveBot_1_4_5 {
    class PlacementMap : ProbabilityMap {

        public PlacementMap(int size, int[] shipLengths, CellStateMap cellStateMap)
            : base(size, shipLengths, cellStateMap) {
        }

        public void calcProbabilities() {
            Coordinates startPoint;
            List<List<Coordinates>> coordsLists;
            int maxLength;

            this.reset();

            for (int x = 0; x < this.map.Length; x++) {
                for (int y = 0; y < this.map[x].Length; y++) {
                    startPoint = new Coordinates(x, y); // position to consider as 'start' of ship

                    if (cellStateMap.getState(startPoint) == cellState.SINK ||
                        cellStateMap.getState(startPoint) == cellState.IMPOSSIBLE ||
                        cellStateMap.getState(startPoint) == cellState.MISS) {
                        //A ship can not start on a sink/miss/0 probability cell
                        continue;
                    }

                    bool[] sides = new bool[] { true, false, false, true };//right & down 
                    coordsLists = ShotHelper.getSurrounding(startPoint, sides, this.shipLengths.Max() - 1);

                    foreach (List<Coordinates> coordList in coordsLists) {
                        maxLength = howManyFree(coordList) + 1; // how many sequential spaces are available in these coordinates
                        addProbabilitiy(startPoint, coordList, maxLength);
                    }
                }
            }
        }
    }
}
