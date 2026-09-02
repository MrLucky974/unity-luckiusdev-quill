using System;
using System.Collections.Generic;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Flow", "", "Random")]
    internal class RandomNode : BaseNode
    {
        public const string k_portCountOptionName = "PortCount";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            var portCountOption = context.AddOption<int>(k_portCountOptionName)
                .WithDefaultValue(2)
                .Delayed();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);
            
            var portCount = GetPortCount();

            for (int i = 0; i < portCount; i++)
            {
                context.AddOutputPort($"{k_outputPortName}{i}")
                    .WithDisplayName($"Output {i + 1}")
                    .Build();
            }
        }

        private int GetPortCount()
        {
            var portCountOption = GetNodeOptionByName(k_portCountOptionName);
            portCountOption.TryGetValue<int>(out var portCount);
            return portCount;
        }
        
        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var portCount = GetPortCount();
            var indices = new List<int>();

            for (int i = 0; i < portCount; i++)
            {
                var nodeIndex = nodeMap[GetNextNode($"{k_outputPortName}{i}")];
                indices.Add(nodeIndex);
            }
            
            return new RandomRuntimeNode(indices);
        }
    }
}