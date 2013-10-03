﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
namespace MBC.Core.Events
{
    /// <summary>
    /// Defines a method that retrieves and handles an <see cref="Event"/>.
    /// </summary>
    /// <param name="ev">The generated <see cref="Event"/></param>
    public delegate void MBCEventHandler(Event ev);

    /// <summary>
    /// The base class for any event created in the MBC core framework. Provides a message string that
    /// describes the generated event.
    /// </summary>
    /// <seealso cref="MatchEvent"/>
    /// <seealso cref="RoundEvent"/>
    /// <seealso cref="PlayerEvent"/>
    public abstract class Event : ISerializable
    {
        public Event()
        {
            Millis = DateTime.Now.Millisecond;
        }

        public virtual Type EventType
        {
            get
            {
                return Type.Unspecified;
            }
        }

        public int Millis
        {
            get;
            private set;
        }

        public enum Type
        {
            MatchBegin,
            MatchEnd,
            MatchConfigChanged,
            MatchTeamCreate,
            MatchPlayerTeamAssign,
            MatchAddPlayer,
            MatchRemovePlayer,
            PlayerHitShip,
            PlayerLost,
            PlayerShipDestroyed,
            PlayerShipsPlaced,
            PlayerShot,
            PlayerTimeout,
            PlayerWon,
            RoundAccolade,
            RoundBegin,
            RoundEnd,
            RoundStateSave,
            RoundPlayerAdd,
            RoundPlayerAction,
            Unspecified
        }
    }
}