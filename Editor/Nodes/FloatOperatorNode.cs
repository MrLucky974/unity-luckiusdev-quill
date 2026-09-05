using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Value/Operators", "", "Float Operator")]
    internal class FloatOperatorNode : BaseNode
    {
        public const string k_operatorOptionName = "Operator";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<EArithmeticOperator>(k_operatorOptionName)
                .WithDefaultValue(EArithmeticOperator.ADD)
                .Delayed();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            var op = GetOperatorMode();
            
            context.AddOutputPort<float>(k_outputPortName)
                .WithDisplayName("Output")
                .Build();
            
            context.AddInputPort<float>($"{k_inputPortName}A")
                .WithCapacity(PortCapacity.Single)
                .WithDisplayName("Input A")
                .Build();
            
            context.AddInputPort<float>($"{k_inputPortName}B")
                .WithCapacity(PortCapacity.Single)
                .WithDisplayName("Input B")
                .Build();
        }

        private EArithmeticOperator GetOperatorMode()
        {
            EArithmeticOperator op = EArithmeticOperator.ADD;
            GetNodeOptionByName(k_operatorOptionName)?.TryGetValue(out op);
            return op;
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var op = GetOperatorMode();
            var aSideNodeIndex = nodeMap[GetPreviousNode($"{k_inputPortName}A")];
            var bSideNodeIndex = nodeMap[GetPreviousNode($"{k_inputPortName}B")];

            var aValueReference = GetValueReference<float>($"{k_inputPortName}A", nodeMap);
            var bValueReference = GetValueReference<float>($"{k_inputPortName}B", nodeMap);
            
            return new FloatOperatorRuntimeNode(op, aValueReference, bValueReference);
        }
    }
}