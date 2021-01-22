using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PolishNewsHarvesterSdk.Targets;

namespace PolishNewsHarvesterSdk
{
    public static class TypeLoaderExtensions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetTypesWithInterface<TInterface>(this Assembly asm) where TInterface : class
        {
            var it = typeof(TInterface);
            return asm.GetLoadableTypes().Where(it.IsAssignableFrom).Where(x => x.IsClass).ToList();
        }
    }
}