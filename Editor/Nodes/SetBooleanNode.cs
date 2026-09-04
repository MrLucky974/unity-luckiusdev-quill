using System;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Editor
{
    [Serializable]
    [Node("Nodes/Value", "", "Set Boolean")]
    internal class SetBooleanNode : SetValueNode<bool>
    {
        internal override Type GetVariableType() => typeof(bool);
    }
}