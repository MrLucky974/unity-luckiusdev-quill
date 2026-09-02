using System.Collections.Generic;
using LuckiusDev.Quill.Nodes;

namespace LuckiusDev.Quill.Events
{
    public readonly struct BranchPathRequestedEvent : IQuillEvent
    {
        public readonly IReadOnlyList<BranchData> Branches;
        
        public BranchPathRequestedEvent(IReadOnlyList<BranchData> branches)
        {
            Branches = branches;
        }
    }
}