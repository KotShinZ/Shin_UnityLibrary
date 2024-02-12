
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionListMasked<T> : CollisionList<T> where T : Component
{
    [Space(19), Header("Masks")]
    public Direction direction = (Direction)63;
    [Label("向きの基準となるオブジェクト(Nullだとワールド座標系となる)")] public Transform gravityObject;

    [Space(10)]
    public LayerMask layerMask = int.MaxValue;

    [Space(10)]
    public bool isTag = false;
    [Tag] public List<string> tags = new List<string>();

    bool Layer(GameObject col)
    {
        //Debug.Log(col.LayerCheck(layerMask));
        return col.LayerCheck(layerMask);
    }

    bool Direction(CollisionData<T> col) {
       // Debug.Log(col.isDirection(direction));
        if(gravityObject == null)
        {
            return col.isDirection(direction);
        }
        else
        {
            return col.isDirection(direction, gravityObject : gravityObject.rotation);
        }
    }

    bool Tag(GameObject col)
    {
        //Debug.Log(col.gameObject.name);
        //Debug.Log(col.gameObject.tag);
        if (isTag) return Utils.CompereTag(col.gameObject, tags);
        else return true;
    }

    public override bool isAddListObject(GameObject col)
    {
        if (!Tag(col)) return false;
        if (!Layer(col)) return false;
        return base.isAddListObject(col);  
    }

    public override bool isAddList(CollisionData<T> col)
    {
        if (!Direction(col)) return false;
        return base.isAddList(col);
    }
}
