﻿using MBC.Shared;
using MBC.Shared.Entities;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had lost a <see cref="GameLogic"/>.
    /// </summary>
    [Serializable]
    public class PlayerDisqualifiedEvent : PlayerEvent
    {
        /// <summary>
        /// Constructs the event with the player that lost.
        /// </summary>
        /// <param name="loser"></param>
        public PlayerDisqualifiedEvent(Player loser, string reason)
            : base(loser)
        {
            Reason = reason;
        }

        /// <summary>
        /// Gets the reason for why this player had been disqualified within a round.
        /// </summary>
        public string Reason
        {
            get;
            private set;
        }

        protected internal override void PerformOperation()
        {
            Player.Disqualifications++;
        }
    }
}