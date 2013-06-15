using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Shared
{
    /// <summary>
    /// A ControllerID is an integer that represents the order that a controller is placed in a round.
    /// This struct may be used exactly like an int value in code.
    /// </summary>
    public struct ControllerID
    {
        private int value;

        private ControllerID(int value)
        {
            this.value = value;
        }

        public static implicit operator ControllerID(int value)
        {
            return new ControllerID(value);
        }

        public static implicit operator int(ControllerID id)
        {
            return id.value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}
