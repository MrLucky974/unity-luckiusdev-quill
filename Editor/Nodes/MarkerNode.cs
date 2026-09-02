using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Flow", "", "Marker")]
    internal class MarkerNode : BaseNode
    {
        public const string k_markerOptionName = "Marker Name";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<string>(k_markerOptionName);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);
            AddNodeOutputPort(context);
        }

        public string GetName()
        {
            string name = string.Empty;
            GetNodeOptionByName(k_markerOptionName)?.TryGetValue(out name);
            return name;
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var nextNodeIndex = nodeMap[GetNextNode(k_outputPortName)];
            var name = GetName();
            
            return new MarkerRuntimeNode(nextNodeIndex, name);
        }
    }
}