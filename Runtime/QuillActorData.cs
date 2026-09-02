using UnityEngine;

namespace LuckiusDev.Quill
{
    [CreateAssetMenu(menuName = "LuckiusDev/Quill/New Actor Data")]
    public class QuillActorData : ScriptableObject
    {
        [SerializeField] private string m_name;
        public string Name => m_name;
    }
}