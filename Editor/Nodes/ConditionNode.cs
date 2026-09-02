using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Flow", "", "Condition")]
    internal class ConditionNode : BaseNode
    {
        public const string k_conditionPortName = "Condition";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);

            context.AddInputPort<bool>(k_conditionPortName)
                .WithCapacity(PortCapacity.Single)
                .Build();
            
            context.AddOutputPort($"True{k_outputPortName}")
                .WithDisplayName("True")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .WithCapacity(PortCapacity.Single)
                .Build();
            
            context.AddOutputPort($"False{k_outputPortName}")
                .WithDisplayName("False")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .WithCapacity(PortCapacity.Single)
                .Build();
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var trueNodeIndex = nodeMap[GetNextNode($"True{k_outputPortName}")];
            var falseNodeIndex = nodeMap[GetNextNode($"False{k_outputPortName}")];

            var conditionNode = GetPreviousNode(k_conditionPortName);
            var conditionNodeIndex = nodeMap[conditionNode];

            GetInputPortByName(k_conditionPortName).TryGetValue<bool>(out var defaultValue);
            return new ConditionRuntimeNode(trueNodeIndex, falseNodeIndex, conditionNodeIndex, defaultValue);
        }
    }
}