using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsInAir : CollisionListMasked<Transform>
{
    /// <summary>
    /// ‹ó’†‚É‚¢‚é‚©‚Ç‚¤‚©
    /// </summary>
    public bool isInAir => hits.Count == 0;

    private void Reset()
    {
        tags = new List<string>() { "Stage", "StageTransparent" };
    }

    public override bool isAddList(CollisionData<Transform> col)
    {
        if (col.component.tag != "StageTransparent" && col.hitType == HitType.Trigger) return false; //d—Í‚É“–‚½‚é‚ÆƒWƒƒƒ“ƒv‰ñ”‚ª‰ñ•œ
        return base.isAddList(col);
    }
}
