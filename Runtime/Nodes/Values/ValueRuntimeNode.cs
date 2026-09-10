using LuckiusDev.Quill.Nodes;
using System;

namespace LuckiusDev.Quill
{
    [Serializable]
    public abstract class ValueRuntimeNode<T> : RuntimeNode
    {
        public abstract T Evaluate(IDialogueContext ctx);
    }
}