using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal
{
    public class RootComponent : Component
    {
        private List<Component> updateQueue = new List<Component>();
        private List<Component> selectable = new List<Component>();
        private Component selected;

        public List<Component> UpdateQueue
        {
            get
            {
                return updateQueue;
            }
        }

        public override Coordinate2D Location
        {
            get
            {
                return base.Location;
            }
            set
            {

            }
        }

        public override Size2D Size
        {
            get
            {
                return base.Size;
            }
            set
            {

            }
        }

        public Component Selected
        {
            get
            {
                return selected;
            }
        }

        public void SelectNext()
        {
            if (selected != null)
            {
                int curSelectIdx = selectable.IndexOf(selected);
                if (curSelectIdx == selectable.Count)
                {
                    selected = selectable[0];
                }
                else
                {
                    selected = selectable[curSelectIdx + 1];
                }
            }
        }

        public void SelectPrevious()
        {
            if (selected != null)
            {
                int curSelectIdx = selectable.IndexOf(selected);
                if (curSelectIdx == 0)
                {
                    selected = selectable[selectable.Count - 1];
                }
                else
                {
                    selected = selectable[curSelectIdx - 1];
                }
            }
        }

        public override void Add(Component comp)
        {
            base.Add(comp);
            if (comp.Selectable)
            {
                selectable.Add(comp);
            }
        }
    }
}
