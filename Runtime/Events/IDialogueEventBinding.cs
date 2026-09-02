using System;

namespace LuckiusDev.Quill.Events
{
    public interface IDialogueEventBinding<in TEvent> where TEvent : IQuillEvent
    {
        /// <summary>
        /// Action to be executed when the event occurs.
        /// </summary>
        public Action<TEvent> OnEvent { get; }
    }
}