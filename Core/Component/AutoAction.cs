using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoAction : MonoBehaviour
{
    public UnityEvent action = new UnityEvent();

    public float time = 3;
    float nowTime = 0;

    public Predicate<AutoAction> predicates = null;

    void Update()
    {
        nowTime += Time.deltaTime;
        if (nowTime > time && time != -1)
        {
            if (predicates == null) { action?.Invoke(); return; }
            if (predicates(this)) action?.Invoke();
        }
    }
}
