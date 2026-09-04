using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class BooleanOperatorRuntimeNode : BooleanRuntimeNode
    {
        [SerializeField] private EBooleanOperator m_operatorMode;
        [SerializeReference] private ValueReference<bool> m_aSideValueReference;
        [SerializeReference] private ValueReference<bool> m_bSideValueReference;

        public BooleanOperatorRuntimeNode(EBooleanOperator op, ValueReference<bool> aSideValueReference, ValueReference<bool> bSideValueReference)
        {
            m_operatorMode = op;

            m_aSideValueReference = aSideValueReference;
            m_bSideValueReference = bSideValueReference;
        }

        public override bool Evaluate(DialogueDirector ctx)
        {
            bool aValue = m_aSideValueReference?.GetValue(ctx) ?? false;
            bool bValue = m_bSideValueReference?.GetValue(ctx) ?? false;

            return m_operatorMode switch
            {
                EBooleanOperator.AND => aValue && bValue,
                EBooleanOperator.OR => aValue || bValue,
                EBooleanOperator.XOR => aValue ^ bValue,
                EBooleanOperator.NOT => !aValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}