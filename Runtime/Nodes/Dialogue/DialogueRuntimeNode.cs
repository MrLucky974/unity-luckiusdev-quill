using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class DialogueRuntimeNode : FlowRuntimeNode
    {
        [SerializeField, TextArea] private string m_dialogueText;
        public string DialogueText => m_dialogueText;

        [SerializeField] private QuillActorData m_actorData;
        public QuillActorData ActorData => m_actorData;
        
        public DialogueRuntimeNode(int nextNodeIndex, string dialogueText, QuillActorData actorData) : base(nextNodeIndex)
        {
            m_dialogueText = dialogueText;
            m_actorData = actorData;
        }
    }
}