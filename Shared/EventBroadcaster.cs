using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MBC.Shared.Events
{
    public class EventBroadcaster
    {
        private Dictionary<Type, List<Delegate>> filterMap;
        private object[] parameters = new object[1];

        private Type typeRestriction;

        public EventBroadcaster(Type typeRestriction)
        {
            this.typeRestriction = typeRestriction;
        }

        public static EventBroadcaster operator -(EventBroadcaster filter, Delegate del)
        {
            filter.AddEventHandler(del);
            return filter;
        }

        public static EventBroadcaster operator +(EventBroadcaster filter, Delegate del)
        {
            filter.RemoveEventHandler(del);
            return filter;
        }

        protected internal void InvokeEvent(Event ev)
        {
            Type eventType = ev.GetType();
            parameters[0] = ev;
            if (!filterMap.ContainsKey(eventType))
            {
                return;
            }
            foreach (Delegate handler in filterMap[eventType])
            {
                try
                {
                    handler.Method.Invoke(handler.Target, parameters);
                }
                catch (Exception e)
                {
                    //Some type of exception was thrown so we will ignore it/log it.
                }
            }
        }

        private void AddEventHandler(Delegate del)
        {
            lock (this)
            {
                Type parameterType = ParamTypeFromDelegate(del);
                if (!parameterType.IsSubclassOf(typeRestriction))
                {
                    throw new InvalidOperationException(String.Format("The subscribing method does not contain a parameter a subclass of {0}", typeRestriction.Name));
                }
                if (!filterMap.ContainsKey(parameterType))
                {
                    filterMap.Add(parameterType, new List<Delegate>());
                }
                filterMap[parameterType].Add(del);
            }
        }

        private Type ParamTypeFromDelegate(Delegate del)
        {
            MethodInfo method = del.GetType().GetMethod("Invoke");
            if (method.GetParameters().Count() != 1)
            {
                throw new InvalidOperationException("Must be method with 1 parameter.");
            }
            ParameterInfo firstParam = method.GetParameters()[0];
            return firstParam.ParameterType;
        }

        private void RemoveEventHandler(Delegate del)
        {
            lock (this)
            {
                Type parameterType = ParamTypeFromDelegate(del);
                if (!filterMap.ContainsKey(parameterType))
                {
                    return;
                }
                filterMap[parameterType].Remove(del);
            }
        }
    }
}