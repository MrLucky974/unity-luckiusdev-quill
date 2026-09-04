using System.Collections.Generic;
using LuckiusDev.Quill.Nodes;
using UnityEngine;

namespace LuckiusDev.Quill
{
    public sealed class QuillRuntimeGraph : ScriptableObject
    {
        [SerializeReference]
        private List<RuntimeNode> m_nodes = new();
        public IReadOnlyList<RuntimeNode> Nodes => m_nodes;

        [SerializeReference] private List<BlackboardVariable> m_variables = new();
        public IReadOnlyList<BlackboardVariable> Variables => new List<BlackboardVariable>(m_variables);

        public static QuillRuntimeGraph Create(List<RuntimeNode> nodes, List<BlackboardVariable> variables)
        {
            var instance = CreateInstance<QuillRuntimeGraph>();
            instance.m_nodes = nodes;
            instance.m_variables = variables;
            return instance;
        }
    }
}