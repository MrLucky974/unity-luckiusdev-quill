using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class BooleanOperatorRuntimeNode : BooleanNode
    {
        [SerializeField] private EBooleanOperator m_operatorMode;
        [SerializeField] private int m_aSideNodeIndex;
        [SerializeField] private bool m_aSideDefaultValue;
        [SerializeField] private int m_bSideNodeIndex;
        [SerializeField] private bool m_bSideDefaultValue;

        public BooleanOperatorRuntimeNode(EBooleanOperator op, int aSideNodeIndex, int bSideNodeIndex, bool aSideDefaultValue = false, bool bSideDefaultValue = false)
        {
            m_operatorMode = op;
            
            m_aSideNodeIndex = aSideNodeIndex;
            m_aSideDefaultValue = aSideDefaultValue;
            
            m_bSideNodeIndex = bSideNodeIndex;
            m_bSideDefaultValue = bSideDefaultValue;
        }

        public override bool Evaluate(DialogueDirector ctx)
        {
            bool aValue = (ctx.GetNode(m_aSideNodeIndex) as BooleanNode)?.Evaluate(ctx) ?? m_aSideDefaultValue;
            bool bValue = (ctx.GetNode(m_bSideNodeIndex) as BooleanNode)?.Evaluate(ctx) ?? m_bSideDefaultValue;

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