namespace LuckiusDev.Quill.Nodes
{
    public interface INodeExecutor
    {
        void Execute(RuntimeNode node, IDialogueContext ctx);
    }

    public interface INodeExecutor<in TNode> : INodeExecutor where TNode : RuntimeNode
    {
        void Execute(TNode node, IDialogueContext ctx);
        void INodeExecutor.Execute(RuntimeNode node, IDialogueContext ctx) => Execute((TNode)node, ctx);
    }
}