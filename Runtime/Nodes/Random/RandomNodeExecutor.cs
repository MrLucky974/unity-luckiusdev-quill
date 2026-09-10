using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(RandomRuntimeNode))]
    public class RandomNodeExecutor : INodeExecutor<RandomRuntimeNode>
    {
        public void Execute(RandomRuntimeNode node, IDialogueContext ctx)
        {
            var indexes = node.NodeIndexes;
            if (indexes == null || indexes.Count == 0)
            {
                Debug.LogWarning("[RandomNodeExecutor] No nodes were connected to random node.");
                return;
            }
            
            var index = Random.Range(0, indexes.Count);
            var selectedIndex = indexes[index];
            ctx.JumpTo(selectedIndex);
        }
    }
}