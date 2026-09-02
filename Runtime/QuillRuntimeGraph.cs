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

        public static QuillRuntimeGraph Create(List<RuntimeNode> nodes)
        {
            var instance = CreateInstance<QuillRuntimeGraph>();
            instance.m_nodes = nodes;
            return instance;
        }
    }
}