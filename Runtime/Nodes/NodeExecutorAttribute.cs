using System;

namespace LuckiusDev.Quill.Nodes
{
    /// <summary>
    /// Declares which runtime node type an INodeExecutor implementation handles.
    /// Must be applied to every concrete executor class so it can be auto-registered.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class NodeExecutorAttribute : Attribute
    {
        public Type NodeType { get; }

        public NodeExecutorAttribute(Type nodeType)
        {
            NodeType = nodeType;
        }
    }
}