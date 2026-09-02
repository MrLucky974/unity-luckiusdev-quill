using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class MarkerRuntimeNode : FlowRuntimeNode
    {
        [SerializeField] private string m_markerName;
        public string MarkerName => m_markerName;
        
        public MarkerRuntimeNode(int nextNodeIndex, string markerName = "") : base(nextNodeIndex)
        {
            m_markerName = markerName;
        }
    }
}