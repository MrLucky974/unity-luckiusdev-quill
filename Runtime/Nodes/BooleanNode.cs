using System;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public abstract class BooleanNode : RuntimeNode
    {
        public abstract bool Evaluate(DialogueDirector ctx);
    }
}