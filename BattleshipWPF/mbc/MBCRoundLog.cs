using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;
using Battleship.mbc.accolades;

namespace Battleship
{
    /**
     * <summary>Keeps a log of a battleship competition round.</summary>
     */
    public class RoundLog
    {
        private List<RoundActivity> actions = new List<RoundActivity>();    //A list of RoundActivity's
        private List<RoundAccolade> accolades = new List<RoundAccolade>();   //A list of accolades
        private int readCount = 0;
        private List<AccoladeProcessor> a_processors = new List<AccoladeProcessor>();

        /**
         * <summary>Initializes the action processors</summary>
         */
        public RoundLog()
        {
            a_processors.Add(new Comeback());
            a_processors.Add(new Domination());
            a_processors.Add(new Fast());
            a_processors.Add(new HeadToHead());
            a_processors.Add(new Intense());
            a_processors.Add(new Slow());
        }

        /**
         * <summary>Starts counting the actions from zero again</summary>
         */
        public void ResetIterator()
        {
            readCount = 0;
        }

        /**
         * <returns>The activity at the specified action idx</returns>
         */
        public RoundActivity GetAt(int idx)
        {
            return actions.ElementAt(idx);
        }

        /**
         * <returns>The number of actions recorded in this round</returns>
         */
        public int GetActivityCount()
        {
            return actions.Count;
        }

        /**
         * <returns>The next action record to read</return>
         */
        public RoundActivity GetNext()
        {
            if (readCount >= actions.Count)
                return null;
            return actions.ElementAt(readCount++);
        }

        /**
         * <summary>Adds a constructed action to the list of activities recorded.
         * Once the round end action has been appended, do not use this function anymore.</summary>
         */
        public void PutAction(RoundActivity action)
        {
            foreach (AccoladeProcessor p in a_processors)
            {
                RoundAccolade a = p.Process(action);
                if (a != RoundAccolade.None)
                    action.AddAccolade(a);
            }
            actions.Add(action);
            if (action.action == RoundAction.RoundEnd)
                a_processors.Clear();
        }

        /**
         * <summary>Defines a round action record</summary>
         */
        public class RoundActivity
        {
            public static const string Reason_Timeout = "Timeout";  //Common string used for timeout messages
            public string activityInfo;                 //A message going with this action
            public IBattleshipOpponent opponent;        //The opponent this action relates to
            public RoundAction action;                  //The action
            public long timeElapsed;                    //The time it took for this opponent
            public List<RoundAccolade> accoladeTimelined;    //The accolade(s) that was generated from this action
            public Battlefield fieldState;              //The state of the battlefield at this action.

            public RoundActivity(IBattleshipOpponent op, RoundAction a)
            {
                init(null, op, a, 0, null);
            }

            public RoundActivity(string info, IBattleshipOpponent op, RoundAction a)
            {
                init(info, op, a, 0, null);
            }

            public RoundActivity(string info, IBattleshipOpponent op, RoundAction a, long time)
            {
                init(info, op, a, time, null);
            }

            public RoundActivity(string info, IBattleshipOpponent op, RoundAction a, long time, Battlefield state)
            {
                init(info, op, a, time, null);
            }

            private void init(string info, IBattleshipOpponent op, RoundAction a, long time, Battlefield state)
            {
                accoladeTimelined = new List<RoundAccolade>();
                activityInfo = info;
                opponent = op;
                action = a;
                timeElapsed = time;
                fieldState = new Battlefield(state);
            }

            /**
             * <summary>Adds the accolade generated from this activity</summary>
             */
            public void AddAccolade(RoundAccolade acc)
            {
                accoladeTimelined.Add(acc);
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
