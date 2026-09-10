namespace LuckiusDev.Quill.Nodes
{
    public readonly struct BranchOption
    {
        public readonly string Text;
        public readonly int TargetPortIndex;
        public readonly bool IsInteractable;

        public BranchOption(string text, int targetPortIndex, bool isInteractable)
        {
            Text = text;
            TargetPortIndex = targetPortIndex;
            IsInteractable = isInteractable;
        }
    }
}