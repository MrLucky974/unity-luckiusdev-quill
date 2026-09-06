using System;
using UnityEngine;

namespace LuckiusDev.Quill
{
    [Serializable]
    public abstract class BlackboardVariable : ICloneable<BlackboardVariable>
    {
        [SerializeField] private Hash128 m_identifier;
        public Hash128 ID => m_identifier;

        protected BlackboardVariable(Hash128 id)
        {
            m_identifier = id;
        }

        public abstract void SetValue(object value);
        public abstract BlackboardVariable Clone();
    }

    [Serializable]
    public class BlackboardVariable<T> : BlackboardVariable
    {
        [SerializeField] private T m_value;
        public T Value => m_value;

        public BlackboardVariable(Hash128 id, T value = default) : base(id)
        {
            m_value = value;
        }

        public void SetValue(T value)
        {
            m_value = value;
        }

        public override void SetValue(object value)
        {
            if (value is T typedValue) { m_value = typedValue; }
        }

        public override BlackboardVariable Clone()
        {
            var value = CloneValue(m_value);
            return new BlackboardVariable<T>(ID, value);
        }

        private static T CloneValue(T value)
        {
            if (value is ICloneable<T> cloneable)
                return cloneable.Clone();
            
            return value;
        }
    }
}