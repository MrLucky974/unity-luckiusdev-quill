using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;
using UnityEngine;

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
                .WithCapacity(PortCapacity.Single)
                .Build();
            
            context.AddInputPort<bool>($"{k_inputPortName}A")
                .WithCapacity(PortCapacity.Single)
                .Build();

            if (op == EBooleanOperator.NOT) return;
            
            context.AddInputPort<bool>($"{k_inputPortName}B")
                .WithCapacity(PortCapacity.Single)
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

            var aDefaultValue = false;
            if (op != EBooleanOperator.NONE)
            {
                GetInputPortByName($"{k_inputPortName}A").TryGetValue(out aDefaultValue);
            }

            var bDefaultValue = false;
            if (op != EBooleanOperator.NONE && op != EBooleanOperator.NOT)
            {
                GetInputPortByName($"{k_inputPortName}B").TryGetValue(out bDefaultValue);
            }
            
            return new BooleanOperatorRuntimeNode(op, aSideNodeIndex, bSideNodeIndex, aDefaultValue, bDefaultValue);
        }
    }
}