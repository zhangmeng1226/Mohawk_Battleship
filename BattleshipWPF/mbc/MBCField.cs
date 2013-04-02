using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Battleship
{
    public class Battlefield
    {
        public Size gameSize;           //The size of the battlefield
        public Random fixedRandom;      //A Random object
        public List<int> shipSizes;     //A list of all the ships available on the battlefield
        public TimeSpan timeoutLimit;   //The time limit for this field
        private Dictionary<IBattleshipOpponent, OpponentInfo> opponents;

        /**
         * <summary>Constructs a Battlefield object initialized with two opponents</summary>
         */
        public Battlefield(IBattleshipOpponent[] ops)
        {
            opponents = new Dictionary<IBattleshipOpponent, OpponentInfo>();
            foreach (IBattleshipOpponent op in ops)
                opponents[op] = new OpponentInfo();
        }

        /**
         * <returns>Opponent information for both opponents</returns>
         */
        public OpponentInfo[] GetInfo()
        {
            return opponents.Values.ToArray();
        }

        /**
         * <summary>Copy constructor</summary>
         */
        public Battlefield(Battlefield copy)
        {
            opponents = new Dictionary<IBattleshipOpponent, OpponentInfo>();
            foreach (KeyValuePair<IBattleshipOpponent, OpponentInfo> op in copy.opponents)
                opponents.Add(op.Key, op.Value);
            gameSize = copy.gameSize;
            fixedRandom = copy.fixedRandom;
            shipSizes = copy.shipSizes;
            timeoutLimit = copy.timeoutLimit;
        }

        /**
         * <returns>The opponent information for the field from the opponent</returns>
         */
        public OpponentInfo this[IBattleshipOpponent i]
        {
            get { return opponents[i]; }
        }

        /**
         * <summary>Contains information related to the state of the battlefield for
         * each opponent</summary>
         */
        public class OpponentInfo
        {
            public List<Point> shotsMade;
            public List<Ship> ships;
            public int score = 0;
        }
    }
}
