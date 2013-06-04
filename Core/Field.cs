using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MBC.Core
{

    /**
     * <summary>The Field class represents a battleship field. It contains battleship field information and
     * two ControllerInfo objects that contain information on the two controllers in the Field.
     * 
     * Accessing the individual ControllerInfo objects is made simple by using a value of 0 or 1. The order
     * of the ControllerInfo objects does not change, so constants can be used to access them.</summary>
     */
    [Serializable]
    public class Field
    {
        public Size gameSize;           //The size of the battlefield
        public Random fixedRandom;      //A Random object
        public List<int> shipSizes;     //A list of all the ships available on the battlefield
        public TimeSpan timeoutLimit;   //The time limit for this field
        private ControllerInfo[] info;  //A 2-element array that contains information for each controller.

        /**
         * <summary>Constructs a Battlefield object initialized with two opponents</summary>
         */
        public Field(IBattleshipController[] ibc)
        {
            info = new ControllerInfo[2];
            info[0] = new ControllerInfo(ibc[0].Name, ibc[0].Version);
            info[1] = new ControllerInfo(ibc[1].Name, ibc[1].Version);
        }

        /**
         * <summary>Copy constructor</summary>
         */
        public Field(Field copy)
        {
            info = (ControllerInfo[])copy.info.Clone();
            gameSize = copy.gameSize;
            fixedRandom = copy.fixedRandom;
            shipSizes = copy.shipSizes;
            timeoutLimit = copy.timeoutLimit;
        }

        /**
         * <summary>Gets the controllers in this field.</summary>
         */
        public ControllerInfo[] Controllers
        {
            get { return info; }
        }

        /**
         * <returns>The ConrollerInfo object at the specified index.</returns>
         */
        public ControllerInfo this[int i]
        {
            get { return info[i]; }
        }

        /**
         * <summary>Contains information related to the state of the battlefield for
         * each opponent.</summary>
         */
        [Serializable]
        public class ControllerInfo
        {
            public List<Point> shotsMade;
            public List<Ship> ships;
            public int score = 0;

            public string name;
            public Version version;

            public ControllerInfo(string ctrlName, Version ctrlVersion)
            {
                shotsMade = new List<Point>();
                ships = new List<Ship>();
                name = ctrlName;
                version = ctrlVersion;
            }
        }
    }
}
