using MBC.Core.Events;
using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;
using System.Collections.Generic;

namespace MBC.Core.Matches
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
    public abstract class Match
    {
        public Match(Configuration config)
        {
            Config = config;
            Info = new CMatchInfo(Config);
            Events = new EventIterator();
            Rounds = RoundIterator.CreateRoundIteratorFor(this);
        }

        public event MBCEventHandler Event;

        public Configuration Config
        {
            get;
            protected set;
        }

        public MatchControls Controls
        {
            get;
            private set;
        }

        public MatchInfo Info
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="ControllerRegister"/>s involved.
        /// </summary>
        /// <seealso cref="ControllerRegister"/>
        public List<ControllerRegister> Registers
        {
            get;
            protected set;
        }

        public RoundIterator Rounds
        {
            get;
            private set;
        }

        private EventIterator Events
        {
            get;
            set;
        }

        /// <summary>
        /// <see cref="Match.Stop()"/>s the match, ends the currently running <see cref="Round"/>, creates
        /// a <see cref="MatchEndEvent"/>, and prevents further progression of the match.
        /// </summary>
        /// <seealso cref="MatchEvent"/>
        /// <seealso cref="Match.InProgress"/>
        public void End()
        {
            Controls.Stop();
            Rounds.CurrentRound.End();
            MakeEvent(new MatchEndEvent(this));
            Rounds.TargetRounds = Rounds.RoundList.Count;
        }

        /// <summary>
        /// Saves the current progress of this <see cref="Match"/> to a file. All generated <see cref="Event"/>s
        /// and <see cref="ControllerRegister"/>s are saved for later use. Match-specific <see cref="Configuration"/>
        /// settings are also saved to this file.
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveToFile(string filePath)
        {
        }

        public bool StepBackward()
        {
            if (Rounds.CurrentRound.StepBackward() && Rounds.PrevRound())
            {
                return true;
            }
            return false;
        }

        public bool StepForward()
        {
            if (Rounds.CurrentRound.StepForward() && Rounds.NextRound())
            {
                return true;
            }
            return false;
        }

        internal abstract Round CreateNewRound();

        private void MakeEvent(Event ev)
        {
            Events.AddEvent(ev);
            if (Event != null)
            {
                Event(ev, false);
            }
        }
    }
}