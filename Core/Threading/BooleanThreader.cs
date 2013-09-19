using System.Threading;

namespace MBC.Core.Matches
{

    public delegate bool ThreadStartBool();

    public class BooleanThreader
    {
        private ThreadStartBool controllingMethod;

        /// <summary>
        /// Initializes the separate thread logic.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> to automatically step.</param>
        public BooleanThreader(ThreadStartBool start)
        {
            IsRunning = false;
            RunningToCompletion = false;
            Thread = new Thread(PlayLoop);
            SleepHandle = new AutoResetEvent(false);
        }

        /// <summary>
        /// Gets a value that indicates the running state.
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        }

        public bool RunningToCompletion
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

        private void Run(bool toCompletion)
        {
            if (IsRunning)
            {
                return;
            }
            SleepHandle.Reset();
            RunningToCompletion = toCompletion;
            Thread.Start();
        }

        private void PlayLoop()
        {
            if (IsRunning)
            {
                return;
            }
            IsRunning = true;
            bool incomplete = false;
            do
            {
                incomplete = controllingMethod();
            } while (RunningToCompletion && incomplete);
            IsRunning = false;
        }

        public void Pause()
        {
            if (IsRunning)
            {
                IsRunning = false;
                SleepHandle.Set();
            }
        }
    }
}