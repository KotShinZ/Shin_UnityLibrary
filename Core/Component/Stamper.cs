using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shin_UnityLibrary;

public class Stamper : CollisionListMasked<Stampable>
{
    public override void Hit(CollisionData<Stampable> t)
    {
        base.Hit(t);
        t.component.Stamped();
    }

    public override bool isAddList(CollisionData<Stampable> col)
    {
        if(col.component.canStamp == false) return false;
        return base.isAddList(col);
    }
}
