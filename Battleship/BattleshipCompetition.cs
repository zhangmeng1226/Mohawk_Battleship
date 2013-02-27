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
        private bool playOut;
        private int rounds;
        private List<RoundLog> roundList;
        private RoundLog roundLogger;

        /**
         * <summary>Constructs a new BattleshipCompetition object using a BattleshipConfig for configuration.</summary>
         */
        public BattleshipCompetition(IBattleshipOpponent[] ops, BattleshipConfig config)
        {
            this.config = config;
            init(ops, 
                new TimeSpan(0, 0, 0, config.ConfigValue<int>("timeout_seconds", 0), config.ConfigValue<int>("timeout_millis", 500)),
                new Size(config.ConfigValue<int>("field_width", 10), config.ConfigValue<int>("field_height", 10)),
                config.ConfigValueArray<int>("field_ship_sizes", "2,3,3,4,5"),
                new Random(config.ConfigValue<int>("field_random_seed", Environment.TickCount)));
        }

        /**
         * <summary>Constructs a new BattleshipCompetition object using a BattleshipConfig for configuration.
         * Also, a seed value can be specified.</summary>
         */
        public BattleshipCompetition(IBattleshipOpponent[] ops, BattleshipConfig config, int seedNum)
        {
            this.config = config;
            init(ops,
                new TimeSpan(0, 0, 0, config.ConfigValue<int>("timeout_seconds", 0), config.ConfigValue<int>("timeout_millis", 500)),
                new Size(config.ConfigValue<int>("field_width", 10), config.ConfigValue<int>("field_height", 10)),
                config.ConfigValueArray<int>("field_ship_sizes", "2,3,3,4,5"),
                new Random(seedNum));
        }

        /**
         * <summary>Constructs a new BattleshipCompetition object using the given parameters</summary>
         * <param name="op1">A class implementing IBattleshipOpponent, playing in this competition</param>
         * <param name="op2">A class implementing IBattleshipOpponent, playing in this competition</param>
         * <param name="timePerGame">The maximum allowed time per function call to each class implementing IBattleshipOpponent</param>
         * <param name="boardSize">The size of the battleship grid.</param>
         * <param name="shipSizes">An array containing x-amount of ships to be used in each game, and each having a specified length</param>
         */
        public BattleshipCompetition(IBattleshipOpponent op1, IBattleshipOpponent op2, TimeSpan timePerGame, Size boardSize, params int[] shipSizes)
        {
            //Arguments should never be wrong, only maybe when developing.
#if DEBUG
            if (op1 == null)
                throw new ArgumentNullException("op1");

            if (op2 == null)
                throw new ArgumentNullException("op2");

            if (timePerGame.TotalMilliseconds <= 0)
                throw new ArgumentOutOfRangeException("timePerGame");

            if (boardSize.Width <= 2 || boardSize.Height <= 2)
                throw new ArgumentOutOfRangeException("boardSize");

            if (shipSizes == null || shipSizes.Length < 1)
                throw new ArgumentNullException("shipSizes");

            if (shipSizes.Where(s => s <= 0).Any())
                throw new ArgumentOutOfRangeException("shipSizes");

            if (shipSizes.Sum() >= (boardSize.Width * boardSize.Height))
                throw new ArgumentOutOfRangeException("shipSizes");
#endif
            init(new IBattleshipOpponent[2]{op1, op2}, timePerGame, boardSize, new List<int>(shipSizes), new Random());
        }

        private void init(IBattleshipOpponent[] ops, TimeSpan timePerGame, Size boardSize, List<int> ships, Random rand)
        {
            fieldInfo = new Battlefield(ops);
            fieldInfo.fixedRandom = rand;
            fieldInfo.gameSize = boardSize;
            fieldInfo.shipSizes = ships;
            fieldInfo.timeoutLimit = timePerGame;

            playOut = config.ConfigValue<bool>("competition_mode_playout", false);
            rounds = config.ConfigValue<int>("competition_rounds", 110);
            roundList = new List<RoundLog>();

            opponents = new BattleshipOpponent[2];
            for (int i = 0; i < ops.Count(); i++)
                opponents[i] = new BattleshipOpponent(ops[i], fieldInfo);
            foreach (BattleshipOpponent op in opponents)
                op.NewMatch(Opposite(op).GetInfo());
        }

        public List<RoundLog> GetRoundList()
        {
            return roundList;
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
        private bool GamePlay(BattleshipOpponent turn)
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

        /**
         * <summary>Runs the match between two opponents and records the most interesting rounds.</summary>
         * <returns>A list of the two opponents with their scores attached.</returns>
         */
        public Dictionary<IBattleshipOpponent, int> RunCompetition()
        {
            int gamePlays = 0;

            //This loop continues until this competition is complete.
            while (playOut ? opponents.Where(o => o.GetFieldInfo().score <= rounds).Any() : gamePlays++ < (rounds * 2 - 1))
            {
                roundLogger = new RoundLog();
                roundList.Add(roundLogger);

                BattleshipOpponent turn = opponents[fieldInfo.fixedRandom.Next(2)];
                roundLogger.PutAction(new RoundLog.RoundActivity(null, turn.iOpponent, RoundLog.RoundAction.RoundBegin));

                if (NewGame()) continue;
                if (ShipPlacement()) continue;
                while (!GamePlay(turn))
                    turn = Opposite(turn);
            }

            foreach (BattleshipOpponent op in opponents)
                op.MatchOver();

            return opponents.ToDictionary(s => s.iOpponent, s => s.GetFieldInfo().score);
        }
    }
}
