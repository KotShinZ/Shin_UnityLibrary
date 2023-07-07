using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionList<T> : MonoBehaviour
{
    public List<T> hits;
    public Action<T> onEnter;
    public Action<T> onExit;

    protected virtual void Awake()
    {
        hits = new List<T>();
    }

    public void OnTriggerEnter(Collider col)
    {
        var p = col.gameObject.GetComponent<T>();
        if (p != null)
        {
            if (!hits.Contains(p) && isAddList(p))
            {
                hits.Add(p);
                onEnter?.Invoke(p);
                Hit(p);
            }
        }
    }
    public void OnTriggerExit(Collider col)
    {
        var p = col.gameObject.GetComponent<T>();
        if (p != null)
        {
            if (hits.Contains(p))
            {
                hits.Remove(p);
                onExit?.Invoke(p);
                Exit(p);
            }
        }
    }

    public virtual bool isAddList(T col)
    {
        return true;
    }

    public virtual void Hit(T t)
    {

    }
    public virtual void Exit(T t)
    {

    }
}

