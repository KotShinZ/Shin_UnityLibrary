#if Shin_Overide_VisualCompositor

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualCompositor;

// Any class that inherits Compositor Node will be available to use in the Compositor.
namespace Unity.VisualCompositor
{
    public class Branch : CompositorNode
    {
        // Fields that have an InputPort attribute will become input ports in the Compositor Editor. 
        [InputPort] public RenderTexture True;
        [InputPort] public RenderTexture False;
        [InputPort] public bool branch;

        // Fields that have an OutputPort attribute will become output ports in the Compositor Editor.
        [OutputPort] public RenderTexture output;

        public override void Render()
        {
            if (branch) { output = True; }
            else { output = False; }
        }
    }
}

#endif