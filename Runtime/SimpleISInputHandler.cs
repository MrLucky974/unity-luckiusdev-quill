using UnityEngine;
using UnityEngine.InputSystem;

namespace LuckiusDev.Quill
{
    public class SimpleISInputHandler : QuillInputHandler
    {
        [SerializeField] private InputActionReference m_actionAsset;
        
        private InputAction m_action;

        private void Awake()
        {
            m_action = m_actionAsset.ToInputAction();
        }

        private void OnEnable()
        {
            m_action.performed += Action_OnPerformed;
            m_action.Enable();
        }

        private void OnDisable()
        {
            m_action.performed -= Action_OnPerformed;
            m_action.Disable();
        }

        private void Action_OnPerformed(InputAction.CallbackContext ctx)
        {
            Invoke();
        }
    }
}