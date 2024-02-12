using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeightObject
{
    public GameObject water;
    public float offset = 0;
    public float defautHeight = 0;

    public float height => water == null ? defautHeight + offset : water.transform.position.y + offset;
}