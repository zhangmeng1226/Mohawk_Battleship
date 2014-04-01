using MBC.Shared.Attributes;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MBC.Shared
{
    public abstract class Entity
    {
        private Dictionary<Type, List<MBCEventHandler>> filters = new Dictionary<Type, List<MBCEventHandler>>();
        private List<MBCEventHandler> noFilters = new List<MBCEventHandler>();

        public event MBCEventHandler OnEvent
        {
            add
            {
                AddEventHandler(value);
            }
            remove
            {
                RemoveEventHandler(value);
            }
        }

        protected virtual void AddEventHandler(MBCEventHandler del)
        {
            lock (this)
            {
                MethodInfo method = del.Method;
                object[] attributes = method.GetCustomAttributes(typeof(EventFilterAttribute), false);
                if (attributes.Length == 0)
                {
                    foreach (object o in attributes)
                    {
                        EventFilterAttribute attribute = (EventFilterAttribute)o;
                        if (!filters.ContainsKey(attribute.Event))
                        {
                            filters.Add(attribute.Event, new List<MBCEventHandler>());
                        }
                        filters[attribute.Event].Add(del);
                    }
                }
                else
                {
                    noFilters.Add(del);
                }
            }
        }

        protected void InvokeEvent(Event ev)
        {
            lock (this)
            {
                Type eventType = ev.GetType();
                if (filters.ContainsKey(eventType))
                {
                    InvokeInList(filters[eventType], ev);
                }
                InvokeInList(noFilters, ev);
            }
        }

        protected virtual void InvokeInList(IList<MBCEventHandler> handlers, Event ev)
        {
            foreach (MBCEventHandler handler in handlers)
            {
                handler(ev);
            }
        }

        protected virtual void RemoveEventHandler(MBCEventHandler del)
        {
            lock (this)
            {
                MethodInfo method = del.Method;
                object[] attributes = method.GetCustomAttributes(typeof(EventFilterAttribute), false);
                if (attributes.Length == 0)
                {
                    foreach (object o in attributes)
                    {
                        EventFilterAttribute attribute = (EventFilterAttribute)o;
                        if (filters.ContainsKey(attribute.Event))
                        {
                            filters[attribute.Event].Remove(del);
                        }
                    }
                }
                else
                {
                    noFilters.Remove(del);
                }
            }
        }
    }
}