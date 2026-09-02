using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Flow", "", "Start")]
    internal sealed class StartNode : BaseNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeOutputPort(context);
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var nextNodeIndex = nodeMap[GetNextNode(k_outputPortName)];
            return new StartRuntimeNode(nextNodeIndex);
        }
    }
}