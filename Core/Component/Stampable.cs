using Cysharp.Threading.Tasks;
using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Stampable : MonoBehaviour
{
    [FoldOut("Collapse")] public GameObject collapseObject;
    [FoldOut("Collapse")] public bool isCollapse = true;
    [FoldOut("Collapse")] public float collapsePower = 2;
    [FoldOut("Collapse")] public float collapseSpead = 2;
    [Space(10)]
    public UnityEvent onStamped = new();
    public UnityEvent onCollapsed = new();

    public bool canStamp = true;
    public bool stampedFrame = false;
    

    public virtual void Stamped()
    {
        onStamped?.Invoke();
        Collapse().Forget();
        SetFrame().Forget();
    }

    public async UniTask SetFrame()
    {
        stampedFrame = true;
        await UniTask.DelayFrame(1);
        stampedFrame = false;
    }

    public virtual async UniTask Collapse()
    {
        //‘Ì‚ð‰Ÿ‚µ‚Â‚Ô‚·
        if(isCollapse) await Utils.To(
            f => transform.localScale = new Vector3(transform.localScale.x, f, transform.localScale.z), 
            transform.localScale.y ,
            transform.localScale.y / collapsePower,
            (float)(1 / (collapseSpead + 0.0001)));;
        onCollapsed?.Invoke();
    }
}
