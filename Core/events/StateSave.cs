using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.Core.Events
{
    public class StateSave
    {
        public StateSave(int idx)
        {
            StateIndex = idx;
        }

        public object ObjectCopy
        {
            get;
            set;
        }

        public int StateIndex
        {
            get;
            private set;
        }
    }
}