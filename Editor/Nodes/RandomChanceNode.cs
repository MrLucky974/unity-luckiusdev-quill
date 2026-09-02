using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Value", "", "Random Chance")]
    internal class RandomChanceNode : BaseNode
    {
        public const string k_chanceOptionName = "Chance";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<float>(k_chanceOptionName)
                .WithDefaultValue(0.5F);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort<bool>(k_outputPortName)
                .WithDisplayName("Output")
                .Build();
        }

        private float GetChance()
        {
            var value = 0F;
            GetNodeOptionByName(k_chanceOptionName)?.TryGetValue(out value);
            return value;
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var chance = GetChance();
            return new RandomChanceRuntimeNode(chance);
        }
    }
}