using System;
using System.Collections.Generic;
using System.Linq;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Flow", "", "Jump")]
    internal class JumpNode : BaseNode
    {
        public const string k_destinationOptionName = "Destination";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<MarkerSelector>(k_destinationOptionName)
                .WithDisplayName("Jump To");
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);
        }

        internal MarkerSelector GetMarkerSelector()
        {
            if (GetNodeOptionByName(k_destinationOptionName).TryGetValue(out MarkerSelector markerSelector))
            {
                return markerSelector;
            }

            markerSelector = new MarkerSelector();
            GetNodeOptionByName(k_destinationOptionName).TrySetValue(markerSelector);
            return markerSelector;
        }

        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var markerSelector = GetMarkerSelector();
            return new JumpRuntimeNode(markerSelector.SelectedMarkerIndex);
        }

        [Serializable]
        public sealed class MarkerSelector
        {
            [SerializeField] private int m_selectedOption;
            [SerializeField] private List<string> m_markerNames = new();
            [SerializeField] private List<int> m_markerIndices = new();
            
            public int SelectedMarkerIndex => m_markerIndices.Count > 0 ? m_markerIndices[m_selectedOption] : -1;

            public void SetOptions(Dictionary<string, int> markers)
            {
                m_markerNames = markers.Keys.ToList();
                m_markerIndices = markers.Values.ToList();
                m_selectedOption = Mathf.Clamp(m_selectedOption, 0, markers.Count);
            }
            
            public static implicit operator string(MarkerSelector markerSelector)
            {
                var markerNames = markerSelector.m_markerNames;
                if (markerNames == null || markerNames.Count == 0) return string.Empty;
                var index = Mathf.Clamp(markerSelector.m_selectedOption, 0, markerNames.Count - 1);
                return markerNames[index];
            }
        }

        
        [CustomPropertyDrawer(typeof(MarkerSelector))]
        private class MarkerSelectorPropertyDrawer : PropertyDrawer
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
}