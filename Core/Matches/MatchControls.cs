using System.Threading;

namespace MBC.Core.Matches
{
    /// <summary>
    /// Provides a number of methods that control the automatic progression of a <see cref="Match"/> in a
    /// separate thread.
    /// </summary>
    public class MatchControls
    {
        /// <summary>
        /// The <see cref="Match"/> being controlled.
        /// </summary>
        private Match controllingMatch;

        /// <summary>
        /// The time to sleep between steps.
        /// </summary>
        private int delay;

        /// <summary>
        /// Initializes the separate thread logic.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> to automatically step.</param>
        public MatchControls(Match match)
        {
            controllingMatch = match;
            IsRunning = false;
            Delay = 0;
            Thread = new Thread(PlayLoop);
            SleepHandle = new AutoResetEvent(false);
        }

        /// <summary>
        /// Gets the time in milliseconds to sleep for between progressions.
        /// </summary>
        public int Delay
        {
            get
            {
                return delay;
            }
            set
            {
                delay = value;
                if (delay < 0)
                {
                    delay = 0;
                }
            }
        }

        /// <summary>
        /// Gets a value that indicates the running state.
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the AutoResetEvent handle used for signalling.
        /// </summary>
        private AutoResetEvent SleepHandle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the separate Thread.
        /// </summary>
        private Thread Thread
        {
            get;
            set;
        }

        /// <summary>
        /// Activates the auto-progression functionality in a separate thread.
        /// </summary>
        public void Play()
        {
            if (IsRunning)
            {
                return;
            }
            SleepHandle.Reset();
            Thread.Start();
        }

        /// <summary>
        /// In the same thread, repeat forward progression of a <see cref="Match"/> until it cannot be progressed further, with an
        /// optional delay.
        /// </summary>
        public void PlayLoop()
        {
            if (IsRunning)
            {
                return;
            }
            IsRunning = true;
            while (IsRunning)
            {
                if (controllingMatch.StepForward())
                {
                    break;
                }
                if (Delay != 0)
                {
                    SleepHandle.WaitOne(Delay);
                }
            }
            IsRunning = false;
        }

        /// <summary>
        /// Stops running the automatic progression.
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                SleepHandle.Set();
            }
        }
    }
}