using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Value/Operators", "", "Boolean Operator")]
    internal class BooleanOperatorNode : BaseNode
    {
        public const string k_operatorOptionName = "BooleanOperator";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<EBooleanOperator>(k_operatorOptionName)
                .WithDefaultValue(EBooleanOperator.OR)
                .Delayed();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            var op = GetOperatorMode();
            if (op == EBooleanOperator.NONE) return;
            
            context.AddOutputPort<bool>(k_outputPortName)
                .WithDisplayName("Output")
                .Build();
            
            context.AddInputPort<bool>($"{k_inputPortName}A")
                .WithCapacity(PortCapacity.Single)
                .WithDisplayName(op == EBooleanOperator.NOT ? "Input" : "Input A")
                .Build();

            if (op == EBooleanOperator.NOT) return;
            
            context.AddInputPort<bool>($"{k_inputPortName}B")
                .WithCapacity(PortCapacity.Single)
                .WithDisplayName("Input B")
                .Build();
        }

        private EBooleanOperator GetOperatorMode()
        {
            EBooleanOperator op = EBooleanOperator.NONE;
            GetNodeOptionByName(k_operatorOptionName)?.TryGetValue(out op);
            return op;
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var op = GetOperatorMode();
            var aSideNodeIndex = nodeMap[GetPreviousNode($"{k_inputPortName}A")];
            var bSideNodeIndex = nodeMap[GetPreviousNode($"{k_inputPortName}B")];

            var aValueReference = GetValueReference<bool>($"{k_inputPortName}A", nodeMap);
            var bValueReference = GetValueReference<bool>($"{k_inputPortName}B", nodeMap);
            
            return new BooleanOperatorRuntimeNode(op, aValueReference, bValueReference);
        }
    }
}