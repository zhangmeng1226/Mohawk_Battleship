using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Battleship
{
    public class Battlefield
    {
        public Size gameSize;
        public Random fixedRandom;
        public List<int> shipSizes;
        public TimeSpan timeoutLimit;
        private Dictionary<IBattleshipOpponent, OpponentInfo> opponents;

        public Battlefield(IBattleshipOpponent[] ops)
        {
            opponents = new Dictionary<IBattleshipOpponent, OpponentInfo>();
            foreach (IBattleshipOpponent op in ops)
                opponents[op] = new OpponentInfo();
        }

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

        public OpponentInfo this[IBattleshipOpponent i]
        {
            get { return opponents[i]; }
        }

        public class OpponentInfo
        {
            public List<Point> shotsMade;
            public List<Ship> ships;
            public int score = 0;
        }
    }
}
