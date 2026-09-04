using System;
using System.Collections.Generic;
using System.Linq;
using LuckiusDev.Quill.Nodes.Editor;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace LuckiusDev.Quill.Editor
{
    [Serializable]
    [Graph(k_assetExtension)]
    internal class QuillDirectorGraph : Graph
    {
        internal const string k_assetExtension = "quill";

        [MenuItem("Assets/Create/LuckiusDev/Quill/New Dialogue Graph")]
        private static void CreateAssetFile() {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<QuillDirectorGraph>("Quill Dialogue Graph");
        }

        public StartNode GetStartNode() => GetNodes().OfType<StartNode>().FirstOrDefault();
        public List<MarkerNode> GetMarkerNodes() => GetNodes().OfType<MarkerNode>().ToList();

        internal static BlackboardVariable CreateBlackboardVariable(IVariable variable)
        {
            var genericType = typeof(BlackboardVariable<>).MakeGenericType(variable.DataType);
            var genericObject = Activator.CreateInstance(genericType, variable.ID);

            var bbVar = genericObject as BlackboardVariable;
            if (variable.TryGetDefaultValue(out object val))
                bbVar.SetValue(val);

            return bbVar;
        }

        /// <summary>
        /// Builds a map from graph node to runtime index, traversing from the start node
        /// through its connections first, then appending any marker nodes not yet reached.
        /// This ordering must remain consistent between editor-time option building and
        /// import-time runtime node creation.
        /// </summary>
        internal static NodeMap CreateNodeMap(StartNode startNode, List<MarkerNode> markerNodes)
        {
            var nodeMap = new NodeMap();

            if (startNode == null)
            {
                return nodeMap;
            }

            var visited = new HashSet<BaseNode>();
            var toProcess = new Queue<BaseNode>();
            var index = 0;

            toProcess.Enqueue(startNode);
            visited.Add(startNode);

            // First: traverse fully from the start node through its connections.
            while (toProcess.Count > 0)
            {
                var currentNode = toProcess.Dequeue();
                nodeMap[currentNode] = index;
                index++;

                foreach (var nextNode in GetConnectedNodes(currentNode))
                {
                    if (nextNode != null && visited.Add(nextNode))
                    {
                        toProcess.Enqueue(nextNode);
                    }
                }
            }

            // Then: any marker nodes not reached by the start node traversal
            // (and anything reachable only from them) get appended.
            foreach (var markerNode in markerNodes)
            {
                if (visited.Add(markerNode))
                {
                    toProcess.Enqueue(markerNode);
                }
            }

            while (toProcess.Count > 0)
            {
                var currentNode = toProcess.Dequeue();
                nodeMap[currentNode] = index;
                index++;

                foreach (var nextNode in GetConnectedNodes(currentNode))
                {
                    if (nextNode != null && visited.Add(nextNode))
                    {
                        toProcess.Enqueue(nextNode);
                    }
                }
            }

            return nodeMap;
        }

        private static IEnumerable<BaseNode> GetConnectedNodes(BaseNode node)
        {
            foreach (var port in node.GetOutputPorts())
            {
                var connectedPort = port.FirstConnectedPort;
                if (connectedPort?.GetNode() is BaseNode nextNode)
                {
                    yield return nextNode;
                }
            }

            foreach (var port in node.GetInputPorts())
            {
                var connectedPort = port.FirstConnectedPort;
                if (connectedPort?.GetNode() is BaseNode nextNode)
                {
                    yield return nextNode;
                }
            }
        }

        public override void OnGraphChanged(GraphLogger graphLogger)
        {
            base.OnGraphChanged(graphLogger);

            Dictionary<string, int> options = new();

            var startNode = GetStartNode();
            var markerNodes = GetMarkerNodes();

            if (startNode == null)
            {
                foreach (var jumpNode in GetNodes().OfType<JumpNode>())
                {
                    jumpNode.GetMarkerSelector().SetOptions(options);
                }
                return;
            }

            var nodeMap = CreateNodeMap(startNode, markerNodes);

            // Add start node first with string "Start"
            if (nodeMap.TryGetValue(startNode, out var startIndex))
            {
                options["Start"] = startIndex;
            }

            // Add marker nodes with their names, falling back to "Unnamed MarkerXX"
            for (int i = 0; i < markerNodes.Count; i++)
            {
                var markerNode = markerNodes[i];

                if (!nodeMap.TryGetValue(markerNode, out var markerIndex))
                {
                    continue; // Marker not reachable/mapped; skip.
                }

                var markerName = markerNode.GetName();
                var displayName = string.IsNullOrEmpty(markerName)
                    ? $"Unnamed Marker {i + 1:D2}"
                    : markerName;

                if (!options.TryAdd(displayName, markerIndex))
                {
                    graphLogger.LogWarning(
                        $"Duplicate marker name '{displayName}' detected. Jump targets may be ambiguous.",
                        markerNode);
                }
            }

            foreach (var jumpNode in GetNodes().OfType<JumpNode>())
            {
                var markerSelector = jumpNode.GetMarkerSelector();
                markerSelector.SetOptions(options);
            }

            var variableOptions = new Dictionary<Hash128, string>();
            var variableTypes = new Dictionary<Type, List<Hash128>>();
            var variables = GetVariables();
            foreach (var variable in variables)
            {
                var variableType = variable.DataType;
                var variableName = variable.Name;
                var variableId = variable.ID;
                
                if (variableTypes.ContainsKey(variableType))
                {
                    variableTypes[variableType].Add(variableId);
                }
                else
                {
                    variableTypes.Add(variableType, new List<Hash128>() { variableId });
                }

                variableOptions.Add(variableId, variableName);
            }

            foreach (var valueNode in GetNodes().OfType<SetValueNode>())
            {
                var variableId = valueNode.GetVariableId();
                var variableType = valueNode.GetVariableType();

                variableTypes.TryGetValue(variableType, out List<Hash128> ids);
                variableId.SetOptions(variableOptions
                    .Where(o => variableTypes.TryGetValue(variableType, out List<Hash128> ids) && ids.Contains(o.Key))
                    .ToDictionary(v => v.Key, v => v.Value));
            }
        }
    }
}