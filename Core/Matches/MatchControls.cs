using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MBC.Core.Matches
{
    public class MatchControls
    {
        private Match controllingMatch;
        private int delay;

        public MatchControls(Match match)
        {
            controllingMatch = match;
            IsRunning = false;
            Delay = 0;
            Thread = new Thread(PlayLoop);
            SleepHandle = new AutoResetEvent(false);
        }

        public bool IsRunning
        {
            get;
            private set;
        }

        private Thread Thread
        {
            get;
            set;
        }

        private AutoResetEvent SleepHandle
        {
            get;
            set;
        }

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

        public void PlayLoop()
        {
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

        public void Play()
        {
            if (IsRunning)
            {
                return;
            }
            IsRunning = true;
            SleepHandle.Reset();
            Thread.Start();
        }

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
