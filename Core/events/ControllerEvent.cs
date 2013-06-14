using MBC.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{

    public delegate void MBCControllerEventHandler(ControllerEvent ev);

    public abstract class ControllerEvent : Event
    {
        private ControllerRegister register;

        public ControllerEvent(ControllerRegister register)
        {
            this.register = register;
        }

        public ControllerRegister Register
        {
            get
            {
                return register;
            }
        }
    }
}
