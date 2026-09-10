using System;
using LuckiusDev.Quill.Events;
using LuckiusDev.Quill.Nodes;
using UnityEngine;

namespace LuckiusDev.Quill
{
    public interface IDialogueContext
    {
        event Action onActionPerformed;
        
        bool TryGetNode<T>(int index, out T node) where T : RuntimeNode;
        bool TryGetVariable<T>(Hash128 id, out BlackboardVariable<T> outVariable);

        void JumpTo(int nodeIndex);
        
        void Raise<TEvent>(TEvent e) where TEvent : IQuillEvent;
    }
}