using MBC.Core.Events;
using MBC.Core.Util;
using MBC.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MBC.Core
{

    /// <summary>
    /// <para>
    /// A match is the basic requirement of a game, in this case, a battleship game. Each Match contains
    /// a number of <see cref="Round"/>s and <see cref="ControllerRegister"/>s. The Match provides functions
    /// on starting, playing, stopping, and ending the progress of the game. The Match has multi-threading
    /// functionality integrated when running a Match in a separate thread is required. The Match fires
    /// its own <see cref="MatchEvent"/>s during Match progression.
    /// </para>
    /// <para>
    /// On the application side, there is never a need for references to <see cref="ControllerUser"/>s.
    /// </para>
    /// </summary>
    /// <seealso cref="MatchBeginEvent"/>
    /// <seealso cref="MatchPlayEvent"/>
    /// <seealso cref="MatchStopEvent"/>
    /// <seealso cref="MatchEndEvent"/>
    /// <seealso cref="Round"/>
    /// <seealso cref="ControllerRegister"/>
    [Configuration("mbc_match_rounds_mode", "all")]
    [Configuration("mbc_match_rounds", 100)]
    public class Match
    {
        private Configuration conf;
        private MatchInfo info;
        private PlayMode roundPlay;

        private List<ControllerUser> controllers; //Empty if the match is loaded by a file
        private List<ControllerRegister> registers;

        private int targetRounds;
        private int roundIteration;
        private List<Round> roundList;
        private Round currentRound;
        private bool inProgress;

        private Thread runningThread;
        private AutoResetEvent sleepHandle; //Used when a delay between Round progression is used.
        private bool isRunning;
        private int delay; //Used to delay each call to progress a Round by milliseconds.

        /// <summary>
        /// Creates a new Match using the application-wide <see cref="Configuration"/> and the
        /// controllers specified in the given <see cref="ControllerInformation"/>s to use.
        /// </summary>
        /// <param name="controllers">A variable number of controllers to load from the given
        /// <see cref="ControllerInformation"/>s.
        /// </param>
        /// <seealso cref="ControllerInformation"/>
        /// <seealso cref="Configuration"/>
        public Match(params ControllerInformation[] controllers)
        {
            Init(Configuration.Global, controllers);
        }

        /// <summary>
        /// Creates a new Match using a specific <see cref="Configuration"/> and a number of
        /// controllers specified in the given <see cref="ControllerInformation"/>s to use.
        /// </summary>
        /// <param name="conf">The <see cref="Configuration"/> to utilize.</param>
        /// <param name="controllers">A variable number of controllers to load from the given
        /// <see cref="ControllerInformation"/>s.
        /// </param>
        /// <seealso cref="ControllerInformation"/>
        /// <seealso cref="Configuration"/>
        public Match(Configuration conf, params ControllerInformation[] controllers)
        {
            Init(conf, controllers);
        }

        /// <summary>
        /// Occurs when a <see cref="MatchEvent"/> has been generated.
        /// </summary>
        /// <seealso cref="MatchEvent"/>
        public event MBCMatchEventHandler MatchEvent;

        /// <summary>
        /// Occurs when a <see cref="RoundEvent"/> has been generated from a <see cref="Round"/>.
        /// </summary>
        /// <seealso cref="RoundEvent"/>
        public event MBCRoundEventHandler RoundEvent;

        /// <summary>
        /// Occurs when a controller within a <see cref="Round"/> has caused a <see cref="ControllerEvent"/>
        /// to generate.
        /// </summary>
        /// <seealso cref="ControllerEvent"/>
        public event MBCControllerEventHandler ControllerEvent;

        /// <summary>
        /// Provides various behaviours as to how a Match will handle the number of rounds it is configured
        /// with.
        /// </summary>
        public enum PlayMode
        {
            /// <summary>
            /// Creates and plays through <see cref="Round"/>s until the number of <see cref="Round"/>s generated
            /// is equal to the number of rounds configured.
            /// </summary>
            /// <seealso cref="Round"/>
            AllRounds,

            /// <summary>
            /// Creates and plays through <see cref="Round"/>s until a <see cref="ControllerRegister"/> has
            /// reached the number of rounds configured.
            /// </summary>
            /// <seealso cref="Round"/>
            /// <seealso cref="ControllerRegister"/>
            FirstTo
        }

        /// <summary>
        /// Gets the behaviour of the number of rounds being played as defined by <see cref="PlayMode"/>.
        /// </summary>
        /// <seealso cref="PlayMode"/>
        public PlayMode RoundPlayMode
        {
            get
            {
                return roundPlay;
            }
        }

        /// <summary>
        /// Gets the <see cref="MatchInfo"/> generated through a <see cref="Configuration"/>
        /// </summary>
        /// <seealso cref="MatchInfo"/>
        /// <seealso cref="Configuration"/>
        public MatchInfo Info
        {
            get
            {
                return info;
            }
        }

        /// <summary>
        /// Gets the number of rounds set through the <see cref="Configuration"/>.
        /// </summary>
        public int NumberOfRounds
        {
            get
            {
                return targetRounds;
            }
        }

        /// <summary>
        /// Gets the list of <see cref="Round"/>s created.
        /// </summary>
        /// <seealso cref="Round"/>
        public IEnumerable<Round> Rounds
        {
            get
            {
                return roundList.AsEnumerable();
            }
        }

        /// <summary>
        /// Gets the <see cref="Round"/> currently being progressed.
        /// </summary>
        /// <seealso cref="Round"/>
        public Round CurrentRound
        {
            get
            {
                return currentRound;
            }
        }

        /// <summary>
        /// True if the match can be progressed, false if it cannot and this match ended.
        /// </summary>
        public bool InProgress
        {
            get
            {
                return inProgress;
            }
        }

        /// <summary>
        /// Gets the <see cref="ControllerRegister"/>s involved.
        /// </summary>
        /// <seealso cref="ControllerRegister"/>
        public IEnumerable<ControllerRegister> Registers
        {
            get
            {
                return registers.AsEnumerable();
            }
        }

        /// <summary>
        /// Gets or sets the time in milliseconds to wait between <see cref="Round"/> progressions while
        /// running.
        /// </summary>
        public int TurnDelay
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
        /// Does a single <see cref="Round"/> progression. Creates new <see cref="Round"/>s if the <see cref="Match.CurrentRound"/>
        /// is complete until the match is complete.
        /// </summary>
        /// <returns>false if the behaviour of <see cref="Match.RoundPlayMode"/> has been satisfied, signifying
        /// the end of the match, or true if the match has not ended.</returns>
        /// <seealso cref="PlayMode"/>
        public bool Step()
        {
            if (isRunning)
            {
                return true;
            }
            return Progress();
        }

        /// <summary>
        /// If the match is not running, runs the match in a different thread, creates a new <see cref="MatchPlayEvent"/>,
        /// and continuously makes calls to <see cref="Match.Step()"/> until that method returns false. If the
        /// match is running, does nothing.
        /// </summary>
        /// <seealso cref="Thread"/>
        public void Play()
        {
            if (isRunning)
            {
                return;
            }
            MakeEvent(new MatchPlayEvent(this));
            isRunning = true;
            sleepHandle.Reset();
            runningThread.Start();
        }

        /// <summary>
        /// If the match is running in a separate thread, stops the thread and creates a new <see cref="MatchStopEvent"/>,
        /// otherwise, does nothing.
        /// </summary>
        /// <seealso cref="Thread"/>
        public void Stop()
        {
            if (isRunning)
            {
                MakeEvent(new MatchStopEvent(this));
                isRunning = false;
                sleepHandle.Set();
            }
        }

        /// <summary>
        /// <see cref="Match.Stop()"/>s the match, ends the currently running <see cref="Round"/>, creates
        /// a <see cref="MatchEndEvent"/>, and prevents further progression of the match.
        /// </summary>
        /// <seealso cref="MatchEvent"/>
        /// <seealso cref="Match.InProgress"/>
        public void End()
        {
            Stop();
            if (currentRound != null)
            {
                currentRound.End();
            }
            MakeEvent(new MatchEndEvent(this));
            roundIteration = targetRounds;
        }

        private void Init(Configuration conf, params ControllerInformation[] controllersToLoad)
        {
            this.conf = conf;
            //Get the game info from the Configuration.
            //Expecting an exception to be thrown here if a controller isn't compatible with the configured game mode.
            info = new CMatchInfo(conf, controllersToLoad);

            //Create and register the controllers to load.
            GenerateControllers(controllersToLoad);
            RegisterControllers();
            FormOpponents();

            //Configuration setting for the number of rounds this Match will perform.
            roundList = new List<Round>();
            targetRounds = conf.GetValue<int>("mbc_match_rounds");
            roundIteration = -1;
            inProgress = true;

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
                    conf.SetValue("mbc_match_rounds_mode", "all");
                    break;
            }
        }

        private void MakeEvent(MatchEvent ev)
        {
            if (MatchEvent != null)
            {
                MatchEvent(ev);
            }
        }

        private bool IsRoundTargetReached()
        {
            if (roundPlay == PlayMode.AllRounds)
            {
                return roundIteration == targetRounds;
            }
            else if (roundPlay == PlayMode.FirstTo)
            {
                foreach (var registrant in controllers)
                {
                    if (registrant.Register.Score == targetRounds)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// No teams taken into account atm
        /// </summary>
        private void FormOpponents()
        {
            foreach (var registrant in registers)
            {
                registrant.Opponents = new List<ControllerID>();
                foreach (var otherRegister in registers)
                {
                    if (otherRegister == registrant) continue;
                    registrant.Opponents.Add(otherRegister.ID);
                }
            }
        }

        private void GenerateControllers(IEnumerable<ControllerInformation> ctrlInfos)
        {
            controllers = new List<ControllerUser>();
            for (var id = 0; id < ctrlInfos.Count(); id++)
            {
                controllers.Add(new ControllerUser(ctrlInfos.ElementAt(id)));
            }
        }

        private void RegisterControllers()
        {
            registers = new List<ControllerRegister>();
            for(var id = 0; id < controllers.Count; id++)
            {
                registers.Add(new ControllerRegister(info, id));
            }
        }

        private bool Progress()
        {
            if (!inProgress || IsRoundTargetReached())
            {
                inProgress = false;
                return false;
            }

            if (currentRound == null)
            {
                if (roundIteration++ == -1)
                {
                    for (var id = 0; id < controllers.Count; id++)
                    {
                        controllers[id].NewMatch(registers[id]);
                    }
                    MakeEvent(new MatchBeginEvent(this));
                }
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

        private bool SetNewRound()
        {
            if (info.GameMode.HasFlag(GameMode.Classic))
            {
                currentRound = new ClassicRound(info, controllers);
                currentRound.RoundEvent += (ev) =>
                    {
                        if (RoundEvent != null)
                        {
                            RoundEvent(ev);
                        }
                    };
                currentRound.ControllerEvent += (ev) =>
                    {
                        if (ControllerEvent != null)
                        {
                            ControllerEvent(ev);
                        }
                    };
                roundList.Add(currentRound);
                return true;
            }
            else
            {
                End();
                return false;
            }
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
    }
}