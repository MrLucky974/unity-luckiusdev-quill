using LuckiusDev.Quill.Events;
using LuckiusDev.Quill.Nodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LuckiusDev.Quill.Modules
{
    public class BranchSelectionModule : QuillModule<BranchPathRequestedEvent>
    {
        [Header("References")]
        [SerializeField] private Button m_buttonPrefab;
        [SerializeField] private GameObject m_panelGameObject;
        [SerializeField] private RectTransform m_choiceContainer;

        private void Awake()
        {
            m_panelGameObject.SetActive(false);
        }
        
        protected override void OnDialogueEnded()
        {
            m_panelGameObject.SetActive(false);
        }

        protected override void OnEventReceived(BranchPathRequestedEvent e)
        {
            m_panelGameObject.SetActive(true);

            for (int i = m_choiceContainer.childCount - 1; i >= 0; i--)
            {
                var child = m_choiceContainer.GetChild(i);
                Destroy(child.gameObject);
            }

            foreach (var option in e.Options)
            {
                var instance = Instantiate(m_buttonPrefab, m_choiceContainer);
                
                instance.GetComponentInChildren<TMP_Text>().text = option.Text;
                instance.interactable = option.IsInteractable;
                instance.onClick.AddListener(() => Select(option.TargetPortIndex));
            }
        }

        private void Select(int targetIndex)
        {
            Director.JumpTo(targetIndex);
            m_panelGameObject.SetActive(false);
        }
    }
}