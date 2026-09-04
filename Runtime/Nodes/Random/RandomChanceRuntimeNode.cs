using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class RandomChanceRuntimeNode : BooleanRuntimeNode
    {
        [SerializeField, Range(0F, 1F)] private float m_chance;
        
        public RandomChanceRuntimeNode(float chance)
        {
            m_chance = Mathf.Clamp01(chance);
        }

        public override bool Evaluate(DialogueDirector ctx)
        {
            return Random.value <= m_chance;
        }
    }
}