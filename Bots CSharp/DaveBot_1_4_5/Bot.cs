using System;
using MBC.Shared.Attributes;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using MBC.Shared.Util;
using MBC.Shared;
using MBC.Shared.Entities;
namespace MBC.Controllers.DaveBot_1_4_5 {
    enum cellState {
        UNKNOWN,
        HIT,
        MISS,
        IMPOSSIBLE,
        SINK
    }

    public class Bot {
        private Random rand;

        //private ShotList shotQueue;

        private int roundHitCounter;
        private int roundSinkCounter;

        private PlacementMap placementMap;
        private HuntingMap huntingMap;
        private CellStateMap cellStateMap;
        private HistoryMap historyMap;

        private int[] shipLengths;

        Player player;
        Match match;
        Controller2 controller;

        Shot shotThisTurn;
        bool resultThisTurn;

        public Bot(DaveBot_1_4_5_Interface controller) {
            this.controller = controller;
        }

        public void NewMatch() {
            this.player = this.controller.Player;
            this.match = this.player.Match;
            ShotHelper.MatchX = match.FieldSize.X - 1;
            ShotHelper.MatchY = match.FieldSize.Y - 1;

            this.shipLengths = this.match.StartingShips.Select(ship => ship.Length).ToArray();

            cellStateMap = new CellStateMap(match.FieldSize.X);

            historyMap = new HistoryMap(match.FieldSize.X, this.shipLengths, cellStateMap);
            placementMap = new PlacementMap(match.FieldSize.X, this.shipLengths, cellStateMap);
            huntingMap = new HuntingMap(match.FieldSize.X, this.shipLengths, cellStateMap);
            
        }

        public void NewRound() {
            //haha! LINQ nonsense working - set starting ships
            this.shipLengths = this.match.StartingShips.Select(ship => ship.Length).ToArray();

            cellStateMap.reset();
            huntingMap.reset();

            roundHitCounter = 0;
            roundSinkCounter = 0;

            historyMap.calculateAverage(); // This average persists across all rounds
            historyMap.calculateVariations();

            //Finally, the controller creates a random number generator into the "rand" field defined
            //in this class. It uses the tick count of the system as a seed.
            rand = match.Random;
            rand.Next();

            this.placeShips();
        }

        public void myTurnStarts() {
            placementMap.calcProbabilities();
            fire();
        }

        public void fire() {
            resultThisTurn = false; // will be used to store result of this shot, is set to true within shotHit
            Coordinates coords = pickTarget();

            huntingMap.setValue(coords, 0);

            shotThisTurn = this.player.Shoot(coords.X, coords.Y);

        }

        private Coordinates pickTarget() {
            ProbabilityMap targetMap;

            targetMap = placementMap + huntingMap;

            for (int x = 0; x < placementMap.map.Length; x++) {
                for (int y = 0; y < placementMap.map[x].Length; y++) {
                    if (cellStateMap.getState(x, y) != cellState.UNKNOWN) {
                        continue;
                    }

                    int value = targetMap.getValue(x, y);

                    if (value == 0) {
                        cellStateMap.setState(x, y, cellState.IMPOSSIBLE); // No chance of ship being here, remove from consideration for future passes
                        continue;
                    }

                    if (historyMap.average > 0) {
                        double variation = historyMap.getVariation(x, y);

                        if (variation < 1) {
                            continue;
                        }

                        // This spot has hit targets slightly more than average, let's factor that in
                        targetMap.setValue(x, y, (int)(value * variation));
                    }
                }
            }

            return targetMap.getHighest();// new Coordinates(maxX, maxY);
        }

        public void shipHit(Coordinates coords, bool sunk) {
            resultThisTurn = true;
            roundHitCounter++;
            historyMap.increment(coords);

            if (sunk == true) {
                shipSunk(coords);
                return;
            }

            cellStateMap.setState(coords, cellState.HIT);
            huntingMap.addHuntingProbability(coords);
        }

        public void shipSunk(Coordinates coords) {
            // Updating probability after non-sinking hits falsely calculates surrounding cells as lower odds
            cellStateMap.setState(coords, cellState.SINK);
            roundSinkCounter++;
            evaluateShipStatus();

            huntingMap.shipSunk(coords);
        }

        public void ShotMiss() {
            Coordinates coords = shotThisTurn.Coordinates;

            cellStateMap.setState(coords, cellState.MISS);

            //huntingMap.removeHuntingProbability(coords); //pointless until a devise a smart way to only remove in the direction attempted
        }

        public void myTurnEnds() {
            if (!resultThisTurn) {
                this.ShotMiss();
            }
        }

        private void placeShips() {
            while (!ShipList.AreShipsPlaced(this.player.Ships)) {
                ShipList.PlaceShip(this.player.Ships, RandomCoordinates(), RandomShipOrientation());
            }
        }

        private Coordinates RandomCoordinates() {
            var xCoord = rand.Next(this.match.FieldSize.X);

            var yCoord = rand.Next(this.match.FieldSize.Y);

            return new Coordinates(xCoord, yCoord);
        }

        private ShipOrientation RandomShipOrientation() {
            var orientations = new ShipOrientation[] { ShipOrientation.Horizontal, ShipOrientation.Vertical };

            return orientations[rand.Next(2)];
        }

        private void evaluateShipStatus() {
            //2 length is gone
            if (roundSinkCounter == 1 && roundHitCounter == 2) {
                shipLengths[0] = 100;
            }

            // one 3 length is gone
            if (roundSinkCounter == 1 && roundHitCounter == 3) {
                shipLengths[1] = 100;
            }

            // 4 length is gone
            if (roundSinkCounter == 1 && roundHitCounter == 4) {
                shipLengths[3] = 100;
            }

            // 5 length is gone
            if (roundSinkCounter == 1 && roundHitCounter == 5) {
                shipLengths[4] = 100;
            }

            // 3 and 2 length gone
            if (roundSinkCounter == 2 && roundHitCounter == 5) {
                shipLengths[0] = 100;
                shipLengths[1] = 100;
            }

            //  5 & 3
            if (roundSinkCounter == 2 && roundHitCounter == 8) {
                shipLengths[1] = 100;
                shipLengths[4] = 100;
            }

            //  5 & 4
            if (roundSinkCounter == 2 && roundHitCounter == 9) {
                shipLengths[3] = 100;
                shipLengths[4] = 100;
            }

            //  2,3,3
            if (roundSinkCounter == 3 && roundHitCounter == 8) {
                shipLengths[0] = 100;
                shipLengths[1] = 100;
                shipLengths[2] = 100;
            }

            //  2,3,4  
            if (roundSinkCounter == 3 && roundHitCounter == 9) {
                shipLengths[0] = 100;
                shipLengths[1] = 100;
                shipLengths[3] = 100;
            }

            //  3,4,5
            if (roundSinkCounter == 3 && roundHitCounter == 12) {
                shipLengths[1] = 100;
                shipLengths[3] = 100;
                shipLengths[4] = 100;
            }
        }
    } // End Class
} // End namespace