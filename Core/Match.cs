using MBC.Core.Events;
using MBC.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MBC.Core
{
    public delegate void MatchEventHandler(MatchEvent ev);

    /// <summary>
    /// Provides information about a matchup of controllers. The Match class sets and uses the following
    /// Configuration keys:
    /// <list type="bullet">    
    ///     <item><b>mbc_match_rounds_mode</b> - "all", "play to", or "best of".
    ///     <list type="bullet">
    ///         <item><b>all</b> - Runs N number of Rounds.</item>
    ///         <item><b>first to</b> - Runs rounds until a controller has reached N number of wins.</item>
    ///     </list>
    ///     </item>
    ///     <item><b>mbc_match_rounds</b> - The number of rounds.</item>
    /// </list>
    /// Match objects internally orders each controller in the same order they are received in the constructors.
    /// Keep this in mind when accessing lists that are associated with the controllers in a Match or Round.
    /// </summary>
    [Configuration("mbc_match_rounds_mode", "all")]
    [Configuration("mbc_match_rounds", 100)]
    public class Match
    {
        public event MatchEventHandler MatchEvent;

        private MatchInfo info;
        private PlayMode roundPlay;

        private List<ControllerUser> participants;

        private int targetRounds;
        private int roundIteration;
        private List<Round> roundList;
        private Round currentRound;

        private Thread runningThread;
        private AutoResetEvent sleepHandle;
        private bool isRunning;
        private int delay;

        public Match(params ControllerInformation[] controllers)
        {
            Init(Configuration.Global, controllers);
        }

        public Match(Configuration conf, params ControllerInformation[] controllers)
        {
            Init(conf, controllers);
        }

        private void Init(Configuration conf, params ControllerInformation[] controllersToLoad)
        {
            //Get the game info from the Configuration.
            //Expecting an exception to be thrown here if a controller isn't compatible with the configured game mode.
            info = new CMatchInfo(conf);

            //Register the given controllers
            RegisterControllers(controllersToLoad);

            //Configuration setting for the number of rounds this Match will perform.
            roundList = new List<Round>();
            targetRounds = conf.GetValue<int>("mbc_match_rounds");
            roundIteration = -1;

            //Make a thread for this Match.
            runningThread = new Thread(PlayLoop);
            sleepHandle = new AutoResetEvent(false);
            isRunning = false;
            delay = 0;

            //Configuration setting for round playing behaviour, given a number of rounds.
            switch (conf.GetValue<string>("mbc_match_rounds_mode"))
            {
                case "all":
                    roundPlay = PlayMode.AllRounds;
                    break;
                case "first to":
                    roundPlay = PlayMode.FirstTo;
                    break;
                default:
                    conf.SetValue<string>("mbc_match_rounds_mode", "all");
                    break;
            }
        }

        private void DetermineOpponents(ControllerRegister registrant, IEnumerable<ControllerInformation> registrants)
        {
            registrant.Opponents = new List<ControllerID>();
            for (var oppId = 0; oppId < registrants.Count(); oppId++)
            {
                if (registrant.ID != oppId)
                {
                    registrant.Opponents.Add(oppId);
                }
            }
        }

        private void RegisterControllers(IEnumerable<ControllerInformation> registrants)
        {
            participants = new List<ControllerUser>();
            for (var id = 0; id < registrants.Count(); id++)
            {
                ControllerRegister newRegistration = new ControllerRegister(info, id);
                participants.Add(new ControllerUser(registrants.ElementAt(id), newRegistration));
            }
        }

        public List<ControllerRegister> GetRegistered()
        {
            var registered = new List<ControllerRegister>();
            foreach (var user in participants)
            {
                registered.Add(user.Register);
            }
            return registered;
        }

        public bool IsRoundTargetReached()
        {
            if (roundPlay == PlayMode.AllRounds)
            {
                return roundIteration == targetRounds;
            }
            else if (roundPlay == PlayMode.FirstTo)
            {
                foreach (var registrant in participants)
                {
                    if (registrant.Register.Score == targetRounds)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SetNewRound()
        {
            if ((info.GameMode & GameMode.Classic) == GameMode.Classic)
            {
                currentRound = new ClassicRound(info, participants);
                roundList.Add(currentRound);
                return true;
            }
            else
            {
                End();
                return false;
            }
        }

        /// <summary>
        /// Progresses the Match. Sets up a new Round when there is no Round in progress. Progresses the
        /// current Round in progress. Continues this until the criteria set by the PlayMode has been met.
        /// </summary>
        /// <returns>true if the Match is still in progress, false when the PlayMode criteria has been met.</returns>
        private bool Progress()
        {
            if (IsRoundTargetReached())
            {
                return false;
            }

            if (currentRound == null)
            {
                if (!SetNewRound())
                {
                    return false;
                }
            }
            if (!currentRound.Progress())
            {
                currentRound = null;
            }
            return true;
        }

        private void PlayLoop()
        {
            while (isRunning)
            {
                if (!Progress())
                {
                    break;
                }
                if (delay != 0)
                {
                    sleepHandle.WaitOne(delay);
                }
            }
            isRunning = false;
        }

        public bool Step()
        {
            if (isRunning)
            {
                return true;
            }
            return Progress();
        }

        public void Play()
        {
            MakeEvent(new MatchBeginEvent(this));
            isRunning = true;
            sleepHandle.Reset();
            runningThread.Start();
        }

        public void Stop()
        {
            MakeEvent(new MatchStopEvent(this));
            isRunning = false;
            sleepHandle.Set();
        }

        public void End()
        {
            Stop();
            MakeEvent(new MatchEndEvent(this));
            roundIteration = targetRounds;
        }

        private void MakeEvent(MatchEvent ev)
        {
            if (MatchEvent != null)
            {
                MatchEvent(ev);
            }
        }

        /// <summary>
        /// Gets the behaviour of rounds being played in this Match.
        /// </summary>
        public PlayMode RoundPlayMode
        {
            get
            {
                return roundPlay;
            }
        }

        /// <summary>
        /// Gets the MatchInfo object associated with this Match that describes the behaviour of the games
        /// by this Match.
        /// </summary>
        public MatchInfo Info
        {
            get
            {
                return info;
            }
        }

        /// <summary>
        /// Gets the number of rounds set in this Match.
        /// </summary>
        public int NumberOfRounds
        {
            get
            {
                return targetRounds;
            }
        }

        public List<Round> Rounds
        {
            get
            {
                return roundList;
            }
        }

        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                if (value <= 0)
                {
                    delay = 0;
                }
                else
                {
                    delay = value;
                }
            }
        }

        /// <summary>
        /// This enumerator identifies the behaviour of rounds being played out, given a number.
        /// </summary>
        public enum PlayMode
        {
            /// <summary>
            /// Runs N rounds.
            /// </summary>
            AllRounds,

            /// <summary>
            /// Runs rounds until a controller has reached N number of wins.
            /// </summary>
            FirstTo
        }
    }
}
