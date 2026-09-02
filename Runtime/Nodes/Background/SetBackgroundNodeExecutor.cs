using LuckiusDev.Quill.Events;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [NodeExecutor(typeof(SetBackgroundRuntimeNode))]
    public class SetBackgroundNodeExecutor : FlowNodeExecutor<SetBackgroundRuntimeNode>
    {
        public override void Execute(SetBackgroundRuntimeNode runtimeNode, DialogueDirector ctx)
        {
            var backgroundSprite = runtimeNode.BackgroundSprite;
            Debug.Log($"Set background to {backgroundSprite}.", backgroundSprite);
            
            var e = new BackgroundChangedEvent(backgroundSprite);
            ctx.Raise(e);
            
            base.Execute(runtimeNode, ctx);
        }
    }
}