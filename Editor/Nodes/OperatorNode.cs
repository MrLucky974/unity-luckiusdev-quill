using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    internal abstract class OperatorNode<TValue, TEnum> : BaseNode where TEnum : Enum
    {
        public const string k_operatorOptionName = "Operator";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<TEnum>(k_operatorOptionName)
                .WithDefaultValue(GetDefaultEnumValue())
                .Delayed();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            var op = GetOperatorMode();
            
            context.AddOutputPort<TValue>(k_outputPortName)
                .WithDisplayName("Output")
                .Build();
            
            context.AddInputPort<TValue>($"{k_inputPortName}A")
                .WithCapacity(PortCapacity.Single)
                .WithDisplayName(IsSingleOperator(op) ? "Input" : "Input A")
                .Build();

            if (IsSingleOperator(op)) return;
            
            context.AddInputPort<TValue>($"{k_inputPortName}B")
                .WithCapacity(PortCapacity.Single)
                .WithDisplayName("Input B")
                .Build();
        }

        private TEnum GetOperatorMode()
        {
            TEnum op = GetDefaultEnumValue();
            GetNodeOptionByName(k_operatorOptionName)?.TryGetValue(out op);
            return op;
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var op = GetOperatorMode();
            var aSideNodeIndex = nodeMap[GetPreviousNode($"{k_inputPortName}A")];
            var bSideNodeIndex = nodeMap[GetPreviousNode($"{k_inputPortName}B")];

            var aValueReference = GetValueReference<TValue>($"{k_inputPortName}A", nodeMap);
            var bValueReference = GetValueReference<TValue>($"{k_inputPortName}B", nodeMap);
            
            return CreateRuntimeNode(op, aValueReference, bValueReference);
        }

        protected abstract TEnum GetDefaultEnumValue();
        protected abstract bool IsSingleOperator(TEnum enumValue);
        protected abstract OperatorRuntimeNode<TValue, TEnum> CreateRuntimeNode(TEnum op, ValueReference<TValue> aValueReference, ValueReference<TValue> bValueReference);
    }
}