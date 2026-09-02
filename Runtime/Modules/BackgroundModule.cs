using System;
using LuckiusDev.Quill.Events;
using UnityEngine;
using UnityEngine.UI;

namespace LuckiusDev.Quill.Modules
{
    public sealed class BackgroundModule : QuillModule<BackgroundChangedEvent>
    {
        [SerializeField] private Image m_backgroundImage;

        private void Awake()
        {
            m_backgroundImage.gameObject.SetActive(false);
        }

        protected override void OnDialogueStarted()
        {
            m_backgroundImage.gameObject.SetActive(true);
        }

        protected override void OnDialogueEnded()
        {
            m_backgroundImage.gameObject.SetActive(false);
        }

        protected override void OnEventReceived(BackgroundChangedEvent e)
        {
            m_backgroundImage.sprite = e.BackgroundSprite;
        }
    }
}