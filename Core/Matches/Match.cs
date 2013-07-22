using MBC.Core.Events;
using MBC.Core.Rounds;
using MBC.Core.Util;
using MBC.Shared;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MBC.Core.Matches
{
    /// <summary>
    /// <para>
    /// A match is the basic requirement of a game, in this case, a battleship game. Each Match contains
    /// a number of <see cref="Round"/>s and <see cref="Register"/>s. The Match provides functions
    /// on starting, playing, stopping, and ending the progress of the game. The Match has multi-threading
    /// functionality integrated when running a Match in a separate thread is required. The Match fires
    /// its own <see cref="MatchEvent"/>s during Match progression.
    /// </para>
    /// <para>
    /// On the application side, there is never a need for references to <see cref="ControllerUser"/>s.
    /// </para>
    /// </summary>
    /// <seealso cref="MatchBeginEvent"/>
    /// <seealso cref="MatchEndEvent"/>
    /// <seealso cref="MatchEvent"/>
    /// <seealso cref="Round"/>
    /// <seealso cref="Register"/>
    public abstract class Match : ISerializable
    {
        public Match(Configuration config)
        {
            Config = config;
            Info = new CMatchInfo(Config);
            Events = new EventIterator();
            Rounds = RoundIterator.CreateRoundIteratorFor(this);
        }

        protected Match()
        {
        }

        public event MBCEventHandler Event;

        public Configuration Config
        {
            get;
            protected set;
        }

        [XmlIgnore]
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

        public List<Register> Registers
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
            MakeEvent(new MatchEndEvent());
            Rounds.TargetRounds = Rounds.RoundList.Count;
        }

        /// <summary>
        /// Saves the current progress of this <see cref="Match"/> to a file. All generated <see cref="Event"/>s
        /// and <see cref="ControllerRegister"/>s are saved for later use. Match-specific <see cref="Configuration"/>
        /// settings are also saved to this file.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveToFile(string fileName)
        {
            var test_ser = new XmlSerializer(typeof(ShotList));
            var serializer = new XmlSerializer(typeof(Match));
            var saveDir = Configuration.Global.GetValue<string>("app_data_root") + "matches\\";
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            var writer = new StreamWriter(saveDir + fileName);
            serializer.Serialize(writer, this);
            writer.Close();
        }

        public void GetObjectDaa(SerializationInfo info, StreamingContext context)
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

        internal void RoundEventGenerated(Event ev, bool backward)
        {
            if (Event != null)
            {
                Event(ev, backward);
            }
        }

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