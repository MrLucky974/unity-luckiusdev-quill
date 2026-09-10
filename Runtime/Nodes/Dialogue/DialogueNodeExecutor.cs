using LuckiusDev.Quill.Events;

namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(DialogueRuntimeNode))]
    public class DialogueNodeExecutor : FlowNodeExecutor<DialogueRuntimeNode>
    {
        public override void Execute(DialogueRuntimeNode node, IDialogueContext ctx)
        {
            var e = new DialogueRequestedEvent(node.DialogueText, node.ActorData, () => base.Execute(node, ctx));
            ctx.Raise(e);
        }
    }
}