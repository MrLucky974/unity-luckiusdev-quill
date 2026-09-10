using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public abstract class OperatorRuntimeNode<TValue, TEnum> : ValueRuntimeNode<TValue>
    {
        [SerializeField] private TEnum m_operatorMode;
        [SerializeReference] private ValueReference<TValue> m_aSideValueReference;
        [SerializeReference] private ValueReference<TValue> m_bSideValueReference;

        protected OperatorRuntimeNode(TEnum op, ValueReference<TValue> aSideValueReference, ValueReference<TValue> bSideValueReference)
        {
            m_operatorMode = op;

            m_aSideValueReference = aSideValueReference;
            m_bSideValueReference = bSideValueReference;
        }

        public override TValue Evaluate(IDialogueContext ctx)
        {
            TValue aValue = m_aSideValueReference != null ? m_aSideValueReference.GetValue(ctx) : default;
            TValue bValue = m_bSideValueReference != null ? m_bSideValueReference.GetValue(ctx) : default;

            return Calculate(m_operatorMode, aValue, bValue);
        }

        protected abstract TValue Calculate(TEnum op, TValue aValue, TValue bValue);
    }
}