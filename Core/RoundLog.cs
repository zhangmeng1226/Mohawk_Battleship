using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Diagnostics;
using MBC.Core.mbc.accolade;

namespace MBC.Core
{
    /**
     * <summary>Keeps a log of a battleship competition round. Most RoundLog objects will be constructed
     * from a Competition object.
     * 
     * Each RoundLog contains a log of all events in a competition round, in order.
     * 
     * Utilizing two methods in a RoundLog object will retrieve the RoundActivity objects that make
     * up the events in a competition round:
     * 
     *      GetActivityCount() - The number of activities in the RoundLog
     *      GetAt(int) - Gets an activity at the specified index.
     *      
     * See the documentation for the RoundActivity class for available information.
     * 
     * When loading a RoundLog from a file, use the method PutAction(RoundActivity) to store a
     * RoundActivity object to the RoundLog, while having accolades generated while doing so.
     * </summary>
     */
    [Serializable]
    public class RoundLog
    {

        /**
         * <summary>Gets the string representation of the given RoundAccolade enum.</summary>
         */
        public static string GetAccoladeStr(RoundAccolade acc)
        {
            return Enum.GetName(typeof(RoundAccolade), acc);
        }

        /**
         * <summary>Gets the string representation of the given RoundAction enum.</summary>
         */
        public static string GetActionStr(RoundAction act)
        {
            return Enum.GetName(typeof(RoundAction), act);
        }

        private List<RoundActivity> actions = new List<RoundActivity>();    //A list of RoundActivity's
        private List<RoundAccolade> accolades = new List<RoundAccolade>();   //A list of RoundAccolades accumulated.
        
        [NonSerialized]
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
         * <summary>A RoundActivity contains information about a single event in a round. Multiple
         * RoundActivity objects make up a RoundLog.
         * 
         * See the public member variable documentation to see the information that is stored in a RoundActivity.
         * </summary>
         */
        [Serializable]
        public class RoundActivity
        {
            public static string Reason_Timeout = "Timeout";  //Common string used for timeout messages
            public string activityInfo;                 //A message attached with this activity.
            public int ibc;        //The controller this action relates to (by Field idx). See constants in the Controller class.
            public RoundAction action;                  //The action
            public long timeElapsed;                    //The time it took for this controller.
            public List<RoundAccolade> accoladeTimelined;    //The accolade(s) that was generated from this action
            public Field fieldState;              //The state of the battlefield at this action.

            /**
             * <summary>The minimal constructor for a RoundActivity object.
             * Each RoundActivity requires that a Controller index and RoundAction be specified. Other parameters
             * are optional.
             * </summary>
             */
            public RoundActivity(int bc, RoundAction a)
            {
                init(null, bc, a, 0, null);
            }

            public RoundActivity(string info, int bc, RoundAction a)
            {
                init(info, bc, a, 0, null);
            }

            public RoundActivity(string info, int bc, RoundAction a, long time)
            {
                init(info, bc, a, time, null);
            }

            public RoundActivity(string info, int bc, RoundAction a, long time, Field state)
            {
                init(info, bc, a, time, state);
            }

            private void init(string info, int bc, RoundAction a, long time, Field state)
            {
                accoladeTimelined = new List<RoundAccolade>();
                activityInfo = info;
                ibc = bc;
                action = a;
                timeElapsed = time;
                fieldState = new Field(state);
            }

            /**
             * <summary>Adds an accolade associated with this RoundActivity. Not recommended for use outside
             * of the RoundLog object.</summary>
             */
            public void AddAccolade(RoundAccolade acc)
            {
                accoladeTimelined.Add(acc);
            }
        }

        /**
         * <summary>A RoundAction defines the type of event that has occurred in a single RoundActivity.
         * 
         * ShotAndMiss - A Controller has made a shot, but missed.
         * ShotAndHit - A Controller has made a shot and hit an opposing ship.
         * ShipDestroyed - A Controller has destroyed an opposing ship. Always accompanied by a ShotAndHit RoundActivity type.
         * RoundBegin - A round has begun.
         * RoundEnd - A round has ended.
         * ShipsPlaced - A Controller has placed their ships, valid or not.
         * Won - A Controller has won the round.
         * Lost - A Controller has lost the round.
         * </summary>
         */
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

        /**
         * <summary>A RoundAccolade defines various desired interests in a single RoundLog.
         * 
         * HeadToHead - Each Controller makes a hit one after another.
         * Domination - A Controller is clearly winning the round with a larger number of hits.
         * Comeback - A Controller has the lead after previously losing the round by a large number of hits.
         * Fast - The number of misses made by each controller is minimal, making the number of events that
         *        make up the RoundLog small.
         * Slow - The number of misses made by each controller is large, making the number of events that
         *        make up the RoundLog large.
         * Intense - There is little difference in the number of hits made by both Controllers at a time.
         * None - Nothing interesting.
         * </summary>
         */
        public enum RoundAccolade
        {
            HeadToHead,
            Domination,
            Comeback,
            Fast,
            Slow,
            Intense,
            None
        }
    }
}
