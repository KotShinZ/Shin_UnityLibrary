using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetColliderEvent : MonoBehaviour
{
    [Header("このオブジェクトもGetComponentで取得出来る")]
    public List<GameObject> parents;
    public Action<Collision> onCollisionEnter;
    public Action<Collision> onCollisionStay;
    public Action<Collision> onCollisionExit;
    public Action<Collider> onTriggerEnter;
    public Action<Collider> onTriggerStay;
    public Action<Collider> onTriggerExit;
    [Space]
    public bool isDebug = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(isDebug) Debug.Log("OnCollisionEnter");
        onCollisionEnter?.Invoke(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (isDebug) Debug.Log("OnCollisionStay");
        onCollisionStay?.Invoke(collision);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (isDebug) Debug.Log("OnCollisionExit");
        onCollisionExit?.Invoke(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDebug) Debug.Log("OnTriggerEnter");
        if (isDebug) Debug.Log(other.gameObject.name);
        onTriggerEnter?.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        if (isDebug) Debug.Log("OnTriggerStay");
        onTriggerStay?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (isDebug) Debug.Log("OnTriggerExit");
        onTriggerExit?.Invoke(other);
    }
}
