using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MBC.Shared.Events
{
    public class EventFilter<T> where T : Delegate
    {
        private Dictionary<Type, List<Delegate>> filterMap;
        private object[] parameters = new object[1];

        public void AddEventHandler(T del)
        {
            lock (this)
            {
                Type parameterType = ParamTypeFromDelegate(del);
                if (!filterMap.ContainsKey(parameterType))
                {
                    filterMap.Add(parameterType, new List<Delegate>());
                }
                filterMap[parameterType].Add(del);
            }
        }

        public void InvokeEvent(Event ev)
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

        public void RemoveEventHandler(T del)
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
    }
}