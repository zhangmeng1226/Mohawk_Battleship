using MBC.Shared.Attributes;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MBC.Shared
{
    public abstract class Entity : IEquatable<Entity>
    {
        private Queue<Event> evQueue = new Queue<Event>();
        private Dictionary<Type, List<MBCEventHandler>> filters = new Dictionary<Type, List<MBCEventHandler>>();
        private IDNumber id;
        private List<MBCEventHandler> noFilters = new List<MBCEventHandler>();

        public Entity(IDNumber id)
        {
            ID = id;
        }

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

        public IDNumber ID
        {
            get
            {
                return id;
            }
            protected internal set
            {
                id = value;
            }
        }

        public static IDNumber FindID(IEnumerable<Entity> children)
        {
            IEnumerable<int> existingIds = children.Select(ent => (int)ent.ID);
            return Enumerable.Range(0, int.MaxValue).Except(existingIds).FirstOrDefault();
        }

        public static bool operator !=(Entity ent1, Entity ent2)
        {
            return !(ent1 == ent2);
        }

        public static bool operator ==(Entity ent1, Entity ent2)
        {
            if (Object.ReferenceEquals(ent1, ent2))
            {
                return true;
            }
            if (Object.ReferenceEquals(ent1, null) || Object.ReferenceEquals(ent2, null))
            {
                return false;
            }
            return ent1.ID == ent2.ID && ent1.GetType() == ent2.GetType();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }

        public bool Equals(Entity ent)
        {
            return this == ent;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        protected virtual void AddEventHandler(MBCEventHandler del)
        {
            MethodInfo method = del.Method;
            object[] attributes = method.GetCustomAttributes(typeof(EventFilterAttribute), false);
            if (attributes.Length > 0)
            {
                foreach (object o in attributes)
                {
                    EventFilterAttribute attribute = (EventFilterAttribute)o;
                    if (attribute.Event == typeof(MatchAddPlayerEvent))
                    {
                        Console.WriteLine("Hello");
                    }
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

        protected void InvokeEvent(Event ev)
        {
            if (evQueue.Count == 0)
            {
                evQueue.Enqueue(ev);
                while (evQueue.Count > 0)
                {
                    Event evInQueue = evQueue.Dequeue();
                    if (evInQueue.GetType() == typeof(MatchAddPlayerEvent))
                    {
                        Console.WriteLine("Hello");
                    }
                    evInQueue.PerformOperation();
                    Type eventType = evInQueue.GetType();
                    if (evInQueue.GetType() == typeof(MatchAddPlayerEvent))
                    {
                        Console.WriteLine("Hello");
                    }
                    if (filters.ContainsKey(eventType))
                    {
                        InvokeInList(filters[eventType], evInQueue);
                    }
                    InvokeInList(noFilters, evInQueue);
                }
            }
            else
            {
                evQueue.Enqueue(ev);
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

        /// <summary>
        /// Finds the Type object of the current Type that is directly a sub class of Entity.
        /// </summary>
        /// <returns></returns>
        private Type GetEntityType()
        {
            Type result = GetType();
            while (result.BaseType != typeof(Entity))
            {
                result = result.BaseType;
            }
            return result;
        }
    }
}