using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsInAir : CollisionListMasked<Transform>
{
    /// <summary>
    /// �󒆂ɂ��邩�ǂ���
    /// </summary>
    public bool isInAir => hits.Count == 0;

    private void Reset()
    {
        tags = new List<string>() { "Stage", "StageTransparent" };
    }

    public override bool isAddList(CollisionData<Transform> col)
    {
        if (col.component.tag != "StageTransparent" && col.hitType == HitType.Trigger) return false; //�d�͂ɓ�����ƃW�����v�񐔂���
        return base.isAddList(col);
    }
}
