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
    public class MBCRoundLog
    {
        private List<RoundActivity> actions = new List<RoundActivity>();    //A list of RoundActivity's
        private List<RoundAccolade> accolades = new List<RoundAccolade>();   //A list of accolades
        private int readCount = 0;
        private List<AccoladeProcessor> a_processors = new List<AccoladeProcessor>();

        /**
         * <summary>Initializes the action processors</summary>
         */
        public MBCRoundLog()
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
            public static string Reason_Timeout = "Timeout";  //Common string used for timeout messages
            public string activityInfo;                 //A message going with this action
            public IBattleshipController ibc;        //The opponent this action relates to
            public RoundAction action;                  //The action
            public long timeElapsed;                    //The time it took for this opponent
            public List<RoundAccolade> accoladeTimelined;    //The accolade(s) that was generated from this action
            public MBCField fieldState;              //The state of the battlefield at this action.

            public RoundActivity(IBattleshipController bc, RoundAction a)
            {
                init(null, bc, a, 0, null);
            }

            public RoundActivity(string info, IBattleshipController bc, RoundAction a)
            {
                init(info, bc, a, 0, null);
            }

            public RoundActivity(string info, IBattleshipController bc, RoundAction a, long time)
            {
                init(info, bc, a, time, null);
            }

            public RoundActivity(string info, IBattleshipController bc, RoundAction a, long time, MBCField state)
            {
                init(info, bc, a, time, null);
            }

            private void init(string info, IBattleshipController bc, RoundAction a, long time, MBCField state)
            {
                accoladeTimelined = new List<RoundAccolade>();
                activityInfo = info;
                ibc = bc;
                action = a;
                timeElapsed = time;
                fieldState = new MBCField(state);
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
