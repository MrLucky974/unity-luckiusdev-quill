using System;
using System.Collections.Generic;
using System.Linq;
using LuckiusDev.Quill.Editor;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Nodes.Editor
{
    [Serializable]
    internal abstract class BaseNode : Node
    {
        public const string k_inputPortName = "in";
        public const string k_outputPortName = "out";
        
        protected void AddNodeInputPort(IPortDefinitionContext context)
        {
            context.AddInputPort(k_inputPortName)
                .WithDisplayName("Input")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        protected void AddNodeOutputPort(IPortDefinitionContext context)
        {
            context.AddOutputPort(k_outputPortName)
                .WithDisplayName("Output")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .WithCapacity(PortCapacity.Single)
                .Build();
        }

        public abstract RuntimeNode TranslateToRuntimeNode(NodeMap nodeMap);
        
        public BaseNode GetPreviousNode(string inputName)
        {
            IPort inputPort = GetInputPortByName(inputName);
            
            IPort connectedPort = inputPort?.FirstConnectedPort;
            return connectedPort?.GetNode() as BaseNode;
        }
        
        public TNode GetPreviousNode<TNode>(string inputName) where TNode : BaseNode
        {
            IPort inputPort = GetInputPortByName(inputName);
            
            IPort connectedPort = inputPort?.FirstConnectedPort;
            return connectedPort?.GetNode() as TNode;
        }
        
        public BaseNode GetNextNode(string outputName)
        {
            IPort outputPort = GetOutputPortByName(outputName);
            
            IPort connectedPort = outputPort?.FirstConnectedPort;
            return connectedPort?.GetNode() as BaseNode;
        }
        
        public TNode GetNextNode<TNode>(string outputName) where TNode : BaseNode
        {
            IPort outputPort = GetOutputPortByName(outputName);
            
            IPort connectedPort = outputPort?.FirstConnectedPort;
            return connectedPort?.GetNode() as TNode;
        }
    }
}