using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MBC.Core
{
    public class Field
    {
        public Size gameSize;           //The size of the battlefield
        public Random fixedRandom;      //A Random object
        public List<int> shipSizes;     //A list of all the ships available on the battlefield
        public TimeSpan timeoutLimit;   //The time limit for this field
        private Dictionary<IBattleshipController, ControllerInfo> controllers;

        /**
         * <summary>Constructs a Battlefield object initialized with two opponents</summary>
         */
        public Field(IBattleshipController[] ibc)
        {
            controllers = new Dictionary<IBattleshipController, ControllerInfo>();
            foreach (IBattleshipController op in ibc)
                controllers[op] = new ControllerInfo();
        }

        /**
         * <returns>Opponent information for both opponents</returns>
         */
        public ControllerInfo[] GetInfo()
        {
            return controllers.Values.ToArray();
        }

        /**
         * <summary>Copy constructor</summary>
         */
        public Field(Field copy)
        {
            controllers = new Dictionary<IBattleshipController, ControllerInfo>();
            foreach (KeyValuePair<IBattleshipController, ControllerInfo> op in copy.controllers)
                controllers.Add(op.Key, op.Value);
            gameSize = copy.gameSize;
            fixedRandom = copy.fixedRandom;
            shipSizes = copy.shipSizes;
            timeoutLimit = copy.timeoutLimit;
        }

        /**
         * <returns>The opponent information for the field from the opponent</returns>
         */
        public ControllerInfo this[IBattleshipController i]
        {
            get { return controllers[i]; }
        }

        /**
         * <summary>Contains information related to the state of the battlefield for
         * each opponent</summary>
         */
        public class ControllerInfo
        {
            public List<Point> shotsMade;
            public List<Ship> ships;
            public int score = 0;
        }
    }
}
