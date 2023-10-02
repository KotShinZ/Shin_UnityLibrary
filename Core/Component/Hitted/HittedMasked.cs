using NaughtyAttributes;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedMasked<T> : Hitted<T> where T : Component
{
    [FoldOut("Masks", true)][EnumFlags] public Direction direction = (Direction)63;
    [FoldOut("Masks")][Label("•ûŒü‚Ì”»’è‚Ì‘å‚«‚³i¬‚³‚¢‚Ù‚Ç”»’è‚ªŠÃ‚¢j")]public float directionThreshold= 0.7f;
    [FoldOut("Masks")] public IGetGravity gravity;

    [FoldOut("Masks", space = 10)] public LayerMask layerMask = int.MaxValue;

    [FoldOut("Masks", space = 10)] public bool isTag = false;
    [FoldOut("Masks")] public List<string> tags = new List<string>();

    bool Layer(CollisionData<T> col)
    {
        return col.component.gameObject.LayerCheck(layerMask);
    }

    bool Direction(CollisionData<T> col)
    {
        if(gravity == null)
        {
            var vectors = AttributeGetParam.GetAttrParamEnum<Vector3>(direction);
            foreach (var v in vectors)
            {
                var b =col.isDirection(v);
                if(b) return true;
            }
        }
        else { return col.isDirection(gravity.GetGravityDirection()); }
        return false;
    }

    bool Tag(CollisionData<T> col)
    {
        if (isTag) return Utils.CompereTag(col.component.gameObject, tags);
        else return true;
    }

    public override bool isHit(CollisionData<T> col)
    {
        if (!Tag(col)) return false;

        if (!Direction(col)) return false;

        if (!Layer(col)) return false;

        return base.isHit(col);
    }
}
