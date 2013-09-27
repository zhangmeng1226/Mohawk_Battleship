using System.Threading;

namespace MBC.Core.Threading
{

    public delegate bool ThreadStartBool();

    public class BooleanThreader : Threader
    {
        private ThreadStartBool controllingMethod;

        /// <summary>
        /// Initializes the separate thread logic.
        /// </summary>
        /// <param name="match">The <see cref="Match"/> to automatically step.</param>
        public BooleanThreader(ThreadStartBool start)
        {
            RunToTrue = false;
            controllingMethod = start;
        }

        public bool RunToTrue
        {
            get;
            private set;
        }

        protected override void ThreadRun()
        {
            bool incomplete = false;
            do
            {
                incomplete = controllingMethod();
            } while (RunToTrue && incomplete);
        }
    }
}