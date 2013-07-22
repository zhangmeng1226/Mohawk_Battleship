using MBC.Core.Matches;
using System;
using System.Xml.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// The base class for a series of <see cref="Event"/>s that are related to a <see cref="Match"/>.
    /// </summary>
    public abstract class MatchEvent : Event
    {
    }
}