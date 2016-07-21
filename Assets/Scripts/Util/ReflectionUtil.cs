using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Util
{
    static public class ReflectionUtil
    {
        static public IEnumerable<Type> GetDerivedTypes<T>()
        {
            var baseType = typeof(T);
            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            var actionTypes = assemblyTypes.Where(t => baseType.IsAssignableFrom(t));

            return actionTypes.Where(t => !t.IsAbstract && !t.IsInterface);
        }
    }
}
