using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public abstract class FlowRuntimeNode : RuntimeNode
    {
        [SerializeField] private int m_nextNodeIndex;
        public int NextNodeIndex => m_nextNodeIndex;

        protected FlowRuntimeNode(int nextNodeIndex)
        {
            m_nextNodeIndex = nextNodeIndex;
        }
    }
}