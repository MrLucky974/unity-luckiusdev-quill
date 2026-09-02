using System;
using System.Collections.Generic;
using UnityEngine;

namespace LuckiusDev.Quill.Nodes
{
    [Serializable]
    public class BranchRuntimeNode : RuntimeNode
    {
        [SerializeField] private List<BranchData> m_branches;
        public IReadOnlyList<BranchData> Branches => m_branches;

        public BranchRuntimeNode(List<BranchData> branches)
        {
            m_branches = branches;
        }
    }
}