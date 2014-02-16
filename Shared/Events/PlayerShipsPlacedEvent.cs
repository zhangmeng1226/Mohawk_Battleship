﻿using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBC.Shared.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/>'s <see cref="ShipList"/> that had
    /// been requested to be modified.
    /// </summary>
    public class PlayerShipsPlacedEvent : PlayerEvent
    {
        private ISet<Ship> shipList;

        /// <summary>
        /// Constructs the event with the player that placed ships.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="newShips"></param>
        public PlayerShipsPlacedEvent(Player player, ISet<Ship> newShips)
            : base(player)
        {
            shipList = newShips;
        }

        /// <summary>
        /// Gets a list of ships placed.
        /// </summary>
        public ISet<Ship> ShipsList
        {
            get
            {
                return new HashSet<Ship>(shipList);
            }
        }

        public override bool ApplyBackward()
        {
            //TODO: Apply the previous ship placement.
            return true;
        }

        public override bool ApplyForward()
        {
            Player.Ships = ShipsList;
            return true;
        }
    }
}