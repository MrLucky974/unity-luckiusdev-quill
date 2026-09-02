namespace LuckiusDev.Quill.Nodes
{
    public interface INodeExecutor
    {
        void Execute(RuntimeNode node, DialogueDirector director);
    }
    
    public interface INodeExecutor<in TNode> : INodeExecutor where TNode : RuntimeNode
    {
        void Execute(TNode node, DialogueDirector ctx);
        void INodeExecutor.Execute(RuntimeNode node, DialogueDirector director) => Execute((TNode)node, director);
    }
}