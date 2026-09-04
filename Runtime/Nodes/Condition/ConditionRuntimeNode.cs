using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class ConditionRuntimeNode : RuntimeNode
    {
        [SerializeField] private int m_trueNodeIndex;
        public int TrueNodeIndex => m_trueNodeIndex;
        
        [SerializeField] private int m_falseNodeIndex;
        public int FalseNodeIndex => m_falseNodeIndex;

        [SerializeReference] private ValueReference<bool> m_valueReference;
        public ValueReference<bool> ValueReference => m_valueReference;

        public ConditionRuntimeNode(int trueNodeIndex, int falseNodeIndex, ValueReference<bool> valueReference)
        {
            m_trueNodeIndex = trueNodeIndex;
            m_falseNodeIndex = falseNodeIndex;
            m_valueReference = valueReference;
        }
    }
}