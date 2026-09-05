using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class FloatOperatorRuntimeNode : OperatorRuntimeNode<float, EArithmeticOperator>
    {
        public FloatOperatorRuntimeNode(EArithmeticOperator op, ValueReference<float> aSideValueReference, ValueReference<float> bSideValueReference) : base(op, aSideValueReference, bSideValueReference)
        {
        }

        protected override float Calculate(EArithmeticOperator op, float aValue, float bValue)
        {
            return op switch
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