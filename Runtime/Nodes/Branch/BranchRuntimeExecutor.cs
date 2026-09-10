using System.Collections.Generic;
using LuckiusDev.Quill.Events;

namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(BranchRuntimeNode))]
    public class BranchRuntimeExecutor : INodeExecutor<BranchRuntimeNode>
    {
        public void Execute(BranchRuntimeNode node, IDialogueContext ctx)
        {
            var options = new List<BranchOption>(node.Branches.Count);

            foreach (var branch in node.Branches)
            {
                var isInteractable = branch.ValueReference?.GetValue(ctx) ?? true;
                options.Add(new BranchOption(branch.Text, branch.TargetPortIndex, isInteractable));
            }

            var e = new BranchPathRequestedEvent(options);
            ctx.Raise(e);
        }
    }
}