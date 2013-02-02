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
        private static const int OPPONENT_ONE = 0;
        private static const int OPPONENT_TWO = 1;
        private static const int TURN_ENDGAME = 2;
        private BattleshipOpponent[] opponents; //Guaranteed to always be a 2-element array.
        private TimeSpan timePerGame; //Max time allowed in each method call.
        private int wins; //Amount of games to play
        private bool playOut; //True if the match ends when a player reaches wins. False if the match ends at (wins * 2 - 1) games played.
        private Size boardSize;
        private List<int> shipSizes;
        private Random rand; //The random number generator for this object.

        /**
         * <summary>Constructs a new BattleshipCompetition object using the given parameters</summary>
         * <param name="op1">A class implementing IBattleshipOpponent, playing in this competition</param>
         * <param name="op2">A class implementing IBattleshipOpponent, playing in this competition</param>
         * <param name="timePerGame">The maximum allowed time per function call to each class implementing IBattleshipOpponent</param>
         * <param name="wins">The number of wins a player must get before this match ends.</param>
         * <param name="playOut">True to play until a player reaches wins, false to play (wins * 2 - 1) times</param>
         * <param name="boardSize">The size of the battleship grid.</param>
         * <param name="shipSizes">An array containing x-amount of ships to be used in each game, and each having a specified length</param>
         */
        public BattleshipCompetition(IBattleshipOpponent op1, IBattleshipOpponent op2, TimeSpan timePerGame, int wins, bool playOut, Size boardSize, params int[] shipSizes)
        {
            if (op1 == null)
                throw new ArgumentNullException("op1");

            if (op2 == null)
                throw new ArgumentNullException("op2");

            if (timePerGame.TotalMilliseconds <= 0)
                throw new ArgumentOutOfRangeException("timePerGame");

            if (wins <= 0)
                throw new ArgumentOutOfRangeException("wins");

            if (boardSize.Width <= 2 || boardSize.Height <= 2)
                throw new ArgumentOutOfRangeException("boardSize");

            if (shipSizes == null || shipSizes.Length < 1)
                throw new ArgumentNullException("shipSizes");

            if (shipSizes.Where(s => s <= 0).Any())
                throw new ArgumentOutOfRangeException("shipSizes");

            if (shipSizes.Sum() >= (boardSize.Width * boardSize.Height))
                throw new ArgumentOutOfRangeException("shipSizes");

            opponents = new BattleshipOpponent[2];
            opponents[OPPONENT_ONE] = new BattleshipOpponent(op1, ref boardSize, ref timePerGame);
            opponents[OPPONENT_TWO] = new BattleshipOpponent(op2, ref boardSize, ref timePerGame);
            this.timePerGame = timePerGame;
            this.wins = wins;
            this.playOut = playOut;
            this.boardSize = boardSize;
            this.shipSizes = new List<int>(shipSizes);
            rand = new Random();
        }

        /**
         * <summary>Creates a list of new ships to be placed</summary>
         * <returns>A list of ships not placed on the board, defined to having a length specified by shipSizes</returns>
         */
        private List<Ship> GenerateNewShips()
        {
            return (from s in this.shipSizes
                    select new Ship(s)).ToList();
        }

        /**
         * <summary>Used to notify both players whether they won or lost.</summary>
         * <returns>Always returns true</returns>
         */
        private bool GameResultPush(BattleshipOpponent winner, BattleshipOpponent loser)
        {
            winner.GameWon();
            loser.GameLost();
            return true;
        }

        /**
         * <summary>Notifys each player that a new game is commencing</summary>
         * <returns>True if the game is over, false if the game is still on-going.</returns>
         */
        private bool NewGame()
        {
            for (int opCount = OPPONENT_ONE; opCount < TURN_ENDGAME; opCount++)
                if (opponents[opCount].NewGame())
                    return GameResultPush(opponents[~opCount], opponents[opCount]);
            return false;
        }

        /**
         * <summary>Ship placement mode for the players. Repeats until ships have been placed validly</summary>
         * <returns>True if the game is over. False if the game is on-going</returns>
         */
        private bool ShipPlacement()
        {
            for (int turn = rand.Next(TURN_ENDGAME); turn < TURN_ENDGAME; turn++)
            {
                if (opponents[turn].PlaceShips(GenerateNewShips()))
                    return GameResultPush(opponents[~turn], opponents[turn]);

                if (!opponents[turn].ShipsReady())
                    continue;
            }
            return false;
        }

        /**
         * <summary>The main game logic of battleship.</summary>
         * <returns>True if the game is over (the method never returns false)</returns>
         */
        private bool GamePlay()
        {
            for (int turn = rand.Next(TURN_ENDGAME); turn < TURN_ENDGAME; turn++)
            {
                Point shot = opponents[turn].ShootAt(opponents[~turn]);

                if (shot.X == BattleshipOpponent.LOSE_MAGIC_NUMBER && shot.Y == BattleshipOpponent.LOSE_MAGIC_NUMBER)
                    return GameResultPush(opponents[~turn], opponents[turn]);
                if (opponents[~turn].OpponentShot(shot))
                    return GameResultPush(opponents[turn], opponents[~turn]);

                Ship shipHit = (from s in opponents[~turn].ships
                                where s.IsAt(shot)
                                select s).SingleOrDefault();

                if (shipHit != null)
                    opponents[turn].ShotHit(shot, shipHit.IsSunk(opponents[turn].shots));
                else
                    opponents[turn].ShotMiss(shot);

                //Are there any ships left from the other opponent?
                if ((from s in opponents[~turn].ships where !s.IsSunk(opponents[turn].shots) select s).Any())
                    return GameResultPush(opponents[turn], opponents[~turn]);

                if (turn == OPPONENT_TWO)
                    turn = OPPONENT_ONE;
            }
            return true;
        }

        /**
         * <summary>Runs the match between two opponents.</summary>
         * <returns>A list of the two opponents with their scores attached.</returns>
         */
        public Dictionary<IBattleshipOpponent, int> RunCompetition()
        {
            int gamePlays = 0;

            //NOTICE:
            //   From now on, I will extensively use the "~" operator to denote "The opposing player".
            //   Also, many loops will simply call single expressions, in this case, I am going to
            //   leave out the begin and end brackets. (This is how I like to look at concise code,
            //   bring it up if you do not want this).

            //First send new matchup information to the opponents.
            for (int opCount = OPPONENT_ONE; opCount < TURN_ENDGAME; opCount++)
                opponents[opCount].NewMatch(opponents[~opCount].GetInfo());

            //This loop continues until this competition is complete.
            while (playOut ? opponents.Where(o => o.score >= wins).Any() : gamePlays++ < (wins * 2 - 1))
            {
                if (NewGame()) continue;
                if (ShipPlacement()) continue;
                if (GamePlay()) continue;
            }

            opponents[OPPONENT_ONE].MatchOver();
            opponents[OPPONENT_TWO].MatchOver();

            return opponents.ToDictionary(s => s.iOpponent, s => s.score);
        }
    }
}
