﻿using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Created during a match when a player is removed.
    /// </summary>
    public class MatchRemovePlayerEvent : MatchEvent
    {
        /// <summary>
        /// Constructs this event with the player being removed.
        /// </summary>
        /// <param name="player"></param>
        public MatchRemovePlayerEvent(Match match, Player player)
            : base(match)
        {
            Player = player;
        }

        /// <summary>
        /// Gets the player that has been removed.
        /// </summary>
        public Player Player
        {
            get;
            private set;
        }

        public override bool ApplyBackward()
        {
            return Match.Players.Add(Player);
        }

        public override bool ApplyForward()
        {
            return Match.Players.Remove(Player);
        }
    }
}