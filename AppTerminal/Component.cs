using MBC.App.Terminal.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBC.App.Terminal
{
    public class Component
    {
        private List<Component> children = new List<Component>();
        private Component parent;
        private Layout layout;
        private string text = ""; //The string of text this UserControl will display.
        private object[] txtFmtParams;
        private ConsoleColorSet unselectedColors;
        private ConsoleColorSet selectedColors;
        private bool selectable;
        private Coordinate2D coords;
        private Size2D size;

        public Component()
        {
            Layout = new FlowLayout(FlowLayout.Alignment.Left);
            selectedColors = new ConsoleColorSet(ConsoleColor.Black, ConsoleColor.White);
            unselectedColors = new ConsoleColorSet(ConsoleColor.White, ConsoleColor.Black);
            selectable = false;
            coords = new Coordinate2D(0, 0);
            size = new Size2D(0, 0);
        }

        /// <summary>Invoked when an input string has been entered into the BattleshipConsole, while this
        /// ControlLayout is in focus. By default, this will invoke the selected UserControl object's
        /// Input(string) method.</summary>
        public virtual void Input(string input)
        {
        }

        /// <summary>Invoked when a key has been pressed, the TerminalModule is selected in the
        /// BattleshipConsole, and this ControlLayout is in focus within the TerminalModule.
        /// 
        /// Through this method, this ControlLayout will iterate through all of the UserControl objects contained
        /// with the up and down arrow keys. If these keys have not been pressed, the selected UserControl
        /// KeyPress(ConsoleKeyInfo) method will be called.
        /// 
        /// Changing this default behaviour may be desireable in some cases such as the NoLayout class.
        /// </summary>
        public virtual bool KeyPressed(ConsoleKeyInfo key)
        {
            return false;
        }

        public List<Component> Children
        {
            get
            {
                return children;
            }
        }

        public Component Parent
        {
            get
            {
                return parent;
            }
        }

        public Layout Layout
        {
            get
            {
                return layout;
            }
            set
            {
                layout = value;
                layout.SetLayoutComponent(this);
            }
        }

        public bool Selectable
        {
            get
            {
                return selectable;
            }
            set
            {
                selectable = value;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }
        }

        public object[] TextFormatParameters
        {
            get
            {
                return txtFmtParams;
            }
        }

        public ConsoleColorSet SelectedColors
        {
            get
            {
                return selectedColors;
            }
            set
            {
                selectedColors = value;
            }
        }

        public ConsoleColorSet UnselectedColors
        {
            get
            {
                return unselectedColors;
            }
            set
            {
                unselectedColors = value;
            }
        }

        public virtual Coordinate2D Location
        {
            get
            {
                return coords;
            }
            set
            {
                coords = value;
            }
        }

        public virtual Size2D Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                Layout.DoLayout();
            }
        }

        public void SetText(string txt, params object[] obj)
        {
            text = txt;
            txtFmtParams = obj;
        }

        public virtual void Add(Component comp)
        {
            children.Add(comp);
            comp.parent = this;
        }

        public void ReLayout()
        {
            Layout.DoLayout();
        }

        public void RequestUpdate()
        {
            RootComponent root = GetRootComponent();
            RecurseRequestUpdate(root);
        }

        private void RecurseRequestUpdate(RootComponent root)
        {
            UpdateRequested(root, this);
            foreach (var comp in children)
            {
                comp.RecurseRequestUpdate(root);
            }
        }

        public RootComponent GetRootComponent()
        {
            if (Parent != null)
            {
                return Parent.GetRootComponent();
            }
            else
            {
                return (RootComponent) this;
            }
        }

        private void UpdateRequested(RootComponent root, Component comp)
        {
            if (!root.UpdateQueue.Contains(comp))
            {
                root.UpdateQueue.Add(comp);
            }
        }
    }
}
