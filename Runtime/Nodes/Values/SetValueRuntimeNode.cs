using LuckiusDev.Quill.Nodes;
using System;
using UnityEngine;

namespace LuckiusDev.Quill
{
    [Serializable]
    public class SetValueRuntimeNode<T> : FlowRuntimeNode
    {
        [SerializeField] private Hash128 m_variableIdentifier;
        [SerializeReference] private ValueReference<T> m_newValueReference;

        public SetValueRuntimeNode(Hash128 variableIdentifier, ValueReference<T> newValueReference, int nextNodeIndex) : base(nextNodeIndex)
        {
            m_variableIdentifier = variableIdentifier;
            m_newValueReference = newValueReference;
        }

        public void SetValue(DialogueDirector ctx)
        {
            if (ctx.TryGetVariable<T>(m_variableIdentifier, out var variable))
            {
                var newValue = m_newValueReference.GetValue(ctx);
                variable.SetValue(newValue);
            }
        }
    }
}