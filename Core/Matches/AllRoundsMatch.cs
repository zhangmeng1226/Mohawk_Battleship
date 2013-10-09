using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;

namespace MBC.Core.Matches
{
    public class AllRoundsMatch : ActiveMatch
    {
        protected GameLogic currentGame;

        public AllRoundsMatch()
            : this(Configuration.Global)
        {
        }

        public AllRoundsMatch(Configuration conf)
            : base(conf)
        {
            IsRunning = false;
        }

        public override bool Ended
        {
            get
            {
                return currentGame.ID >= CompiledConfig.NumberOfRounds;
            }
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        public override void Play()
        {
            if (IsRunning)
            {
                return;
            }
            if (currentGame == null)
            {
                currentGame = CreateNewRound(0);
            }
            IsRunning = true;
            while (IsRunning)
            {
                if (Ended)
                {
                    IsRunning = false;
                    return;
                }
                if (currentGame.Ended)
                {
                    currentGame = CreateNewRound(currentGame.ID + 1);
                }
                currentGame.Play();
            }
        }

        public override void Stop()
        {
            IsRunning = false;
            currentGame.Stop();
        }

        private GameLogic CreateNewRound(IDNumber roundID)
        {
            var game = new ClassicGame(roundID, this);
            foreach (var ctrl in Controllers)
            {
                ctrl.Value.NewRound();
            }
            return game;
        }
    }
}