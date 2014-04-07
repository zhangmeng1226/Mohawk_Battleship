using MBC.Shared.Attributes;
using MBC.Shared.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MBC.Shared.Entities
{
    public abstract class Entity : IEquatable<Entity>
    {
        private Dictionary<Type, List<Entity>> children = new Dictionary<Type, List<Entity>>();
        private Queue<EntityEventInvokeQueueItem> evQueue;
        private Dictionary<Type, List<MBCEventHandler>> filters = new Dictionary<Type, List<MBCEventHandler>>();
        private IDNumber id;
        private List<MBCEventHandler> noFilters = new List<MBCEventHandler>();
        private Entity parent;
        private Stopwatch parentTimer;

        public Entity(Entity parent)
        {
            this.parent = parent;
            if (parent == null)
            {
                parentTimer = new Stopwatch();
                evQueue = new Queue<EntityEventInvokeQueueItem>();
                id = 0;
            }
            else
            {
                Type myType = GetType();
                var parentChildren = parent.children;
                if (!parentChildren.ContainsKey(myType))
                {
                    parentChildren.Add(myType, new List<Entity>());
                }
                List<Entity> children = parentChildren[myType];
                id = FindID(children);
                children.Add(this);
                evQueue = parent.evQueue;
                parentTimer = parent.parentTimer;
                OnEvent += parent.NotifyEvent;
            }
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

        protected internal Stopwatch GameTimerParent
        {
            get
            {
                return parentTimer;
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

        protected static void InvokeInList(IList<MBCEventHandler> handlers, Event ev)
        {
            foreach (MBCEventHandler handler in handlers)
            {
                handler(ev);
            }
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
                evQueue.Enqueue(new EntityEventInvokeQueueItem(this, ev));
                ev.PerformOperation();
                while (evQueue.Count > 0)
                {
                    EntityEventInvokeQueueItem item = evQueue.Peek();
                    item.Entity.NotifyEvent(item.Event);
                    evQueue.Dequeue();
                }
            }
            else
            {
                evQueue.Enqueue(new EntityEventInvokeQueueItem(this, ev));
                ev.PerformOperation();
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

        private void NotifyEvent(Event ev)
        {
            Type eventType = ev.GetType();
            if (filters.ContainsKey(eventType))
            {
                InvokeInList(filters[eventType], ev);
            }
            InvokeInList(noFilters, ev);
        }

        private class EntityEventInvokeQueueItem
        {
            public EntityEventInvokeQueueItem(Entity entity, Event ev)
            {
                Entity = entity;
                Event = ev;
            }

            public Entity Entity { get; set; }

            public Event Event { get; set; }
        }
    }
}