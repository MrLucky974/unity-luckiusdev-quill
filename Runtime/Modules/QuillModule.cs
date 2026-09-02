using System;
using LuckiusDev.Quill.Events;
using UnityEngine;

namespace LuckiusDev.Quill.Modules
{
    public abstract class QuillModule<TEvent> : MonoBehaviour, IDialogueEventBinding<TEvent> where TEvent : IQuillEvent
    {
        [SerializeField] private DialogueDirector m_director;
        protected DialogueDirector Director => m_director;

        public Action<TEvent> OnEvent => OnEventReceived;

        protected abstract void OnEventReceived(TEvent e);

        protected virtual void OnDialogueStarted() {}
        protected virtual void OnDialogueEnded() {}

        protected virtual void OnEnable()
        {
            m_director.Register(this);
            m_director.onDialogueStarted += OnDialogueStarted;
            m_director.onDialogueEnded += OnDialogueEnded;
        }

        protected virtual void OnDisable()
        {
            m_director.Deregister(this);
            m_director.onDialogueStarted -= OnDialogueStarted;
            m_director.onDialogueEnded -= OnDialogueEnded;
        }

        protected virtual void OnValidate()
        {
            Validate();
        }

        protected virtual void Reset()
        {
            Validate();
        }

        private void Validate()
        {
            if (m_director == null)
                m_director = GetComponentInParent<DialogueDirector>();
        }
    }
}