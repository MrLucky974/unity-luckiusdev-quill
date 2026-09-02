using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace LuckiusDev.Quill.Editor
{
    public sealed class NodeMap : Dictionary<INode, int>
    {
        public new int this[INode i]
        {
            get
            {
                if (i == null) return -1;
                return TryGetValue(i, out int index) ? index : -1;
            }
            set => base[i] = value;
        }
    }
}