namespace Battleship
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Threading;

    /**
     * <summary>A BattleshipCompetition object contains game logic for a game of battleship.
     * BattleshipCompetition puts two classes, implementing IBattleshipOpponent, into this game.</summary>
     */
    public class BattleshipCompetition
    {
        private BattleshipOpponent[] opponents;     //Guaranteed to always be a 2-element array.
        private BattleshipConfig config;            //Configuration used for this competition
        private Battlefield fieldInfo;              //Contains field information
        private List<RoundLog> roundList;
        private RoundLog roundLogger;               //The current RoundLog for the current round

        private Thread compThread;                  //The thread used for tasking this competition 
        private bool isRunning = false;             //Should be false whenever rounds should stop running.

        private BattleshipOpponent turn;            //The current opponent's turn

        private delegate void RndTick();
        public event RndTick RoundTurnEndEvent;

        private delegate void RndEnd();
        public event RndEnd RoundEndEvent;

        private delegate void MatchEnd();
        public event MatchEnd MatchEndEvent;

        /**
         * <summary>Constructs a new BattleshipCompetition object using a BattleshipConfig for configuration.</summary>
         */
        public BattleshipCompetition(IBattleshipOpponent[] ops, BattleshipConfig config)
        {
            init(ops, config, config.GetConfigValue<int>("field_random_seed", Environment.TickCount));
        }

        /**
         * <summary>Constructs a new BattleshipCompetition object using a BattleshipConfig for configuration.
         * Also, a seed value can be specified.</summary>
         */
        public BattleshipCompetition(IBattleshipOpponent[] ops, BattleshipConfig config, int seedNum)
        {
            init(ops, config, seedNum); 
        }

        /**
         * <summary>Called only by the constructor, does initialization of this class.</summary>
         */
        private void init(IBattleshipOpponent[] ops, BattleshipConfig conf, int seedNum)
        {
            config = conf;
            fieldInfo = new Battlefield(ops);
            fieldInfo.fixedRandom = new Random(seedNum);
            fieldInfo.gameSize = new Size(config.GetConfigValue<int>("field_width", 10), config.GetConfigValue<int>("field_height", 10));
            fieldInfo.shipSizes = config.GetConfigValueArray<int>("field_ship_sizes", "2,3,3,4,5");
            fieldInfo.timeoutLimit = new TimeSpan(0, 0, 0, config.GetConfigValue<int>("timeout_seconds", 0), config.GetConfigValue<int>("timeout_millis", 500));

            roundList = new List<RoundLog>();

            opponents = new BattleshipOpponent[2];
            for (int i = 0; i < ops.Count(); i++)
                opponents[i] = new BattleshipOpponent(ops[i], fieldInfo);
            foreach (BattleshipOpponent op in opponents)
                op.NewMatch(Opposite(op).ToString());

            compThread = new Thread(new ThreadStart(RunCompetition));
        }

        /**
         * <returns>The battlefield associated with this competition</returns>
         */
        public Battlefield GetBattlefield()
        {
            return fieldInfo;
        }

        /**
         * <returns>The number of rounds played, complete or incomplete</returns>
         */
        public int GetRoundCount()
        {
            return roundList.Count;
        }

        /**
         * <returns>The log for the round #idx</returns>
         */
        public RoundLog GetRoundLogAt(int idx)
        {
            lock(roundList) {
                if (idx < 0 || idx > roundList.Count)
                    return null;
                return roundList.ElementAt(idx);
            }
        }

        /**
         * <summary>Creates a list of new ships to be placed</summary>
         * <returns>A list of ships not placed on the board, defined to having a length specified by shipSizes</returns>
         */
        private List<Ship> GenerateNewShips()
        {
            return (from s in fieldInfo.shipSizes
                    select new Ship(s)).ToList();
        }

        /**
         * <summary>Used to notify both players whether they won or lost.</summary>
         * <returns>Always returns true</returns>
         */
        private bool GameResultPush(BattleshipOpponent winner, BattleshipOpponent loser, string reason)
        {
            winner.GameWon();
            loser.GameLost();
            roundLogger.PutAction(new RoundLog.RoundActivity(null, winner.iOpponent, RoundLog.RoundAction.Won));
            roundLogger.PutAction(new RoundLog.RoundActivity(null, loser.iOpponent, RoundLog.RoundAction.Lost));
            roundLogger.PutAction(new RoundLog.RoundActivity(reason, null, RoundLog.RoundAction.RoundEnd));
            return true;
        }

        /**
         * <returns>The opposing BattleshipOpponent relative to 'opponent'</returns>
         */
        private BattleshipOpponent Opposite(BattleshipOpponent opponent)
        {
            foreach (BattleshipOpponent op in opponents)
                if (opponent != op)
                    return op;
            return null;
        }

        /**
         * <summary>Notifys each player that a new game is commencing</summary>
         * <returns>True if the game is over, false if the game is still on-going.</returns>
         */
        private bool NewGame()
        {
            foreach (BattleshipOpponent op in opponents)
                if (op.NewGame())
                    return GameResultPush(Opposite(op), op, RoundLog.RoundActivity.Reason_Timeout);
            return false;
        }

        /**
         * <summary>Ship placement mode for the players. Repeats until ships have been placed validly</summary>
         * <returns>True if the game is over. False if the game is on-going</returns>
         */
        private bool ShipPlacement()
        {
            foreach (BattleshipOpponent op in opponents)
                do
                {
                    if (op.PlaceShips(GenerateNewShips()))
                        return GameResultPush(Opposite(op), op, RoundLog.RoundActivity.Reason_Timeout);
                    RoundLog.RoundActivity action = new RoundLog.RoundActivity(op.ShipsReady() ? "Ready" : "Invalid", op.iOpponent, RoundLog.RoundAction.ShipsPlaced);
                    action.fieldState = new Battlefield(fieldInfo);
                    roundLogger.PutAction(action);
                } while (!op.ShipsReady());
            return false;
        }

        /**
         * <summary>The main game logic of battleship.</summary>
         * <returns>True if the game is over (the method never returns false)</returns>
         */
        private bool GamePlay()
        {
            Point shot = turn.ShootAt(Opposite(turn));

            if (shot.X == BattleshipOpponent.LOSE_MAGIC_NUMBER && shot.Y == BattleshipOpponent.LOSE_MAGIC_NUMBER)
                return GameResultPush(Opposite(turn), turn, RoundLog.RoundActivity.Reason_Timeout);
            if (Opposite(turn).OpponentShot(shot))
                return GameResultPush(turn, Opposite(turn), RoundLog.RoundActivity.Reason_Timeout);

            Ship shipHit = Opposite(turn).GetShipAtPoint(shot);

            if (shipHit != null)
            {
                roundLogger.PutAction(new RoundLog.RoundActivity(shot.X + "," + shot.Y, turn.iOpponent, RoundLog.RoundAction.ShotAndHit, turn.GetTimeTaken()));

                bool sunk = shipHit.IsSunk(turn.GetFieldInfo().shotsMade);

                turn.ShotHit(shot, sunk);
                if (sunk)
                    roundLogger.PutAction(new RoundLog.RoundActivity(shot.X + "," + shot.Y, turn.iOpponent, RoundLog.RoundAction.ShipDestroyed, turn.GetTimeTaken()));
            }
            else
            {
                roundLogger.PutAction(new RoundLog.RoundActivity(shot.X + "," + shot.Y, turn.iOpponent, RoundLog.RoundAction.ShotAndMiss, turn.GetTimeTaken()));
                turn.ShotMiss(shot);
            }

            //Are there any ships left from the other opponent?
            if (!Opposite(turn).IsAlive(turn.GetFieldInfo().shotsMade))
                return GameResultPush(turn, Opposite(turn), null);
            return false;
        }

        /**
         * <summary>Part of manual round control. Clears the previous round if it exists
         * and starts a new round</summary>
         * <returns>True if the round is ongoing, false if it has been won</returns>
         */
        public bool NewRound()
        {
            lock (roundList)
            {
                if (roundLogger != null && roundLogger.GetAt(roundLogger.GetActivityCount()).action != RoundLog.RoundAction.RoundEnd)
                    roundLogger.PutAction(new RoundLog.RoundActivity("Round ended before win", null, RoundLog.RoundAction.RoundEnd));

                roundLogger = new RoundLog();
                roundList.Add(roundLogger);

                turn = opponents[fieldInfo.fixedRandom.Next(2)];
                roundLogger.PutAction(new RoundLog.RoundActivity(null, turn.iOpponent, RoundLog.RoundAction.RoundBegin));
            }

            return NewGame() || ShipPlacement();
        }

        /**
         * <summary>Part of manual round control. Runs a turn</summary>
         * <returns>True if the round is still ongoing, false if the round has been won</returns>
         */
        public bool RoundTurn()
        {
            bool res = GamePlay();
            turn = Opposite(turn);

            if (RoundTurnEndEvent != null)
                RoundTurnEndEvent();

            return res;
        }

        /**
         * <summary>Runs an entire round until an opponent wins</summary>
         */
        public void RunRound()
        {
            NewRound();
            while (!RoundTurn() && isRunning) ;
        }

        /**
         * <summary>Determines if the number of rounds specified has been reached</summary>
         * <returns>True if an opponent has reached the number of rounds</returns>
         */
        private bool RoundsReached(int rnds)
        {
            return opponents[0].GetFieldInfo().score >= rnds || opponents[1].GetFieldInfo().score >= rnds;
        }

        /**
         * <summary>Returns a dictionary linking the scores to the opponents.</summary>
         */
        public Dictionary<IBattleshipOpponent, int> GetScores()
        {
            return opponents.ToDictionary(s => s.iOpponent, s => s.GetFieldInfo().score);
        }

        /**
         * <summary>Runs a specified amount of rounds between two opponents.</summary>
         * <returns>A list of the two opponents with their scores attached.</returns>
         * <param name="playOut">If the amount of rounds specified is to be played out (ie.
         * total score between the two opponents) or not (ie. first score to rounds)</param>
         * <param name="rnds">The amount of rounds to play.</param>
         */
        public void RunRounds(int rnds, bool playOut)
        {
            isRunning = true;
            int gamePlays = 0;

            foreach (BattleshipOpponent op in opponents)
                op.NewMatch(Opposite(op).ToString());

            //This loop continues until this competition is complete.
            while (playOut ? !RoundsReached(rnds) : gamePlays++ < (rnds * 2 - 1) && isRunning)
            {
                RunRound();
                if (RoundEndEvent != null)
                    RoundEndEvent();
            }

            foreach (BattleshipOpponent op in opponents)
                op.MatchOver();
            if (MatchEndEvent != null)
                MatchEndEvent();
        }

        /**
         * <summary>Runs the match between two opponents.
         * Bases the amount of rounds to play and how to play them in the configuration.</summary>
         */
        public void RunCompetition()
        {
            RunRounds(config.GetConfigValue<int>("competition_rounds", 110),
                config.GetConfigValue<bool>("competition_mode_playout", false));
        }

        /**
         * <summary>Runs the match between two opponents in a separate thread using the configuration.</summary>
         */
        public void RunCompetitionThread()
        {
            if (isRunning)
                return;
            compThread.Start();
        }

        /**
         * <summary>Stops rounds from being played anymore.</summary>
         */
        public void StopCompetitionThread()
        {
            isRunning = false;
        }
    }
}
