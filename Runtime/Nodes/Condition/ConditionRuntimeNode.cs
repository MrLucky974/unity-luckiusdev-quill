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
        
        [SerializeField] private int m_conditionNodeIndex;
        public int ConditionNodeIndex => m_conditionNodeIndex;

        [SerializeField] private bool m_defaultValue;
        public bool DefaultValue => m_defaultValue;

        public ConditionRuntimeNode(int trueNodeIndex, int falseNodeIndex, int conditionIndex = -1, bool defaultValue = false)
        {
            m_trueNodeIndex = trueNodeIndex;
            m_falseNodeIndex = falseNodeIndex;
            m_conditionNodeIndex = conditionIndex;
            m_defaultValue = defaultValue;
        }
    }
}