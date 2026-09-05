using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class FloatOperatorRuntimeNode : FloatRuntimeNode
    {
        [SerializeField] private EArithmeticOperator m_operatorMode;
        [SerializeReference] private ValueReference<float> m_aSideValueReference;
        [SerializeReference] private ValueReference<float> m_bSideValueReference;

        public FloatOperatorRuntimeNode(EArithmeticOperator op, ValueReference<float> aSideValueReference, ValueReference<float> bSideValueReference)
        {
            m_operatorMode = op;

            m_aSideValueReference = aSideValueReference;
            m_bSideValueReference = bSideValueReference;
        }

        public override float Evaluate(DialogueDirector ctx)
        {
            float aValue = m_aSideValueReference?.GetValue(ctx) ?? 0F;
            float bValue = m_bSideValueReference?.GetValue(ctx) ?? 0F;

            return m_operatorMode switch
            {
                EArithmeticOperator.ADD => aValue + bValue,
                EArithmeticOperator.SUB => aValue - bValue,
                EArithmeticOperator.MUL => aValue * bValue,
                EArithmeticOperator.DIV => aValue / bValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}