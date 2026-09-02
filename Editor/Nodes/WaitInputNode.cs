using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Actions", "", "Wait For Input")]
    internal class WaitInputNode : BaseNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);
            AddNodeOutputPort(context);
        }
        
        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var nextNodeIndex = nodeMap[GetNextNode(k_outputPortName)];
            return new WaitForInputRuntimeNode(nextNodeIndex);
        }
    }
}