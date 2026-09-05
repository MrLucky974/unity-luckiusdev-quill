using System;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Value/Operators", "", "Float Operator")]
    internal class FloatOperatorNode : OperatorNode<float, EArithmeticOperator>
    {
        protected override EArithmeticOperator GetDefaultEnumValue()
        {
            return EArithmeticOperator.ADD;
        }

        protected override bool IsSingleOperator(EArithmeticOperator enumValue)
        {
            return false;
        }

        protected override OperatorRuntimeNode<float, EArithmeticOperator> CreateRuntimeNode(EArithmeticOperator op, ValueReference<float> aValueReference, ValueReference<float> bValueReference)
        {
            return new FloatOperatorRuntimeNode(op, aValueReference, bValueReference);
        }
    }
}