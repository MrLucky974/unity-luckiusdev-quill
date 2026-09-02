using System;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class JumpRuntimeNode : FlowRuntimeNode
    {
        public JumpRuntimeNode(int nextNodeIndex) : base(nextNodeIndex) { }
    }
}