using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(StartRuntimeNode))]
    public sealed class StartNodeExecutor : FlowNodeExecutor<StartRuntimeNode>
    {
        public override void Execute(StartRuntimeNode runtimeNode, IDialogueContext ctx)
        {
            Debug.Log("Starting dialogue.");
            base.Execute(runtimeNode, ctx);
        }
    }
}