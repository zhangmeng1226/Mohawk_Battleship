using MBC.Core.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.Shared;
using System.Runtime.Serialization;

namespace MBC.Core.Events
{
    /// <summary>
    /// Provides information about a <see cref="Round"/> that has begun.
    /// </summary>
    public class RoundBeginEvent : RoundEvent
    {
        /// <summary>
        /// Passes the <paramref name="round"/> to the base constructor and generates a <see cref="Event.Message"/>
        /// based on the <see cref="MBC.Shared.Register"/>s that are involved in it.
        /// </summary>
        /// <param name="round">The associated <see cref="Round"/>.</param>
        public RoundBeginEvent(IDNumber roundID) 
            : base(roundID)
        {
            RoundID = roundID;
        }

        public IDNumber RoundID
        {
            get;
            private set;
        }

        private RoundBeginEvent(SerializationInfo info, StreamingContext context)
        {

        }

        private void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }

        public virtual Type EventType
        {
            get
            {
                return Type.RoundBegin;
            }
        }
    }
}