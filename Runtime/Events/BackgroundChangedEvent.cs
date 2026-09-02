using UnityEngine;

namespace LuckiusDev.Quill.Events
{
    public readonly struct BackgroundChangedEvent : IQuillEvent
    {
        public readonly Sprite BackgroundSprite;

        public BackgroundChangedEvent(Sprite backgroundSprite)
        {
            BackgroundSprite = backgroundSprite;
        }
    }
}