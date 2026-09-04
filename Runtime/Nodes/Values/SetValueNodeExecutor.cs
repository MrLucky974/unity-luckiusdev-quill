using LuckiusDev.Quill.Nodes;

namespace LuckiusDev.Quill
{
    public abstract class SetValueNodeExecutor<T> : FlowNodeExecutor<SetValueRuntimeNode<T>>
    {
        public override void Execute(SetValueRuntimeNode<T> node, DialogueDirector ctx)
        {
            node.SetValue(ctx);
            base.Execute(node, ctx);
        }
    }
}