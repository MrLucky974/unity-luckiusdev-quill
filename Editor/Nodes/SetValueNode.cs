using LuckiusDev.Quill.Editor;
using LuckiusDev.Quill.Nodes;
using LuckiusDev.Quill.Nodes.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LuckiusDev.Quill.Editor
{
    [Serializable]
    internal abstract class SetValueNode : BaseNode
    {
        public const string k_variableOptionName = "Variable";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<VariableId>(k_variableOptionName)
                .WithDisplayName("Variable")
                .Delayed();
        }

        internal VariableId GetVariableId()
        {
            if (GetNodeOptionByName(k_variableOptionName).TryGetValue(out VariableId variableId))
            {
                return variableId;
            }

            variableId = new VariableId();
            GetNodeOptionByName(k_variableOptionName).TrySetValue(variableId);
            return variableId;
        }

        internal abstract Type GetVariableType();
    }

    [Serializable]
    internal abstract class SetValueNode<T> : SetValueNode
    {
        public const string k_valuePortName = "Value";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);
            AddNodeOutputPort(context);

            context.AddInputPort<T>(k_valuePortName)
                .WithCapacity(PortCapacity.Single)
                .Build();
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var nextNodeIndex = nodeMap[GetNextNode(k_outputPortName)];
            var variableId = GetVariableId();
            var valueReference = GetValueReference<T>(k_valuePortName, nodeMap);

            return new SetValueRuntimeNode<T>(variableId.ID, valueReference, nextNodeIndex);
        }
    }

    [Serializable]
    public sealed class VariableId
    {
        [SerializeField] private int m_selectedOption;
        [SerializeField] private List<string> m_markerNames = new();
        [SerializeField] private List<Hash128> m_markerIndices = new();

        public Hash128 ID => m_markerIndices.Count > 0 ? m_markerIndices[m_selectedOption] : default;

        public static implicit operator Hash128(VariableId variableID)
        {
            return variableID.ID;
        }

        public void SetOptions(Dictionary<Hash128, string> variables)
        {
            m_markerNames = variables.Values.ToList();
            m_markerIndices = variables.Keys.ToList();
            m_selectedOption = Mathf.Clamp(m_selectedOption, 0, variables.Count);
        }
    }

    [CustomPropertyDrawer(typeof(VariableId))]
    public sealed class VariableIdPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var markerNamesProperty = property.FindPropertyRelative("m_markerNames");
            var selectedOptionProperty = property.FindPropertyRelative("m_selectedOption");

            var dropdown = new DropdownField(property.displayName);

            void RefreshChoices()
            {
                var choices = new List<string>();
                for (var i = 0; i < markerNamesProperty.arraySize; i++)
                {
                    choices.Add(markerNamesProperty.GetArrayElementAtIndex(i).stringValue);
                }

                dropdown.choices = choices;

                var clampedIndex = choices.Count > 0
                    ? Mathf.Clamp(selectedOptionProperty.intValue, 0, choices.Count - 1)
                    : -1;

                // SetValueWithoutNotify avoids re-triggering the change callback below.
                if (clampedIndex >= 0)
                {
                    dropdown.SetValueWithoutNotify(choices[clampedIndex]);
                }
                else
                {
                    dropdown.SetValueWithoutNotify(null);
                }

                if (clampedIndex != selectedOptionProperty.intValue)
                {
                    selectedOptionProperty.intValue = Mathf.Max(clampedIndex, 0);
                    selectedOptionProperty.serializedObject.ApplyModifiedProperties();
                }
            }

            RefreshChoices();

            dropdown.RegisterValueChangedCallback(evt =>
            {
                var newIndex = dropdown.choices.IndexOf(evt.newValue);
                if (newIndex < 0)
                {
                    return;
                }

                selectedOptionProperty.intValue = newIndex;
                selectedOptionProperty.serializedObject.ApplyModifiedProperties();
            });

            // Keep the dropdown in sync if m_markerNames changes externally
            // (e.g., SetOptions called from a custom inspector, undo/redo, etc.)
            dropdown.TrackPropertyValue(markerNamesProperty, _ => RefreshChoices());
            dropdown.TrackPropertyValue(selectedOptionProperty, _ =>
            {
                if (selectedOptionProperty.intValue >= 0 &&
                    selectedOptionProperty.intValue < dropdown.choices.Count)
                {
                    dropdown.SetValueWithoutNotify(dropdown.choices[selectedOptionProperty.intValue]);
                }
            });

            return dropdown;
        }
    }
}