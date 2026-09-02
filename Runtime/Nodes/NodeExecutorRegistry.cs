using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    public static class NodeExecutorRegistry
    {
        private static Dictionary<Type, INodeExecutor> s_executors;

        public static IReadOnlyDictionary<Type, INodeExecutor> Executors
        {
            get
            {
                if (s_executors == null)
                {
                    Build();
                }
                return s_executors;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Build()
        {
            s_executors = new Dictionary<Type, INodeExecutor>();

            var executorTypes = UnityEngine.Assemblies.CurrentAssemblies.GetLoadedAssemblies()            .SelectMany(SafeGetTypes)
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(ImplementsNodeExecutorInterface);

            foreach (var executorType in executorTypes)
            {
                RegisterExecutor(executorType);
            }

            Debug.Log($"[NodeExecutorRegistry] Registered {s_executors.Count} node executor(s).");
        }

        private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                // Some assemblies (e.g. editor-only or partially loaded) may throw here.
                return e.Types.Where(t => t != null);
            }
        }

        private static bool ImplementsNodeExecutorInterface(Type type)
        {
            return type.GetInterfaces().Any(IsNodeExecutorInterface);
        }

        private static bool IsNodeExecutorInterface(Type interfaceType)
        {
            return interfaceType.IsGenericType &&
                   interfaceType.GetGenericTypeDefinition() == typeof(INodeExecutor<>);
        }

        private static void RegisterExecutor(Type executorType)
        {
            var interfaceType = executorType.GetInterfaces().First(IsNodeExecutorInterface);
            var expectedNodeType = interfaceType.GetGenericArguments()[0];

            var attribute = executorType.GetCustomAttribute<NodeExecutorAttribute>();

            if (!ValidateExecutor(executorType, attribute, expectedNodeType))
            {
                return;
            }

            if (s_executors.ContainsKey(attribute.NodeType))
            {
                Debug.LogError("[NodeExecutorRegistry] Duplicate executor for node type " +
                                $"'{attribute.NodeType.Name}'. '{executorType.Name}' conflicts with an " +
                                "already registered executor. Skipping.");
                return;
            }

            INodeExecutor instance;
            try
            {
                instance = Activator.CreateInstance(executorType) as INodeExecutor;
            }
            catch (Exception e)
            {
                Debug.LogError("[NodeExecutorRegistry] Failed to instantiate executor " +
                                $"'{executorType.Name}'. It must have an accessible parameterless " +
                                $"constructor. Exception: {e}");
                return;
            }

            s_executors.Add(attribute.NodeType, instance);
        }

        private static bool ValidateExecutor(Type executorType, NodeExecutorAttribute attribute, Type expectedNodeType)
        {
            if (attribute == null)
            {
                Debug.LogError($"[NodeExecutorRegistry] '{executorType.Name}' implements " +
                                $"IExecutor<{expectedNodeType.Name}> but is missing the " +
                                "[NodeExecutor] attribute. Skipping registration.");
                return false;
            }

            if (attribute.NodeType != expectedNodeType)
            {
                Debug.LogError($"[NodeExecutorRegistry] '{executorType.Name}' declares " +
                                $"[NodeExecutor(typeof({attribute.NodeType?.Name ?? "null"}))] but implements " +
                                $"INodeExecutor<{expectedNodeType.Name}>. The attribute's type must " +
                                "match the interface's generic argument. Skipping registration.");
                return false;
            }

            return true;
        }
    }
}