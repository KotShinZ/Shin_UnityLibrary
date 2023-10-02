using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class HIttedLayered<T> : Hitted<T> where T : Component
{
    public LayerMask layerMask = int.MaxValue;

    public override bool isHit(CollisionData<T> col)
    {
        return col.component.gameObject.LayerCheck(layerMask);
    }
}
