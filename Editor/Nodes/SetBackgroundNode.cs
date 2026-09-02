using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Actions", "", "Set Background")]
    internal sealed class SetBackgroundNode : BaseNode
    {
        public const string k_backgroundOptionName = "Background";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<Sprite>(k_backgroundOptionName);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);
            AddNodeOutputPort(context);
        }

        private Sprite GetBackgroundSprite()
        {
            Sprite backgroundSprite = null;
            GetNodeOptionByName(k_backgroundOptionName)?.TryGetValue(out backgroundSprite);
            return backgroundSprite;
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var nextNodeIndex = nodeMap[GetNextNode(k_outputPortName)];
            Sprite backgroundSprite = GetBackgroundSprite();
            
            return new SetBackgroundRuntimeNode(nextNodeIndex, backgroundSprite);
        }
    }
}