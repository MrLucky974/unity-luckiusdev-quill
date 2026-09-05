using System;
using Unity.GraphToolkit.Editor;

namespace LuckiusDev.Quill.Editor
{
    [Serializable]
    [Node("Nodes/Value", "", "Set Float")]
    internal class SetFloatNode : SetValueNode<float>
    {
        internal override Type GetVariableType() => typeof(float);
    }
}