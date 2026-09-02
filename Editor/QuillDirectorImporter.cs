using System.Collections.Generic;
using System.Linq;
using LuckiusDev.Quill.Nodes;
using LuckiusDev.Quill.Nodes.Editor;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace LuckiusDev.Quill.Editor
{
    [ScriptedImporter(1, QuillDirectorGraph.k_assetExtension)]
    internal sealed class QuillDirectorImporter : ScriptedImporter
    {
        private const int k_invalidNodeIndex = -1;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var graph = GraphDatabase.LoadGraphForImporter<QuillDirectorGraph>(ctx.assetPath);

            if (graph == null) {
                Debug.LogError($"Failed to load Quill Director graph asset: {ctx.assetPath}");
                return;
            }

            var startNode = graph.GetStartNode();

            if (startNode == null) {
                Debug.LogWarning($"Quill Director graph '{ctx.assetPath}' has no Start node. Skipping runtime asset build.");
                return;
            }

            var markerNodes = graph.GetMarkerNodes();

            List<RuntimeNode> nodes = CreateRuntimeNodes(startNode, markerNodes);

            var runtimeAsset = QuillRuntimeGraph.Create(nodes);
            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        private static List<RuntimeNode> CreateRuntimeNodes(StartNode startNode, List<MarkerNode> markerNodes)
        {
            var nodeMap = QuillDirectorGraph.CreateNodeMap(startNode, markerNodes);
            var runtimeNodes = new List<RuntimeNode>(new RuntimeNode[nodeMap.Count]);

            foreach (var (node, index) in nodeMap)
            {
                if (node is BaseNode baseNode)
                {
                    runtimeNodes[index] = baseNode.TranslateToRuntimeNode(nodeMap);
                }
            }

            return runtimeNodes;
        }
    }
}