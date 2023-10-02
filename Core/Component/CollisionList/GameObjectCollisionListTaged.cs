using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Shin_UnityLibrary;

public class GameObjectCollisionListTaged : GameObjectCollisionList
{
    [Tag]public List<string> tags = new List<string>();

    public override bool isAddList(CollisionData<Transform> col)
    {
        return tags.Any(t => col.component.gameObject.tag == t);
    }
}