using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Battleship
{
    public class RoundLog
    {
        private List<RoundActivity> actions = new List<RoundActivity>();
        public List<RoundAccolade> accolades = new List<RoundAccolade>();
        private int readCount = 0;
        private Stopwatch watch = new Stopwatch();

        public void ResetIterator()
        {
            readCount = 0;
        }

        public RoundActivity GetAt(int idx)
        {
            return actions.ElementAt(idx);
        }

        public RoundActivity GetNext()
        {
            if (readCount >= actions.Count)
                return null;
            return actions.ElementAt(readCount++);
        }

        public void PutAction(RoundActivity action)
        {
            action.timeElapsed = watch.ElapsedMilliseconds;
            watch.Restart();
            actions.Add(action);
        }

        public class RoundActivity
        {
            public string activityInfo;
            public IBattleshipOpponent opponent;
            public RoundAction action;
            public long timeElapsed;
            public RoundAccolade accoladeTimelined = RoundAccolade.None;
            public Battlefield fieldState;

            public RoundActivity(string info, IBattleshipOpponent op, RoundAction a)
            {
                activityInfo = info;
                opponent = op;
                action = a;
            }
        }

        public enum RoundAction
        {
            ShotAndMiss,
            ShotAndHit,
            ShipDestroyed,
            RoundBegin,
            RoundEnd,
            ShipsPlaced,
            Won,
            Lost
        }

        public enum RoundAccolade
        {
            HeadToHead,
            Domination,
            Comeback,
            Fast,
            Slow,
            Intense,
            Stupid,
            None
        }
    }
}
