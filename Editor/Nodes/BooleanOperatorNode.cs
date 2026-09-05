using System;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Value/Operators", "", "Boolean Operator")]
    internal class BooleanOperatorNode : OperatorNode<bool, EBooleanOperator>
    {
        protected override EBooleanOperator GetDefaultEnumValue()
        {
            return EBooleanOperator.OR;
        }

        protected override bool IsSingleOperator(EBooleanOperator enumValue)
        {
            return enumValue == EBooleanOperator.NOT;
        }

        protected override OperatorRuntimeNode<bool, EBooleanOperator> CreateRuntimeNode(EBooleanOperator op, ValueReference<bool> aValueReference, ValueReference<bool> bValueReference)
        {
            return new BooleanOperatorRuntimeNode(op, aValueReference, bValueReference);
        }
    }
}