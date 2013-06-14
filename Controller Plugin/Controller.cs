using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Generic;

namespace MBC.Core
{
    /// <summary>
    /// A delegate defining a method that receives a string.
    /// </summary>
    /// <param name="message">The message that the controller wants to output.</param>
    public delegate void StringOutputHandler(string message);

    /// <summary>This interface is to be implemented by any class that is to participate in a classic or salvo
    /// match of battleship (see <see cref="GameMode"/>).
    /// 
    /// The various methods in this class are called at times during the battleship game. Read over the
    /// documentation for each method to understand when these methods are invoked.
    /// </summary>
    public abstract class Controller
    {
        /// <summary>
        /// This event should be invoked when the controller interface wants to output a message.
        /// </summary>
        public event StringOutputHandler ControllerMessageEvent;

        public ControllerRegister Register { get; set; }

        public void SendMessage(string message)
        {
            ControllerMessageEvent(message);
        }

        /// <summary>Called when this controller is being placed in a match.</summary>
        /// <param name="matchInfo">Information about the match this controller is being placed in.</param>
        /// <seealso cref="MatchInfo"/>
        public virtual void NewMatch() { }

        /// <summary>Called when a new round in the match is commencing.</summary>
        public virtual void NewRound() { }

        /// <summary>Called when this controller must place their ships. Utilize the Ship objects in the
        /// given collection to place ships. Do not provide invalid ship placements (overlapping, bad coords, etc.)</summary>
        /// <param name="ships">A collection of ships to place.</param>
        public virtual void PlaceShips() { }

        /// <summary>
        /// Called when this controller has the opportunity to make a shot in their turn. The given
        /// Shot object must have its properties modified to whatever is desired, keeping in mind the rules
        /// of a round.
        /// </summary>
        /// <param name="shot">The Shot given to this controller to modify.</param>
        /// <seealso cref="Shot"/>
        public virtual void MakeShot(Shot shot) { }

        /// <summary>Called when this controller is being shot at by another controller.</summary>
        /// <param name="shot">The shot the opponent has made against this controller</param>
        /// <seealso cref="Shot"/>
        public virtual void OpponentShot(Shot shot) { }

        /// <summary>Called when this controller has hit an opposing Ship from the Shot given by a previous
        /// call to MakeShot().</summary>
        /// <param name="shot">The Shot that made a ship hit on a controller.</param>
        /// <param name="sunk">True if the shot had sunk an opposing ship.</param>
        /// <seealso cref="Shot"/>
        public virtual void ShotHit(Shot shot, bool sunk) { }

        /// <summary>Called when this controller did not hit an opposing ship from the Shot given by a previous
        /// call to MakeShot().</summary>
        /// <param name="shot">The Shot that missed.</param>
        /// <seealso cref="Shot"/>
        public virtual void ShotMiss(Shot shot) { }

        /// <summary>
        /// Called when an opposing controller has been removed from the round and has lost.
        /// </summary>
        /// <param name="destroyedID">The ControllerID of the controller that lost.</param>
        /// <seealso cref="ControllerID"/>
        public virtual void OpponentDestroyed(ControllerID destroyedID) { }

         /// <summary>
         /// Called when a round is over. In this case, this controller has won the round.
         /// </summary>
        public virtual void RoundWon() { }

        /// <summary>Called when a round is over. In this case, this controller has lost the round.</summary>
        public virtual void RoundLost() { }

        /// <summary>Called when a matchup has ended.</summary>
        public virtual void MatchOver() { }
    }
}
