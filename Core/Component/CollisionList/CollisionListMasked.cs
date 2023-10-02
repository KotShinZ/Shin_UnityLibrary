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
    public IGetGravity gravity;

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
        Vector3 gravityVec = gravity == null ? AttributeGetParam.GetAttrParamEnum<Vector3>(direction).Aggregate((x, y) => x + y) : gravity.GetGravityDirection(); //d—Í‚Ì•ûŒü‚ğæ“¾
        return col.isDirection(gravityVec);
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
