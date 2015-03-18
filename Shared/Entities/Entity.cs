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
    /// <summary>
    /// Provides the base functionality for an MBC game entity.
    /// </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        private Dictionary<Type, List<Entity>> children = new Dictionary<Type, List<Entity>>();
        private Queue<EntityEventInvokeQueueItem> evQueue;
        private Dictionary<Type, List<MBCEventHandler>> filters = new Dictionary<Type, List<MBCEventHandler>>();
        private IDNumber id;
        private List<MBCEventHandler> noFilters = new List<MBCEventHandler>();
        private Entity parent;
        private Stopwatch parentTimer;

        /// <summary>
        /// Creates the entity based on the state of the parent that it will be added to.
        /// </summary>
        /// <param name="parent"></param>
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

        /// <summary>
        /// Invoked whenever an Event is created and invoked.
        /// </summary>
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

        /// <summary>
        /// Gets the stop watch object that provides the running time of the game.
        /// </summary>
        public Stopwatch GameTimer
        {
            get
            {
                return parentTimer;
            }
        }

        /// <summary>
        /// Gets the ID number associated with the entity.
        /// </summary>
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

        /// <summary>
        /// Not equals comparison is the NOT of the equals comparison.
        /// </summary>
        /// <param name="ent1"></param>
        /// <param name="ent2"></param>
        /// <returns></returns>
        public static bool operator !=(Entity ent1, Entity ent2)
        {
            return !(ent1 == ent2);
        }

        /// <summary>
        /// Compares two entity objects for equality.
        /// </summary>
        /// <param name="ent1"></param>
        /// <param name="ent2"></param>
        /// <returns>True if the ID and type of the two entities are equal, false otherwise.</returns>
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

        /// <summary>
        /// Compares the entity with another object for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }

        /// <summary>
        /// Compares the entity with another enttiy for equality.
        /// </summary>
        /// <param name="ent"></param>
        /// <returns></returns>
        public bool Equals(Entity ent)
        {
            return this == ent;
        }

        /// <summary>
        /// Gets the hash code of the entity.
        /// </summary>
        /// <returns>The ID of the entity.</returns>
        public override int GetHashCode()
        {
            return ID;
        }

        /// <summary>
        /// Adds an MBCEventHandler delegate to the events to invoke.
        /// </summary>
        /// <param name="del"></param>
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
                        //Console.WriteLine("Hello");
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

        /// <summary>
        /// Causes an event to perform its operation and invokes all specific handlers.
        /// </summary>
        /// <param name="ev"></param>
        protected void InvokeEvent(Event ev)
        {
            if (evQueue.Count == 0)
            {
                EventPerform(ev);
                while (evQueue.Count > 0)
                {
                    EntityEventInvokeQueueItem item = evQueue.Peek();
                    item.Entity.NotifyEvent(item.Event);
                    evQueue.Dequeue();
                }
            }
            else
            {
                EventPerform(ev);
            }
        }

        /// <summary>
        /// Removes an event handler from being invoked.
        /// </summary>
        /// <param name="del"></param>
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
        /// Finds the first unused ID number in a collection of children.
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        private static IDNumber FindID(IEnumerable<Entity> children)
        {
            IEnumerable<int> existingIds = children.Select(ent => (int)ent.ID);
            return Enumerable.Range(0, int.MaxValue).Except(existingIds).FirstOrDefault();
        }

        /// <summary>
        /// Enqueues an association between the entity and the provided event and invokes the
        /// operation of the event. Removes the same association from the queue if an
        /// exception is thrown by the event.
        /// </summary>
        /// <param name="ev"></param>
        private void EventPerform(Event ev)
        {
            EntityEventInvokeQueueItem item = new EntityEventInvokeQueueItem(this, ev);
            evQueue.Enqueue(item);
            try
            {
                ev.PerformOperation();
            }
            catch (Exception e)
            {
                Queue<EntityEventInvokeQueueItem> tmpQueue = new Queue<EntityEventInvokeQueueItem>();
                while (evQueue.Count > 0)
                {
                    if (evQueue.Peek() != item)
                    {
                        tmpQueue.Enqueue(evQueue.Dequeue());
                    }
                }
                while (tmpQueue.Count > 0)
                {
                    evQueue.Enqueue(tmpQueue.Dequeue());
                }
                throw e;
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

        /// <summary>
        /// Invokes all MBCEventHandler delegates in a list with the provided event as a parameter.
        /// </summary>
        /// <param name="handlers"></param>
        /// <param name="ev"></param>
        private void InvokeInList(IList<MBCEventHandler> handlers, Event ev)
        {
            foreach (MBCEventHandler handler in handlers)
            {
                try
                {
                    handler(ev);
                }
                catch (Exception e)
                {
                    if (ev.GetType() == typeof(ExceptionEvent))
                    {
                        throw e;
                    }
                    InvokeEvent(new ExceptionEvent(ev.Entity, e));
                }
            }
        }

        /// <summary>
        /// Invokes all attached MBCEventHandler delegates with the provided event.
        /// </summary>
        /// <param name="ev"></param>
        private void NotifyEvent(Event ev)
        {
            Type eventType = ev.GetType();
            if (filters.ContainsKey(eventType))
            {
                InvokeInList(filters[eventType], ev);
            }
            InvokeInList(noFilters, ev);
        }

        /// <summary>
        /// Provides an association between an entity and an event. Used to keep track of
        /// which entity has had an event created from.
        /// </summary>
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