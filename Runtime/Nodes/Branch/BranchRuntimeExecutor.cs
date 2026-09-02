using LuckiusDev.Quill.Events;

namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(BranchRuntimeNode))]
    public class BranchRuntimeExecutor : INodeExecutor<BranchRuntimeNode>
    {
        public void Execute(BranchRuntimeNode node, DialogueDirector ctx)
        {
            var e = new BranchPathRequestedEvent(node.Branches);
            ctx.Raise(e);
        }
    }
}