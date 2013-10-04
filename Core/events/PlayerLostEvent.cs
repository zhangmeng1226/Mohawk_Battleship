using MBC.Core.Rounds;
using MBC.Shared;
using System;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Register"/> that had lost a <see cref="Round"/>.
    /// </summary>
    public class PlayerLostEvent : PlayerEvent
    {
        public PlayerLostEvent(IDNumber loser) : base(loser) { }
        
        private PlayerLostEvent(SerializationInfo info, StreamingContext context)
        {

        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }
    }
}