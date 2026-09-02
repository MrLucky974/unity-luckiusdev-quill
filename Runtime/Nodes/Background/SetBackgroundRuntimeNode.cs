using System;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public sealed class SetBackgroundRuntimeNode : FlowRuntimeNode
    {
        [SerializeField] private Sprite m_backgroundSprite;
        public Sprite BackgroundSprite => m_backgroundSprite;
        
        public SetBackgroundRuntimeNode(int nextNodeIndex, Sprite backgroundSprite) : base(nextNodeIndex)
        {
            m_backgroundSprite = backgroundSprite;
        }
    }
}