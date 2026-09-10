namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(WaitForInputRuntimeNode))]
    public class WaitForInputNodeExecutor : FlowNodeExecutor<WaitForInputRuntimeNode>
    {
        public override void Execute(WaitForInputRuntimeNode node, IDialogueContext ctx)
        {
            ctx.onActionPerformed += OnActionPerformed;
            
            void OnActionPerformed()
            {
                base.Execute(node, ctx);
                ctx.onActionPerformed -= OnActionPerformed;
            }
        }
    }
}