using System;
using UnityEngine;

namespace LuckiusDev.Quill
{
    [Serializable]
    public abstract class BlackboardVariable
    {
        [SerializeField] private Hash128 m_identifier;
        public Hash128 ID => m_identifier;

        public BlackboardVariable(Hash128 id)
        {
            m_identifier = id;
        }

        public abstract void SetValue(object value);
    }

    [Serializable]
    public class BlackboardVariable<T> : BlackboardVariable
    {
        [SerializeField] private T m_value;
        public T Value => m_value;

        public BlackboardVariable(Hash128 id, T value) : base(id)
        {
            m_value = value;
        }

        public BlackboardVariable(Hash128 id) : this(id, default(T)) { }

        public void SetValue(T value)
        {
            m_value = value;
        }

        public override void SetValue(object value)
        {
            if (value is T typedValue) { m_value = typedValue; }
        }
    }
}