using LuckiusDev.Quill.Nodes;
using System;
using System.Collections;
using UnityEngine;

namespace LuckiusDev.Quill
{
    [Serializable]
    public abstract class ValueRuntimeNode<T> : RuntimeNode
    {
        public abstract T Evaluate(DialogueDirector ctx);
    }
}