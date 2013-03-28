namespace Battleship
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;

    /**
     * <summary>A BattleshipCompetition object contains game logic for a game of battleship.
     * BattleshipCompetition puts two classes, implementing IBattleshipOpponent, into this game.</summary>
     */
    public class BattleshipCompetition
    {
        private BattleshipOpponent[] opponents;  //Guaranteed to always be a 2-element array.
        private BattleshipConfig config;
        private Battlefield fieldInfo;
        private List<RoundLog> roundList;
        private RoundLog roundLogger;

        private BattleshipOpponent turn;

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
                op.NewMatch(Opposite(op).GetInfo());
        }

        public Battlefield GetBattlefield()
        {
            return fieldInfo;
        }

        public List<RoundLog> GetRoundList()
        {
            return roundList;
        }

        public RoundLog GetCurrentRoundLog()
        {
            return roundLogger;
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
        private bool GameResultPush(BattleshipOpponent winner, BattleshipOpponent loser)
        {
            roundLogger.PutAction(new RoundLog.RoundActivity(null, winner.iOpponent, RoundLog.RoundAction.Won));
            roundLogger.PutAction(new RoundLog.RoundActivity(null, loser.iOpponent, RoundLog.RoundAction.Lost));
            roundLogger.PutAction(new RoundLog.RoundActivity(null, null, RoundLog.RoundAction.RoundEnd));
            winner.GameWon();
            loser.GameLost();
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
                    return GameResultPush(Opposite(op), op);
            return false;
        }

        public BattleshipOpponent[] GetOpponentWrappers()
        {
            return opponents;
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
                        return GameResultPush(Opposite(op), op);
                    RoundLog.RoundActivity action = new RoundLog.RoundActivity(op.ShipsReady() ? "Ready" : "Invalid", op.iOpponent, RoundLog.RoundAction.ShipsPlaced);
                    action.fieldState = new Battlefield(fieldInfo);
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
                return GameResultPush(Opposite(turn), turn);
            if (Opposite(turn).OpponentShot(shot))
                return GameResultPush(turn, Opposite(turn));

            Ship shipHit = Opposite(turn).GetShipAtPoint(shot);

            if (shipHit != null)
            {
                roundLogger.PutAction(new RoundLog.RoundActivity(shot.X + "," + shot.Y, turn.iOpponent, RoundLog.RoundAction.ShotAndHit));

                bool sunk = shipHit.IsSunk(turn.GetFieldInfo().shotsMade);

                turn.ShotHit(shot, sunk);
                if (sunk)
                    roundLogger.PutAction(new RoundLog.RoundActivity(shot.X + "," + shot.Y, turn.iOpponent, RoundLog.RoundAction.ShipDestroyed));
            }
            else
            {
                roundLogger.PutAction(new RoundLog.RoundActivity(shot.X + "," + shot.Y, turn.iOpponent, RoundLog.RoundAction.ShotAndMiss));
                turn.ShotMiss(shot);
            }

            //Are there any ships left from the other opponent?
            if (!Opposite(turn).IsAlive(turn.GetFieldInfo().shotsMade))
                return GameResultPush(turn, Opposite(turn));
            return false;
        }

        public bool NewRound()
        {
            roundLogger = new RoundLog();
            roundList.Add(roundLogger);

            turn = opponents[fieldInfo.fixedRandom.Next(2)];
            roundLogger.PutAction(new RoundLog.RoundActivity(null, turn.iOpponent, RoundLog.RoundAction.RoundBegin));

            return NewGame() || ShipPlacement();
        }

        public bool RoundTick()
        {
            bool res = GamePlay();
            turn = Opposite(turn);

            return res;
        }

        public void RunRound()
        {
            NewRound();
            while (!RoundTick()) ;
        }

        private bool RoundsReached(int rnds)
        {
            return opponents[0].GetFieldInfo().score >= rnds || opponents[1].GetFieldInfo().score >= rnds;
        }

        public Dictionary<IBattleshipOpponent, int> RunRounds(int rnds, bool playOut)
        {
            int gamePlays = 0;

            //This loop continues until this competition is complete.
            while (playOut ? !RoundsReached(rnds) : gamePlays++ < (rnds * 2 - 1))
            {
                RunRound();
            }

            foreach (BattleshipOpponent op in opponents)
                op.MatchOver();

            return opponents.ToDictionary(s => s.iOpponent, s => s.GetFieldInfo().score);
        }

        /**
         * <summary>Runs the match between two opponents and records the most interesting rounds.</summary>
         * <returns>A list of the two opponents with their scores attached.</returns>
         */
        public Dictionary<IBattleshipOpponent, int> RunCompetition()
        {
            return RunRounds(config.GetConfigValue<int>("competition_rounds", 110),
                config.GetConfigValue<bool>("competition_mode_playout", false));
        }
    }
}
