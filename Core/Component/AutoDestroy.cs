using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Events;

public class AutoDestroy : MonoBehaviour
{
    public float time = 3;
    public bool isDisable = false;
    [Readonly] public float nowTime = 0;

    public Predicate<AutoDestroy> predicates = null;
    public UnityEvent PreDestroy = new UnityEvent();

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
        PreDestroy?.Invoke();
        if(isDisable == false) Destroy(gameObject);
        else gameObject.SetActive(false);
    }
}
