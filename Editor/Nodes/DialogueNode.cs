using System;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    [Node("Nodes/Actions", "", "Dialogue")]
    internal class DialogueNode : BaseNode
    {
        public const string k_textOptionName = "Text";
        public const string k_actorOptionName = "Actor";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption<QuillActorData>(k_actorOptionName);
            context.AddOption<string>(k_textOptionName).AsTextArea();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddNodeInputPort(context);
            AddNodeOutputPort(context);
        }

        private string GetText()
        {
            string text = string.Empty;
            GetNodeOptionByName(k_textOptionName)?.TryGetValue(out text);
            return text;
        }
        
        private QuillActorData GetActor()
        {
            QuillActorData actor = null;
            GetNodeOptionByName(k_actorOptionName)?.TryGetValue(out actor);
            return actor;
        }
        
        public override RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap)
        {
            var nextNodeIndex = nodeMap[GetNextNode(k_outputPortName)];
            string text = GetText();
            QuillActorData actor = GetActor();
            
            return new DialogueRuntimeNode(nextNodeIndex, text, actor);
        }
    }
}