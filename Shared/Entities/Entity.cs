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
        private static Dictionary<Type, HashSet<WeakReference>> idMappings = new Dictionary<Type, HashSet<WeakReference>>();
        private Queue<Event> evQueue = new Queue<Event>();
        private Dictionary<Type, List<MBCEventHandler>> filters = new Dictionary<Type, List<MBCEventHandler>>();
        private IDNumber id;
        private List<MBCEventHandler> noFilters = new List<MBCEventHandler>();

        public Entity()
        {
            id = GetNewId(this);
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

        protected abstract Type GetEntityType();

        protected void InvokeEvent(Event ev)
        {
            if (evQueue.Count == 0)
            {
                evQueue.Enqueue(ev);
                while (evQueue.Count > 0)
                {
                    Event evInQueue = evQueue.Dequeue();
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

        private static IDNumber GetNewId(Entity ent)
        {
            Type entityType = ent.GetEntityType();
            if (!idMappings.ContainsKey(entityType))
            {
                idMappings.Add(entityType, new HashSet<WeakReference>());
            }
            List<int> excepted = new List<int>(idMappings[entityType].Count);
            foreach (WeakReference entRef in idMappings[entityType])
            {
                if (entRef.IsAlive)
                {
                    Entity entity = (Entity)entRef.Target;
                    excepted.Add(entity.ID);
                }
            }
            idMappings[entityType].Add(new WeakReference(ent));
            return Enumerable.Range(0, int.MaxValue).Except(excepted).First();
        }
    }
}