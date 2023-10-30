using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class AutoDestroy : MonoBehaviour
{
    public float time = 3;
    float nowTime = 0;

    public Predicate<AutoDestroy> predicates = null;

    void Update()
    {
        nowTime += Time.deltaTime;
        if (nowTime > time && time != -1)
        {
            if(predicates == null) { Destroy(); return; }
            if(predicates(this)) Destroy();
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
