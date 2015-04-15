using System;
using System.Reflection;

namespace SalvagerEngine.Utilities.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsOfTypeOrSubClassOf(this Type me, Type type)
        {        
#if WINDOWS8
            return me == type || me.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
#else
            return me == type || me.IsSubclassOf(type);
#endif
        }
    }
}
