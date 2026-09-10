using System.Collections.Generic;
using LuckiusDev.Quill.Nodes;

namespace LuckiusDev.Quill.Events
{
    public readonly struct BranchPathRequestedEvent : IQuillEvent
    {
        public readonly IReadOnlyList<BranchOption> Options;
        
        public BranchPathRequestedEvent(IReadOnlyList<BranchOption> options)
        {
            Options = options;
        }
    }
}