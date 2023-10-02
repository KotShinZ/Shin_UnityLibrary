
#if Shin_Overide_VisualCompositor
using System.Collections.Generic;


// Any class that inherits Compositor Node will be available to use in the Compositor.
namespace Unity.VisualCompositor
{
    public class InputNode<T> : CompositorNode
    {
        [OutputPort] public T output;

        [ExposeField] public string key;
        [ExposeField, Unity.Collections.ReadOnly] public T value;

        static Dictionary<string, T> inputs = new();

        // This method implements the functionality of the node. You can read from input port fields
        // and write to output port fields, the Compositor takes care of moving those values around
        // the graph.
        public override void Render()
        {
            T t;
            var b = inputs.TryGetValue(key, out t);
            if (b != false) { value = t; output = t; }
            else output = default(T);
        }
        public static void SetValue(string key, T t)
        {
            inputs[key] = t;
        }
    }
}


#endif