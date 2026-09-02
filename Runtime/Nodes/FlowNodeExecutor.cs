namespace LuckiusDev.Quill.Nodes
{
    public abstract class FlowNodeExecutor<TNode> : INodeExecutor<TNode> where TNode : FlowRuntimeNode
    {
        public virtual void Execute(TNode node, DialogueDirector ctx)
        {
            ctx.JumpTo(node.NextNodeIndex);
        }
    }
}