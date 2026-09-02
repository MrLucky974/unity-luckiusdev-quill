using System;
using UnityEngine;

namespace LuckiusDev.Quill
{
    [DisallowMultipleComponent]
    public abstract class QuillInputHandler : MonoBehaviour
    {
        public event Action onInputPressed;

        protected void Invoke()
        {
            onInputPressed?.Invoke();
        }
    }
}