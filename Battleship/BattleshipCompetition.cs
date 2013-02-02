namespace Battleship
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;

    /**
     * BattleshipCompetition:
     * 
     * No docs right now.. look at RunCompetition() for now.
     * 
     */
    public class BattleshipCompetition
    {
        private static const int OPPONENT_ONE = 0;
        private static const int OPPONENT_TWO = 1;
        private static const int TURN_ENDGAME = 2;
        private BattleshipOpponent[] opponents;
        private TimeSpan timePerGame;
        private int wins;
        private bool playOut;
        private Size boardSize;
        private List<int> shipSizes;
        private Random rand;

        public BattleshipCompetition(IBattleshipOpponent op1, IBattleshipOpponent op2, TimeSpan timePerGame, int wins, bool playOut, Size boardSize, params int[] shipSizes)
        {
            if (op1 == null)
            {
                throw new ArgumentNullException("op1");
            }

            if (op2 == null)
            {
                throw new ArgumentNullException("op2");
            }

            if (timePerGame.TotalMilliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException("timePerGame");
            }

            if (wins <= 0)
            {
                throw new ArgumentOutOfRangeException("wins");
            }

            if (boardSize.Width <= 2 || boardSize.Height <= 2)
            {
                throw new ArgumentOutOfRangeException("boardSize");
            }

            if (shipSizes == null || shipSizes.Length < 1)
            {
                throw new ArgumentNullException("shipSizes");
            }

            if (shipSizes.Where(s => s <= 0).Any())
            {
                throw new ArgumentOutOfRangeException("shipSizes");
            }

            if (shipSizes.Sum() >= (boardSize.Width * boardSize.Height))
            {
                throw new ArgumentOutOfRangeException("shipSizes");
            }

            opponents = new BattleshipOpponent[2];
            opponents[0] = new BattleshipOpponent(op1, ref boardSize, ref timePerGame);
            opponents[1] = new BattleshipOpponent(op2, ref boardSize, ref timePerGame);
            this.timePerGame = timePerGame;
            this.wins = wins;
            this.playOut = playOut;
            this.boardSize = boardSize;
            this.shipSizes = new List<int>(shipSizes);
            rand = new Random();
        }

        private List<Ship> generateNewShips()
        {
            return (from s in this.shipSizes
                    select new Ship(s)).ToList();
        }

        private bool GameResultPush(BattleshipOpponent winner, BattleshipOpponent loser)
        {
            winner.GameWon();
            loser.GameLost();
            return true;
        }

        private bool newGame()
        {
            for (int opCount = OPPONENT_ONE; opCount < TURN_ENDGAME; opCount++)
                if (opponents[opCount].NewGame())
                {
                    GameResultPush(opponents[~opCount], opponents[opCount]);
                    return true;
                }
            return false;
        }

        private bool shipPlacement()
        {
            for (int turn = rand.Next(TURN_ENDGAME); turn < TURN_ENDGAME; turn++)
            {
                if (opponents[turn].PlaceShips(generateNewShips()))
                    return GameResultPush(opponents[~turn], opponents[turn]);

                if (!opponents[turn].ShipsReady())
                    continue;
            }
            return false;
        }

        private bool gamePlay()
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
            return false;
        }

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
            while (gamePlays++ < wins)
            {
                if (newGame()) continue;
                if (shipPlacement()) continue;
                if (gamePlay()) continue;
            }

            opponents[OPPONENT_ONE].MatchOver();
            opponents[OPPONENT_TWO].MatchOver();

            return opponents.ToDictionary(s => s.iOpponent, s => s.score);
        }
    }
}
