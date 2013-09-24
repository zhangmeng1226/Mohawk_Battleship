using MBC.Core.Rounds;
using System;
using System.Xml.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that relate to changes in a <see cref="Round"/>.
    /// </summary>
    public abstract class RoundEvent : Event
    {
    }
}