using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameObjectCollisionListTaged : GameObjectCollisionList
{
    [Tag]public List<string> tags = new List<string>();

    public override bool isAddList(GameObject col)
    {
        return tags.Any(t => col.tag == t);
    }
}
