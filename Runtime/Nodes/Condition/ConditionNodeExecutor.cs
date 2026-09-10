namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(ConditionRuntimeNode))]
    public class ConditionNodeExecutor : INodeExecutor<ConditionRuntimeNode>
    {
        public void Execute(ConditionRuntimeNode node, IDialogueContext ctx)
        {
            var value = node.ValueReference?.GetValue(ctx) ?? false;
            ctx.JumpTo(value ? node.TrueNodeIndex : node.FalseNodeIndex);
        }
    }
}