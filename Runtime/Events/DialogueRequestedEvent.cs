using System;

namespace LuckiusDev.Quill.Events
{
    public readonly struct DialogueRequestedEvent : IQuillEvent
    {
        public readonly string Text;
        public readonly QuillActorData ActorData;
        public readonly Action OnComplete;

        public DialogueRequestedEvent(string text, QuillActorData actorData, Action onComplete = null)
        {
            Text = text;
            ActorData = actorData;
            OnComplete = onComplete;
        }
    }
}