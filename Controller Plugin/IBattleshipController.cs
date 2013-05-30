namespace MBC.Core
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Diagnostics;
    using System.Collections.Generic;

    /**
     * <summary>This interface is to be implemented by any class that is to participate in a battleship competition.
     * 
     * The various methods in this class are called at times during the battleship competition. Read over the
     * documentation for each method to understand when these methods are invoked.
     * </summary>
     * 
     */
    public interface IBattleshipController
    {
        /**
         * <summary>Gets a string that represents the name of this IBattleshipController.</summary>
         */
        string Name
        {
            get;
        }

        /**
         * <summary>Gets a Version object that represents the assembly version of this IBattleshipController.</summary>
         */
        Version Version
        {
            get;
        }

        /**
         * <summary>Called when this IBattleshipController has been matched up with another IBattleshipController
         * on the field.</summary>
         * <param name="opponent">The name of the opposing IBattleshipController.</param>
         */
        void NewMatch(string opponent);

        /**
         * <summary>Called when a new game (round) is commencing.</summary>
         * <param name="size">The size of the battlefield.</param>
         * <param name="timeSpan">The time that this IBattleshipController has to finish an invoked method.</param>
         * <param name="rand">A Random object that is to be used by this IBattleshipController.
         * Do not use any other Random object. This object is created specifically for being able to create
         * replays.</param>
         */
        void NewGame(Size size, TimeSpan timeSpan, Random rand);

        /**
         * <summary>Called when this IBattleshipOpponent must place their ships. Utilize the Ship objects in the
         * given collection to place ships. Do not provide invalid ship placements (overlapping, bad coords, etc.)</summary>
         * <param name="ships">A collection of ships to place.</param>
         */
        void PlaceShips(ReadOnlyCollection<Ship> ships);

        /**
         * <summary>Called when this IBattleshipOpponent has the opportunity to make a shot in their turn.</summary>
         * <returns>The shot to be made.</returns>
         */
        Point GetShot();

        /**
         * <summary>Called when this IBattleshipOpponent is being shot at by the opponent.</summary>
         * <param name="shot">The shot the opponent has made against this IBattleshipOpponent</param>
         */
        void OpponentShot(Point shot);

        /**
         * <summary>Called when this IBattleshipOpponent has hit an opponent ship from the Point given by a previous
         * call to GetShot().</summary>
         * <param name="shot">The Point that made a ship hit on the opponent</param>
         * <param name="sunk">True if the shot had sunk an opponent ship.</param>
         */
        void ShotHit(Point shot, bool sunk);

        /**
         * <summary>Called when this IBattleshipOpponent did not hit an opponent ship from the Point given by a previous
         * call to GetShot().</summary>
         * <param name="shot">The Point that missed.</param>
         */
        void ShotMiss(Point shot);

        /**
         *<summary>Called when a game (round) is over. In this case, this IBattleshipController has won the round.</summary>
         */
        void GameWon();

        /**
         * <summary>Called when a game (round) is over. In this case, this IBattelshipController has lost the round.</summary>
         */
        void GameLost();

        /**
         * <summary>Called when a competition matchup is over. Note that a matchup can start up again with the same
         * parameters.</summary>
         */
        void MatchOver();
    }
}
