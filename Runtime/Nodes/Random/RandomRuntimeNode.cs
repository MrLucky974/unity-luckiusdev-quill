using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class RandomRuntimeNode : RuntimeNode
    {
        [SerializeField] private List<int> m_nodeIndexes;
        public IReadOnlyList<int> NodeIndexes => m_nodeIndexes;
        
        public RandomRuntimeNode(List<int> nodeIndexes)
        {
            m_nodeIndexes = nodeIndexes;
        }
    }
}