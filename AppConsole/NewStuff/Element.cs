using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBC.App.Terminal;
using MBC.Core.Util;
using MBC.Shared;

namespace MBC.App.BattleshipConsole.NewStuff
{
    public delegate void ConsoleElementHandler(Element element);

    [Configuration("mbc_console_text_foreground", ConsoleColor.Gray)]
    [Configuration("mbc_console_text_background", ConsoleColor.Black)]
    [Configuration("mbc_console_text_flashing_foreground", ConsoleColor.White)]
    [Configuration("mbc_console_text_flashing_background", ConsoleColor.DarkRed)]
    [Configuration("mbc_console_text_selected_foreground", ConsoleColor.Gray)]
    [Configuration("mbc_console_text_selected_background", ConsoleColor.DarkBlue)]
    public abstract class Element
    {
        private List<Element> subElements;

        public Element()
        {
            var globalConf = Configuration.Global;
            subElements = new List<Element>();
            NormalColors = new ColorStore(globalConf.GetValue<ConsoleColor>("mbc_console_text_foreground"),
                globalConf.GetValue<ConsoleColor>("mbc_console_text_background"));
            SelectedColors = new ColorStore(globalConf.GetValue<ConsoleColor>("mbc_console_text_selected_foreground"),
                globalConf.GetValue<ConsoleColor>("mbc_console_text_selected_background"));
            FlashColors = new ColorStore(globalConf.GetValue<ConsoleColor>("mbc_console_text_flashing_foreground"),
                globalConf.GetValue<ConsoleColor>("mbc_console_text_flashing_background"));
            ThreadRequired = false;
        }

        public event ConsoleElementHandler UpdateRequired;

        public ColorStore FlashColors
        {
            get;
            protected set;
        }

        public ColorStore NormalColors
        {
            get;
            protected set;
        }

        public Element Parent
        {
            get;
            private set;
        }

        public ColorStore SelectedColors
        {
            get;
            protected set;
        }

        public IList<Element> SubElements
        {
            get
            {
                return subElements;
            }
        }

        public bool ThreadRequired
        {
            get;
            protected set;
        }

        public void AddElement(Element sub)
        {
            sub.Parent = this;
            subElements.Add(sub);
        }

        public abstract string GetTextFrom(int x, int y, int width);

        public virtual bool Selected()
        {
            return false;
        }

        public virtual void ThreadUpdate()
        {
        }
    }
}