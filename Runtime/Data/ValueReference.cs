using System;
using UnityEngine;

namespace LuckiusDev.Quill
{
    [Serializable]
    public abstract class ValueReference { }

    [Serializable]
    public class ValueReference<T> : ValueReference
    {
        [SerializeField] protected T m_defaultValue;
        public T DefaultValue => m_defaultValue;

        public ValueReference(T defaultValue)
        {
            m_defaultValue = defaultValue;
        }

        public ValueReference() : this(default) { }

        public virtual T GetValue(DialogueDirector ctx) => m_defaultValue;
    }

    [Serializable]
    public class NodeValueReference<T> : ValueReference<T>
    {
        [SerializeField] protected int m_referenceNodeIndex;
        public int ReferenceNodeIndex => m_referenceNodeIndex;

        public NodeValueReference(int referenceNodeIndex = -1, T defaultValue = default) : base(defaultValue)
        {
            m_referenceNodeIndex = referenceNodeIndex;
        }

        public override T GetValue(DialogueDirector ctx)
        {
            var conditionNode = ctx.GetNode(m_referenceNodeIndex) as ValueRuntimeNode<T>;
            return conditionNode == null ? m_defaultValue : conditionNode.Evaluate(ctx);
        }
    }

    [Serializable]
    public class VariableValueReference<T> : ValueReference<T>
    {
        [SerializeField] protected Hash128 m_variableIdentifier;
        public Hash128 VariableID => m_variableIdentifier;

        public VariableValueReference(Hash128 variableIdentifier, T defaultValue = default) : base(defaultValue)
        {
            m_variableIdentifier = variableIdentifier;
        }

        public override T GetValue(DialogueDirector ctx)
        {
            if (ctx.TryGetVariable<T>(m_variableIdentifier, out var variable))
            {
                return variable.Value;
            }
            {
                return m_defaultValue;
            }
        }
    }
}