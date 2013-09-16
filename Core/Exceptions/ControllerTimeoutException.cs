using MBC.Shared;
using System;

namespace MBC.Core
{
    /// <summary>
    /// An exception that provides information about the time it took for a method run through a
    /// <see cref="Player"/>. Provides the <see cref="ControllerRegister"/> of the
    /// <see cref="Player"/> that threw this exception. Used when a <see cref="Controller"/>
    /// has exceeded the <see cref="MatchInfo.TimeLimit"/> specified in <see cref="MatchInfo"/>.
    /// </summary>
    public class ControllerTimeoutException : Exception
    {
        private Player player;
        private string methodName;
        private int timeTaken;

        /// <summary>
        /// Stores the parameters given and generates a message describing the exception.
        /// </summary>
        /// <param name="register">The associated <see cref="ControllerRegister"/>.</param>
        /// <param name="methodName">The name of the method called to a <see cref="Controller"/>.</param>
        /// <param name="timeTaken">The time in milliseconds that the method took to finish.</param>
        public ControllerTimeoutException(Player player, string methodName, int timeTaken)
            : base(player + " took too long executing " + methodName + " according to the time limit of " +
            player.Match.TimeLimit + "ms. Time taken was " + timeTaken + "ms.")
        {
            this.player = player;
            this.methodName = methodName;
            this.timeTaken = timeTaken;
        }

        /// <summary>
        /// Gets the associated <see cref="ControllerRegister"/>.
        /// </summary>
        public Player Player
        {
            get
            {
                return Player;
            }
        }

        /// <summary>
        /// Gets a string that identifies the method used in a <see cref="Controller"/>.
        /// </summary>
        public string MethodName
        {
            get
            {
                return methodName;
            }
        }

        /// <summary>
        /// Gets the time taken by a <see cref="Controller"/> in milliseconds.
        /// </summary>
        public int TimeTaken
        {
            get
            {
                return timeTaken;
            }
        }
    }
}