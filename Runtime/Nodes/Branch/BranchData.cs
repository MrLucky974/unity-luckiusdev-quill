using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public sealed class BranchData
    {
        [SerializeField] private string m_text;
        public string Text => m_text;

        [SerializeField] private int m_conditionPortIndex;
        public int ConditionPortIndex => m_conditionPortIndex;
        
        [SerializeField] private int m_targetPortIndex;
        public int TargetPortIndex => m_targetPortIndex;

        public BranchData(string text, int targetIndex, int conditionIndex = -1)
        {
            m_text = text;
            m_targetPortIndex = targetIndex;
            m_conditionPortIndex = conditionIndex;
        }
    }
}