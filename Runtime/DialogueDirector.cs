using System;
using System.Collections.Generic;
using System.Linq;
using LuckiusDev.Quill.Events;
using LuckiusDev.Quill.Nodes;
using UnityEngine;

namespace LuckiusDev.Quill
{
    public sealed class DialogueDirector : MonoBehaviour
    {
        private static event Action<QuillRuntimeGraph> onDialogueRequestedEvent;
        
        public event Action onDialogueStarted;
        public event Action onDialogueEnded;
        public event Action onActionPerformed;

        [SerializeField] private QuillInputHandler m_inputHandler;

        [Header("Debug")]        
        [SerializeField] private bool m_debugMode;
        [SerializeField] private QuillRuntimeGraph m_testGraph;
        
        private IReadOnlyDictionary<Type, INodeExecutor> m_executors;
        private readonly Dictionary<Type, object> m_bindings = new();
        
        private int m_currentNodeIndex;
        private IReadOnlyList<RuntimeNode> m_nodes;
        private IReadOnlyList<BlackboardVariable> m_variables;
        
        private void Awake()
        {
            m_executors = NodeExecutorRegistry.Executors;
        }

        private void Start()
        {
            if (!m_debugMode) return;
            if (m_testGraph != null) Read(m_testGraph);
        }

        private void OnEnable()
        {
            onDialogueRequestedEvent += Event_OnDialogueRequested;
            if (m_inputHandler != null) m_inputHandler.onInputPressed += Event_OnInputPressed;
        }

        private void OnDisable()
        {
            onDialogueRequestedEvent -= Event_OnDialogueRequested;
            if (m_inputHandler != null) m_inputHandler.onInputPressed -= Event_OnInputPressed;
        }

        public static void Read(QuillRuntimeGraph graph)
        {
            onDialogueRequestedEvent?.Invoke(graph);
        }
        
        private void Event_OnInputPressed()
        {
            onActionPerformed?.Invoke();
        }
        
        private void Event_OnDialogueRequested(QuillRuntimeGraph graph)
        {
            m_nodes = graph.Nodes;
            m_variables = graph.Variables
                .Select(variable => variable.Clone())
                .ToList();
            m_currentNodeIndex = 0;
            
            onDialogueStarted?.Invoke();
            ProcessNode();
        }

        private void ProcessNode()
        {
            var node = m_nodes[m_currentNodeIndex];

            if (m_executors.TryGetValue(node.GetType(), out var executor))
            {
                executor.Execute(node, this);
            }
            else
            {
                Debug.LogError($"[DialogueDirector] No executor registered for node type '{node.GetType().Name}'.");
            }
        }

        public void JumpTo(int nodeIndex)
        {
            if (nodeIndex == -1)
            {
                Stop();
                return;
            }
            
            m_currentNodeIndex = nodeIndex;
            ProcessNode();
        }

        public void Stop()
        {
            Debug.Log("Dialogue ended.");
            onDialogueEnded?.Invoke();
        }

        public RuntimeNode GetNode(int index)
        {
            return index == -1 ? null : m_nodes[index];
        }

        public bool TryGetVariable<T>(Hash128 id, out BlackboardVariable<T> outVariable)
        {
            foreach (var variable in m_variables)
            {
                if (variable.ID == id)
                {
                    outVariable = (BlackboardVariable<T>)variable;
                    return true;
                }
            }

            outVariable = new BlackboardVariable<T>(id);
            return false;
        }

        public void Register<TEvent>(IDialogueEventBinding<TEvent> binding) where TEvent : IQuillEvent
        {
            if (binding == null)
            {
                Debug.LogError($"[DialogueDirector] Attempted to register a null binding for event '{typeof(TEvent).Name}'.");
                return;
            }

            var bindings = GetOrCreateBindingList<TEvent>();

            if (bindings.Contains(binding))
            {
                Debug.LogWarning($"[DialogueDirector] Binding already registered for event '{typeof(TEvent).Name}'.");
                return;
            }

            bindings.Add(binding);
        }
        
        public void Deregister<TEvent>(IDialogueEventBinding<TEvent> binding) where TEvent : IQuillEvent
        {
            if (binding == null)
            {
                return;
            }

            if (m_bindings.TryGetValue(typeof(TEvent), out var raw))
            {
                var bindings = (List<IDialogueEventBinding<TEvent>>)raw;
                bindings.Remove(binding);
            }
        }

        public void Raise<TEvent>(TEvent e) where TEvent : IQuillEvent
        {
            if (!m_bindings.TryGetValue(typeof(TEvent), out var raw))
            {
                return;
            }

            var bindings = (List<IDialogueEventBinding<TEvent>>)raw;

            // Snapshot so handlers can safely (de)register during dispatch.
            var snapshot = new IDialogueEventBinding<TEvent>[bindings.Count];
            bindings.CopyTo(snapshot);

            foreach (var binding in snapshot)
            {
                try
                {
                    binding.OnEvent?.Invoke(e);
                }
                catch (Exception ex)
                {
                    Debug.LogError("[DialogueDirector] Exception thrown while handling event " +
                                   $"'{typeof(TEvent).Name}': {ex}");
                }
            }
        }
        
        private List<IDialogueEventBinding<TEvent>> GetOrCreateBindingList<TEvent>() where TEvent : IQuillEvent
        {
            var type = typeof(TEvent);

            if (!m_bindings.TryGetValue(type, out var raw))
            {
                raw = new List<IDialogueEventBinding<TEvent>>();
                m_bindings[type] = raw;
            }

            return (List<IDialogueEventBinding<TEvent>>)raw;
        }
    }
}