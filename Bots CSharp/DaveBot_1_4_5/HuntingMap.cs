using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Controllers.DaveBot_1_4_5 {
    class HuntingMap : ProbabilityMap {
        protected const bool RESET_HUNTING_ON_SINK = true; // Wipes hunting mode memory when a ship is sunk, if false, 
        protected const bool REDUCE_PROBABILITY_ON_SINK = false; // Treats sinks as a miss and reduces hunting probability as such

        public HuntingMap(int size, int[] shipLengths, CellStateMap cellStateMap)
            : base(size, shipLengths, cellStateMap) {
                this.weighted = 50;
        }

        public void shipSunk(Coordinates coords) {
            if (RESET_HUNTING_ON_SINK) {
                //should not exist, probability reducing should be better
                this.reset();
            }

            // Pointless if hunting was just reset
            if (REDUCE_PROBABILITY_ON_SINK) {
                this.removeHuntingProbability(coords);
            }
        }

        public void addHuntingProbability(Coordinates coords) {
            List<List<Coordinates>> coordsLists;
            int maxLength;

            coordsLists = ShotHelper.getSurrounding(coords, shipLengths.Max() - 1);

            foreach (List<Coordinates> coordList in coordsLists) {
                maxLength = howManyFree(coordList) + 1; // how many sequential spaces are available in these coordinates
                addProbabilitiy(coords, coordList, maxLength);
            }
        }

        public void removeHuntingProbability(Coordinates coords) {
            List<List<Coordinates>> coordsLists;
            int maxLength;

            coordsLists = ShotHelper.getSurrounding(coords, shipLengths.Max() - 1);

            foreach (List<Coordinates> coordList in coordsLists) {
                maxLength = howManyFree(coordList) + 1;
                removeProbabilitiy(coordList, maxLength);
            }
        }
    }
}
