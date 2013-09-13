using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MBC.Core.Rounds
{
    /// <summary>
    /// A derivative of the <see cref="ControlledRound"/> which provides a turn-by-turn based game of
    /// battleship.
    /// </summary>
    internal class ClassicRound : ControlledRound
    {
        [XmlIgnore]
        private LogicState currentState;

        /// <summary>
        /// Passes parameters to the base constructor.
        /// </summary>
        /// <param name="info">The <see cref="MatchInfo"/> from a round to associate with.</param>
        /// <param name="controllers">The <see cref="Player"/>s to utilize.</param>
        public ClassicRound(MatchInfo info, List<Player> controllers)
            : base(info, controllers)
        {
            currentState = LogicState.Begin;
        }

        private enum LogicState
        {
            Begin,
            PlaceShips,
            Turn,
            End
        }

        /// <summary>
        /// Performs a step in the game logic for the battleship game.
        /// </summary>
        protected internal override void GameLogicStep()
        {
            switch (currentState)
            {
                case LogicState.Begin:
                    Begin();
                    currentState = LogicState.PlaceShips;
                    break;

                case LogicState.PlaceShips:
                    StandardShipPlacement();
                    currentState = LogicState.Turn;
                    break;

                case LogicState.Turn:
                    StandardPlaceShot();
                    if (Remaining.Count <= 1)
                    {
                        End();
                        currentState = LogicState.End;
                    }
                    else
                    {
                        NextTurn();
                    }
                    break;
            }
        }
    }
}