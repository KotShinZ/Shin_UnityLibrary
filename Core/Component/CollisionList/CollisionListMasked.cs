using NaughtyAttributes;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionListMasked<T> : CollisionList<T> where T : Component
{
    [Space(19), Header("Masks")]
    [EnumFlags]public Direction direction;
    [Label("向きの基準となるオブジェクト(Nullだとワールド座標系となる)")] public Transform gravityObject;

    [Space(10)]
    public LayerMask layerMask = int.MaxValue;

    [Space(10)]
    public bool isTag = false;
    [Tag] public List<string> tags = new List<string>();

    bool Layer(CollisionData<T> col)
    {
        return col.component.gameObject.LayerCheck(layerMask);
    }

    bool Direction(CollisionData<T> col) {
        if(gravityObject == null)
        {
            return col.isDirection(direction);
        }
        else
        {
            return col.isDirection(direction, gravityObject : gravityObject.rotation);
        }
    }

    bool Tag(CollisionData<T> col)
    {
        if (isTag) return Utils.CompereTag(col.component.gameObject, tags);
        else return true;
    }

    public override bool isAddList(CollisionData<T> col)
    {
        if (!Tag(col)) return false;

        if (!Direction(col)) return false;

        if(!Layer(col)) return false;

        return base.isAddList(col);  
    }
}
