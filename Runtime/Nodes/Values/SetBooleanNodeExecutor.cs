using LuckiusDev.Quill.Nodes;

namespace LuckiusDev.Quill
{
    [NodeExecutor(typeof(SetValueRuntimeNode<bool>))]
    public class SetBooleanNodeExecutor : SetValueNodeExecutor<bool> { }
}