using System;
using System.Collections.Generic;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Flow", "", "Branch")]
    internal class BranchNode : BaseNode
    {
        public const string k_portCountOptionName = "PortCount";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            var portCountOption = context.AddOption<int>(k_portCountOptionName)
                .WithDefaultValue(2)
                .Delayed()
                .Build();

            var portCount = 2;
            portCountOption?.TryGetValue(out portCount);
            for (int i = 0; i < portCount; i++)
            {
                context.AddOption<string>($"Choice{i}")
                    .WithDisplayName($"Choice {i + 1}");
            }
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);

            int portCount = GetPortCount();
            for (int i = 0; i < portCount; i++)
            {
                context.AddInputPort<bool>($"Condition{i}")
                    .WithDisplayName($"Condition {i + 1}")
                    .WithCapacity(PortCapacity.Single)
                    .WithDefaultValue(true)
                    .Build();
                
                context.AddOutputPort($"{k_outputPortName}{i}")
                    .WithDisplayName($"Output {i + 1}")
                    .WithCapacity(PortCapacity.Single)
                    .Build();
            }
        }

        private int GetPortCount()
        {
            var portCountOption = GetNodeOptionByName(k_portCountOptionName);
            portCountOption.TryGetValue<int>(out var portCount);
            return portCount;
        }

        private string GetChoiceText(int choiceIndex)
        {
            if (choiceIndex < 0 || choiceIndex > GetPortCount()) throw new ArgumentOutOfRangeException(nameof(choiceIndex));

            var option = GetNodeOptionByName($"Choice{choiceIndex}");

            option.TryGetValue(out string text);
            return text;
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            int portCount = GetPortCount();
            List<BranchData> branches = new();
            for (int i = 0; i < portCount; i++)
            {
                var choiceText = GetChoiceText(i);
                var targetNodeIndex = nodeMap[GetNextNode($"{k_outputPortName}{i}")];
                var valueReference = GetValueReference<bool>($"Condition{i}", nodeMap);
                
                branches.Add(new BranchData(choiceText, targetNodeIndex, valueReference));
            }

            return new BranchRuntimeNode(branches);
        }
    }
}