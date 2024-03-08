using System;
using System.Collections.Generic;

namespace Assets.Scripts.Util
{
    public static class PredefinedAssemblyUtil
    {
        enum AssemblyType
        {
            AssemblyCSharp,
            AssemblyCSharpEditor,
            AssemblyCSharpEditorFirstPass,
            AssemblyCSharpFirstPass
        }

        static AssemblyType? GetAssemblyType(string assemblyName)
        {
            return assemblyName switch
            {
                "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
                "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
                "Assembly-CSharp-Editor-firstpass" => AssemblyType.AssemblyCSharpEditorFirstPass,
                "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
                _ => null
            };
        }

        public static List<Type> GetTypes(Type interfaceType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
            List<Type> types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                AssemblyType? assemblyType = GetAssemblyType(assembly.GetName().Name);
                if (assemblyType == null) continue;
                assemblyTypes.Add((AssemblyType)assemblyType, assembly.GetTypes());
            }

            foreach (var values in assemblyTypes.Values)
            {
                AddTypesFromAssembly(values, interfaceType, types);
            }

            return types;
        }

        private static void AddTypesFromAssembly(Type[] assembly, Type interfaceType, ICollection<Type> types)
        {
            if (assembly == null) return;
            foreach (var assemblyType in assembly)
            {
                if (assemblyType != interfaceType && interfaceType.IsAssignableFrom(assemblyType)) types.Add(assemblyType);
            }
        }
    }
}
