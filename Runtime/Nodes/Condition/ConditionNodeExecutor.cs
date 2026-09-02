namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(ConditionRuntimeNode))]
    public class ConditionNodeExecutor : INodeExecutor<ConditionRuntimeNode>
    {
        public void Execute(ConditionRuntimeNode node, DialogueDirector ctx)
        {
            var conditionNode = ctx.GetNode(node.ConditionNodeIndex) as BooleanNode;
            if (conditionNode == null)
            {
                ctx.JumpTo(node.DefaultValue ? node.TrueNodeIndex : node.FalseNodeIndex);
            }
            else
            {
                ctx.JumpTo(conditionNode.Evaluate(ctx) ? node.TrueNodeIndex : node.FalseNodeIndex);
            }
        }
    }
}