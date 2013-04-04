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
    public class MBCCompetition
    {
        private MBCController[] controllers;     //Guaranteed to always be a 2-element array.
        private BattleshipConfig config;            //Configuration used for this competition
        private MBCField fieldInfo;              //Contains field information
        private List<MBCRoundLog> roundList;
        private MBCRoundLog roundLogger;               //The current RoundLog for the current round

        private Thread compThread;                  //The thread used for tasking this competition 
        private bool isRunning = false;             //Should be false whenever rounds should stop running.

        private MBCController turn;            //The current opponent's turn

        public delegate void RndTick();
        public event RndTick RoundTurnEndEvent;

        public delegate void RndEnd();
        public event RndEnd RoundEndEvent;

        public delegate void MatchEnd();
        public event MatchEnd MatchEndEvent;

        /**
         * <summary>Constructs a new BattleshipCompetition object using a BattleshipConfig for configuration.</summary>
         */
        public MBCCompetition(IBattleshipController[] ibc, BattleshipConfig config)
        {
            init(ibc, config, config.GetConfigValue<int>("field_random_seed", Environment.TickCount));
        }

        /**
         * <summary>Constructs a new BattleshipCompetition object using a BattleshipConfig for configuration.
         * Also, a seed value can be specified.</summary>
         */
        public MBCCompetition(IBattleshipController[] ibc, BattleshipConfig config, int seedNum)
        {
            init(ibc, config, seedNum); 
        }

        /**
         * <summary>Called only by the constructor, does initialization of this class.</summary>
         */
        private void init(IBattleshipController[] ibc, BattleshipConfig conf, int seedNum)
        {
            config = conf;
            fieldInfo = new MBCField(ibc);
            fieldInfo.fixedRandom = new Random(seedNum);
            fieldInfo.gameSize = new Size(config.GetConfigValue<int>("field_width", 10), config.GetConfigValue<int>("field_height", 10));
            fieldInfo.shipSizes = config.GetConfigValueArray<int>("field_ship_sizes", "2,3,3,4,5");
            fieldInfo.timeoutLimit = new TimeSpan(0, 0, 0, config.GetConfigValue<int>("timeout_seconds", 0), config.GetConfigValue<int>("timeout_millis", 500));

            roundList = new List<MBCRoundLog>();

            controllers = new MBCController[2];
            for (int i = 0; i < ibc.Count(); i++)
                controllers[i] = new MBCController(ibc[i], fieldInfo);
            foreach (MBCController op in controllers)
                op.NewMatch(Opponent(op).ToString());

            compThread = new Thread(new ThreadStart(RunCompetition));
        }

        /**
         * <returns>The battlefield associated with this competition</returns>
         */
        public MBCField GetBattlefield()
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
        public MBCRoundLog GetRoundLogAt(int idx)
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
        private bool GameResultPush(MBCController winner, MBCController loser, string reason)
        {
            winner.GameWon();
            loser.GameLost();
            roundLogger.PutAction(new MBCRoundLog.RoundActivity(null, winner.ibc, MBCRoundLog.RoundAction.Won));
            roundLogger.PutAction(new MBCRoundLog.RoundActivity(null, loser.ibc, MBCRoundLog.RoundAction.Lost));
            roundLogger.PutAction(new MBCRoundLog.RoundActivity(reason, null, MBCRoundLog.RoundAction.RoundEnd));
            return true;
        }

        /**
         * <returns>The opposing BattleshipOpponent relative to 'ibc'</returns>
         */
        private MBCController Opponent(MBCController ibc)
        {
            foreach (MBCController bc in controllers)
                if (ibc != bc)
                    return bc;
            return null;
        }

        /**
         * <summary>Notifys each player that a new game is commencing</summary>
         * <returns>True if the game is over, false if the game is still on-going.</returns>
         */
        private bool NewGame()
        {
            foreach (MBCController bc in controllers)
                if (bc.NewGame())
                    return GameResultPush(Opponent(bc), bc, MBCRoundLog.RoundActivity.Reason_Timeout);
            return false;
        }

        /**
         * <summary>Ship placement mode for the players. Repeats until ships have been placed validly</summary>
         * <returns>True if the game is over. False if the game is on-going</returns>
         */
        private bool ShipPlacement()
        {
            foreach (MBCController bc in controllers)
                do
                {
                    if (bc.PlaceShips(GenerateNewShips()))
                        return GameResultPush(Opponent(bc), bc, MBCRoundLog.RoundActivity.Reason_Timeout);
                    MBCRoundLog.RoundActivity action = new MBCRoundLog.RoundActivity(bc.ShipsReady() ? "Ready" : "Invalid", bc.ibc, MBCRoundLog.RoundAction.ShipsPlaced);
                    action.fieldState = new MBCField(fieldInfo);
                    roundLogger.PutAction(action);
                } while (!bc.ShipsReady());
            return false;
        }

        /**
         * <summary>The main game logic of battleship.</summary>
         * <returns>True if the game is over (the method never returns false)</returns>
         */
        private bool GamePlay()
        {
            Point shot = turn.ShootAt(Opponent(turn));

            if (shot.X == MBCController.LOSE_MAGIC_NUMBER && shot.Y == MBCController.LOSE_MAGIC_NUMBER)
                return GameResultPush(Opponent(turn), turn, MBCRoundLog.RoundActivity.Reason_Timeout);
            if (Opponent(turn).OpponentShot(shot))
                return GameResultPush(turn, Opponent(turn), MBCRoundLog.RoundActivity.Reason_Timeout);

            Ship shipHit = Opponent(turn).GetShipAtPoint(shot);

            if (shipHit != null)
            {
                roundLogger.PutAction(new MBCRoundLog.RoundActivity(shot.X + "," + shot.Y, turn.ibc, MBCRoundLog.RoundAction.ShotAndHit, turn.GetTimeTaken()));

                bool sunk = shipHit.IsSunk(turn.GetFieldInfo().shotsMade);

                turn.ShotHit(shot, sunk);
                if (sunk)
                    roundLogger.PutAction(new MBCRoundLog.RoundActivity(shot.X + "," + shot.Y, turn.ibc, MBCRoundLog.RoundAction.ShipDestroyed, turn.GetTimeTaken()));
            }
            else
            {
                roundLogger.PutAction(new MBCRoundLog.RoundActivity(shot.X + "," + shot.Y, turn.ibc, MBCRoundLog.RoundAction.ShotAndMiss, turn.GetTimeTaken()));
                turn.ShotMiss(shot);
            }

            //Are there any ships left from the other opponent?
            if (!Opponent(turn).IsAlive(turn.GetFieldInfo().shotsMade))
                return GameResultPush(turn, Opponent(turn), null);
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
                if (roundLogger != null && roundLogger.GetAt(roundLogger.GetActivityCount()).action != MBCRoundLog.RoundAction.RoundEnd)
                    roundLogger.PutAction(new MBCRoundLog.RoundActivity("Round ended before win", null, MBCRoundLog.RoundAction.RoundEnd));

                roundLogger = new MBCRoundLog();
                roundList.Add(roundLogger);

                turn = controllers[fieldInfo.fixedRandom.Next(2)];
                roundLogger.PutAction(new MBCRoundLog.RoundActivity(null, turn.ibc, MBCRoundLog.RoundAction.RoundBegin));
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
            turn = Opponent(turn);

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
            return controllers[0].GetFieldInfo().score >= rnds || controllers[1].GetFieldInfo().score >= rnds;
        }

        /**
         * <summary>Returns a dictionary linking the scores to the opponents.</summary>
         */
        public Dictionary<IBattleshipController, int> GetScores()
        {
            return controllers.ToDictionary(s => s.ibc, s => s.GetFieldInfo().score);
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

            foreach (MBCController bc in controllers)
                bc.NewMatch(Opponent(bc).ToString());

            //This loop continues until this competition is complete.
            while (playOut ? !RoundsReached(rnds) : gamePlays++ < (rnds * 2 - 1) && isRunning)
            {
                RunRound();
                if (RoundEndEvent != null)
                    RoundEndEvent();
            }

            foreach (MBCController bc in controllers)
                bc.MatchOver();
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
