using System;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class WaitForInputRuntimeNode : FlowRuntimeNode
    {
        public WaitForInputRuntimeNode(int nextNodeIndex) : base(nextNodeIndex)
        {
        }
    }
}