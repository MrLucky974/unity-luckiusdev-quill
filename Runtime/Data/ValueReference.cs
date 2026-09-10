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

        public virtual T GetValue(IDialogueContext ctx) => m_defaultValue;
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

        public override T GetValue(IDialogueContext ctx)
        {
            return ctx.TryGetNode<ValueRuntimeNode<T>>(m_referenceNodeIndex, out var node) ? node.Evaluate(ctx) : m_defaultValue;
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

        public override T GetValue(IDialogueContext ctx)
        {
            return ctx.TryGetVariable<T>(m_variableIdentifier, out var variable) ? variable.Value : m_defaultValue;
        }
    }
}