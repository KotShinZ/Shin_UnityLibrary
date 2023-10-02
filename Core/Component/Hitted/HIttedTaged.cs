using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class HIttedTaged<T> : Hitted<T> where T : Component
{
    [Tag, FoldOut("Hit")] public List<string> tags = new List<string>();

    public override bool isHit(CollisionData<T> col)
    {
        //Debug.Log(col.component.gameObject.name + "    " + col.component.gameObject.tag);
        return tags.Any(t => col.component.gameObject.tag == t);
    }
}
