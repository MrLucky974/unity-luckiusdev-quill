using LuckiusDev.Quill.Nodes;

namespace LuckiusDev.Quill
{
    [NodeExecutor(typeof(SetValueRuntimeNode<float>))]
    public class SetFloatNodeExecutor : SetValueNodeExecutor<float> { }
}