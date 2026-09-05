using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class BooleanOperatorRuntimeNode : OperatorRuntimeNode<bool, EBooleanOperator>
    {
        public BooleanOperatorRuntimeNode(EBooleanOperator op, ValueReference<bool> aSideValueReference, ValueReference<bool> bSideValueReference) : base(op, aSideValueReference, bSideValueReference)
        {
        }

        protected override bool Calculate(EBooleanOperator op, bool aValue, bool bValue)
        {
            return op switch
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