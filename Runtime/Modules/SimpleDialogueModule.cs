using System;
using System.Collections;
using LuckiusDev.Quill.Events;
using TMPro;
using UnityEngine;

namespace LuckiusDev.Quill.Modules
{
    public class SimpleDialogueModule : QuillModule<DialogueRequestedEvent>
    {
        [Header("References")]
        [SerializeField] private TMP_Text m_nameLabel;
        [SerializeField] private TMP_Text m_textLabel;
        
        [Space]
        
        [SerializeField] private GameObject m_dialogueBoxObject;

        [Header("Settings")]
        [SerializeField] private float m_characterDelay = 0.2F;
        [SerializeField] private float m_spaceDelay = 0.2F;
        [SerializeField] private float m_endDelay = 0.2F;

        private Coroutine m_coroutine;

        private void Awake()
        {
            m_dialogueBoxObject.SetActive(false);
        }

        protected override void OnDialogueEnded()
        {
            m_dialogueBoxObject.SetActive(false);
        }

        private IEnumerator ReadText(string text, Action onComplete = null)
        {
            var index = 0;
            m_textLabel.text = "";

            var skipDialogue = false;
            Director.onActionPerformed += Director_OnActionPerformed;

            while (index < text.Length && !skipDialogue)
            {
                var character = text[index];
                yield return new WaitForSecondsRealtime(char.IsWhiteSpace(character) ? m_spaceDelay : m_characterDelay);
                m_textLabel.text += character;
                index++;
            }

            if (!skipDialogue)
            {
                yield return new WaitForSecondsRealtime(m_endDelay);
            }

            m_textLabel.text = text;
            m_coroutine = null;
            onComplete?.Invoke();

            void Director_OnActionPerformed()
            {
                skipDialogue = true;
                Director.onActionPerformed -= Director_OnActionPerformed;
            }
        }
        
        protected override void OnEventReceived(DialogueRequestedEvent e)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
            
            m_dialogueBoxObject.SetActive(true);
            
            m_coroutine = StartCoroutine(ReadText(e.Text, e.OnComplete));
            
            var actorData = e.ActorData;
            m_nameLabel.text = actorData.Name;
        }
    }
}