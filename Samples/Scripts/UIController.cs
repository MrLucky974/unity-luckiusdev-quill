using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LuckiusDev.Quill.Samples
{
    [DisallowMultipleComponent]
    public sealed class UIController : MonoBehaviour
    {
        [SerializeField] private QuillRuntimeGraph m_dialogueGraph;
        
        [SerializeField] private Button m_startButton;
        [SerializeField] private Button m_saveButton;
        [SerializeField] private List<Button> m_loadButtons = new();

        private void OnEnable()
        {
            m_startButton.onClick.AddListener(StartButton_OnClicked);
            m_saveButton.onClick.AddListener(SaveButton_OnClicked);
            foreach (var loadButton in m_loadButtons)
            {
                loadButton.onClick.AddListener(LoadButton_OnClicked);
            }
        }
        
        private void OnDisable()
        {
            m_startButton.onClick.RemoveListener(StartButton_OnClicked);
            m_saveButton.onClick.RemoveListener(SaveButton_OnClicked);
            foreach (var loadButton in m_loadButtons)
            {
                loadButton.onClick.RemoveListener(LoadButton_OnClicked);
            }
        }

        private void StartButton_OnClicked()
        {
            DialogueDirector.Read(m_dialogueGraph);
        }
        
        private void SaveButton_OnClicked()
        {
            
        }
        
        private void LoadButton_OnClicked()
        {
            
        }
    }
}