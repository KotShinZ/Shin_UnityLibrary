using NaughtyAttributes;
using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittedMasked<T> : Hitted<T> where T : Component
{
    [FoldOut("Masks")][EnumFlags] public Direction direction = (Direction)63;
    [FoldOut("Masks", true)]public float directionThreshold= 0.7f; // •ûŒü‚Ì”»’è‚Ì‘å‚«‚³i¬‚³‚¢‚Ù‚Ç”»’è‚ªŠÃ‚¢j
    [FoldOut("Masks")] public IGetGravity gravity;

    [FoldOut("Masks", space = 10)] public LayerMask layerMask = int.MaxValue;

    [FoldOut("Masks", space = 10)] public bool isTag = false;
    [FoldOut("Masks")] public List<string> tags = new List<string>();
    public int n;

    bool Layer(CollisionData<T> col)
    {
        return col.component.gameObject.LayerCheck(layerMask);
    }

    bool Direction(CollisionData<T> col)
    {
        var b = col.isDirection(direction);
        return b;
    }

    bool Tag(CollisionData<T> col)
    {
        if (isTag) return Utils.CompereTag(col.component.gameObject, tags);
        else return true;
    }
    private void Update()
    {
        n = (int)direction;
    }

    public override bool isHit(CollisionData<T> col)
    {
        if (!Tag(col)) return false;

        if (!Direction(col)) return false;

        if (!Layer(col)) return false;

        return base.isHit(col);
    }
}
