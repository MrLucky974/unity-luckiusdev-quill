using System;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public sealed class StartRuntimeNode : FlowRuntimeNode
    {
        public StartRuntimeNode(int nextNodeIndex) : base(nextNodeIndex) { }
    }
}