using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Shin_UnityLibrary;

public class CollisionListTaged<T> : CollisionList<T> where T : Component
{
    [Tag] public List<string> tags = new List<string>();

    public override bool isAddList(CollisionData<T> col)
    {
        return tags.Any(t => col.component.gameObject.tag == t);
    }
}
