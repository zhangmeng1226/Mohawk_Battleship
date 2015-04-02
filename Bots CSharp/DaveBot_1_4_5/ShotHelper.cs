using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Shared;

namespace MBC.Controllers.DaveBot_1_4_5 {
    static class ShotHelper {

        public static int MatchX;
        public static int MatchY;
        public static Coordinates getLeft(Coordinates coords) {
            return new Coordinates(coords.X - 1, coords.Y);
        }

        public static Coordinates getUp(Coordinates coords) {
            return new Coordinates(coords.X, coords.Y - 1);
        }

        public static Coordinates getRight(Coordinates coords) {
            return new Coordinates(coords.X + 1, coords.Y);
        }

        public static Coordinates getDown(Coordinates coords) {
            return new Coordinates(coords.X, coords.Y + 1);
        }

        public static List<Coordinates> getLeft(Coordinates coords, int howMany) {
            List<Coordinates> coordsArray = new List<Coordinates>();

            for (int i = 0; i < howMany; i++) {

                if (coords.X == 0) {
                    return coordsArray;
                } else {
                    Coordinates next = getLeft(coords);
                    coords = next;
                    coordsArray.Add(next);
                }

            }

            return coordsArray;
        }

        public static List<Coordinates> getUp(Coordinates coords, int howMany) {
            List<Coordinates> coordsArray = new List<Coordinates>();

            for (int i = 0; i < howMany; i++) {

                if (coords.Y == 0) {
                    return coordsArray;
                } else {
                    Coordinates next = getUp(coords);
                    coords = next;
                    coordsArray.Add(next);
                }
            }

            return coordsArray;
        }

        public static List<Coordinates> getRight(Coordinates coords, int howMany) {
            List<Coordinates> coordsArray = new List<Coordinates>();

            for (int i = 0; i < howMany; i++) {
                if (coords.X == MatchX) {
                    return coordsArray;
                } else {
                    Coordinates next = getRight(coords);
                    coords = next;
                    coordsArray.Add(next);
                }
            }

            return coordsArray;
        }

        public static List<Coordinates> getDown(Coordinates coords, int howMany) {
            //Coordinates[] coordsArray = new Coordinates[howMany];
            List<Coordinates> coordsArray = new List<Coordinates>();

            for (int i = 0; i < howMany; i++) {

                if (coords.Y == MatchY) {
                    return coordsArray;
                } else {
                    Coordinates next = getDown(coords);
                    coords = next;
                    coordsArray.Add(next);
                }
            }

            return coordsArray;
        }

        public static List<List<Coordinates>> getSurrounding(Coordinates coords, int howMany) {
            bool[] sides = new bool[] { true, true, true, true };//right,left,up,down
            return getSurrounding(coords, sides, howMany);
        }

        public static List<List<Coordinates>> getSurrounding(Coordinates coords, bool[] sides, int howMany) { //Gets all surrounding coordinates up to specified range

            // sides order is right,left,up,down
            List<List<Coordinates>> coordsLists = new List<List<Coordinates>>(); // A list of coordinate Lists 
            List<Coordinates> coordsList = new List<Coordinates>();

            if (sides[0]) {
                coordsList = (getRight(coords, howMany));
                coordsLists.Add(coordsList);
            }

            if (sides[1]) {
                coordsList = (getLeft(coords, howMany));
                coordsLists.Add(coordsList);
            }

            if (sides[2]) {
                coordsList = (getUp(coords, howMany));
                coordsLists.Add(coordsList);
            }

            if (sides[3]) {
                coordsList = (getDown(coords, howMany));
                coordsLists.Add(coordsList);
            }

            return coordsLists;
        }
    }
}
