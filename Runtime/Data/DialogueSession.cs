using System.Collections.Generic;
using System.Linq;
using LuckiusDev.Quill.Nodes;
using UnityEngine;

namespace LuckiusDev.Quill
{
    /// <summary>
    /// Holds the runtime state of a single dialogue playback session.
    /// </summary>
    public sealed class DialogueSession
    {
        private int m_currentNodeIndex;

        private readonly IReadOnlyList<RuntimeNode> m_nodes;
        private readonly IReadOnlyList<BlackboardVariable> m_variables;

        public int CurrentNodeIndex
        {
            get => m_currentNodeIndex;
            set => m_currentNodeIndex = value;
        }

        private DialogueSession(QuillRuntimeGraph graph)
        {
            m_nodes = graph.Nodes;
            m_variables = graph.Variables
                .Select(variable => variable.Clone())
                .ToList();
            m_currentNodeIndex = 0;
        }

        public static DialogueSession CreateFromGraph(QuillRuntimeGraph graph) => new(graph);

        public bool TryGetNode<T>(int index, out T node) where T : RuntimeNode
        {
            if (index < 0 || index >= m_nodes.Count)
            {
                node = null;
                return false;
            }

            if (m_nodes[index] is not T typedNode)
            {
                Debug.LogWarning($"[DialogueSession] Node at index '{index}' is not of specified type '{typeof(T)}'.");
                node = null;
                return false;
            }

            node = typedNode;
            return true;
        }

        public bool TryGetVariable<T>(Hash128 id, out BlackboardVariable<T> outVariable)
        {
            foreach (var variable in m_variables)
            {
                if (variable.ID != id) continue;
                outVariable = (BlackboardVariable<T>)variable;
                return true;
            }

            Debug.LogWarning($"[DialogueSession] Variable with identifier '{id}' not found.");
            outVariable = new BlackboardVariable<T>(id);
            return false;
        }
    }
}