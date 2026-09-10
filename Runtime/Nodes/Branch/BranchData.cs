using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public sealed class BranchData
    {
        [SerializeField] private string m_text;
        public string Text => m_text;
        
        [SerializeField] private int m_targetPortIndex;
        public int TargetPortIndex => m_targetPortIndex;

        [SerializeReference] private ValueReference<bool> m_valueReference;
        public ValueReference<bool> ValueReference => m_valueReference;

        public BranchData(string text, int targetIndex, ValueReference<bool> valueReference)
        {
            m_text = text;
            m_targetPortIndex = targetIndex;
            m_valueReference = valueReference;
        }
    }
}