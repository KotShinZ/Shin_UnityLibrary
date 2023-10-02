#if Shin_Overide_VisualCompositor

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualCompositor;

// Any class that inherits Compositor Node will be available to use in the Compositor.
namespace Unity.VisualCompositor
{
    public class BranchSmooth : Branch
    {
        [ExposeField] public float time = 1;

        public override void Render()
        {
            if (branch) { output = True; }
            else { output = False; }
        }
    }
}
#endif