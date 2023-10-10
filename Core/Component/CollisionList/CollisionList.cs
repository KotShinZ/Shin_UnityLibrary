using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using Shin_UnityLibrary;
using UnityEngine.Events;

public abstract class CollisionList<T> : MonoBehaviour where T : UnityEngine.Component
{
    [SerializeField,Readonly]private List<CollisionData<T>> hitsList = new();
    public ReadOnlyCollection<CollisionData<T>> hits => hitsList.AsReadOnly();

    [Space(15)]
    public Action<CollisionData<T>> onEnter;
    public Action<CollisionData<T>> onExit;
    public bool collision = true;
    public bool trigger = true;

    [Space(15)]
    public UnityEvent<CollisionData<T>> OnEnter = new();
    public UnityEvent<CollisionData<T>> OnExit = new();

    private bool dummy= false;
    public int hitInterval;
    [HideInInspector] public CollisionData<T> nowHit = null;
    [HideInInspector] public bool nowHitFrame = false;
    [HideInInspector] public HitType nowHitType;
    public bool isHitting => hits != null;

    public GetColliderEvent getColliderEvent = null;

    protected virtual void Awake()
    {
        hitsList = new List<CollisionData<T>>();
    }

    private void Start()
    {
        if (getColliderEvent != null)
        {
            getColliderEvent.onCollisionEnter += col => AddCollision(col);
            getColliderEvent.onCollisionExit += col => RemoveCollsion(col);
            getColliderEvent.onCollisionStay+= col => CollisionStay(col);
            getColliderEvent.onTriggerStay += col => TriggerStay(col);
            getColliderEvent.onTriggerEnter += col => AddCollision(col);
            getColliderEvent.onTriggerExit += col => RemoveCollsion(col);
        }
    }

    public virtual void Update()
    {
        NowHit();
    }

    /// <summary>
    /// 現在当たったフレームかどうかの処理
    /// </summary>
    void NowHit()
    {
        if (dummy == true)
        {
            dummy = false;
            nowHitFrame = false;
        }
        else
        {
            if (nowHitFrame == true)
            {
                dummy = true;
            }
        }
    }

    public void AddCollision(Collision col)
    {
        if (!collision) return;
        
        T p = col.gameObject.GetComponentWithGetColliderEvent<T>();
        if (p != null)
        {
            if(hitsList.Find(h => h.component.Equals(p)) == null)
            {
                nowHit = new CollisionData<T>(p, col, transform);
                if (isAddList(nowHit)) SetEnterCollision(nowHit);
            }
        }
    }
    public void AddCollision(Collider col)
    {
        if (!trigger) return;

        T p = col.gameObject.GetComponentWithGetColliderEvent<T>();
        if (p != null)
        {
            if (hitsList.Find(h => h.component.Equals(p)) == null)
            {
                nowHit = new CollisionData<T>(p, col, transform);
                if (isAddList(nowHit)) SetEnterCollision(nowHit);
            }
        }
    }
    void RemoveCollsion(Collision col) {
        if (collision)
        {
            var p = col.gameObject.GetComponent<T>();
            RemoveList(p);
        }
    }
    void RemoveCollsion(Collider col) {
        if (trigger)
        {
            var p = col.gameObject.GetComponent<T>();
            RemoveList(p);
        }
    }

    /// <summary>
    /// CollisionDataからその他のデータを設定
    /// </summary>
    /// <param name="data"></param>
    void SetEnterCollision(CollisionData<T> data)
    {
        OnEnter.Invoke(data);

        nowHitType = data.hitType;
        nowHitFrame = true;

        hitsList.Add(data);
        //if (gameObject.name == "Player") Debug.Log("add");
        onEnter?.Invoke(data);
        Hit(data);
    }

    public void OnTriggerEnter(Collider col)
    {
        if(trigger && getColliderEvent == null)
        {
            AddCollision(col);
        }
    }
    public void OnCollisionEnter(UnityEngine.Collision col)
    {
        if (collision && getColliderEvent == null)
        {
            AddCollision(col);
        }
    }
    public virtual void CollisionStay(Collision collision)
    {
        bool b = false;
        hitsList.ForEach(h => b |= (h.component.Equals(collision.gameObject)));
        
        if (collision.gameObject.name == "Hole" && gameObject.name == "IsInAir")
        {
            Debug.Log(gameObject.name);
            Debug.Log(b);
        }
        if (!b) AddCollision(collision);
    }
    public void OnCollisionStay(Collision collision)
    {
        CollisionStay(collision);
    }

    public virtual void TriggerStay(Collider collider)
    {
        bool b = false;
        hitsList.ForEach(h => b |= (h.component.Equals(collider.gameObject)));
        if (collider.gameObject.name == "HoleArea" && gameObject.name == "IsInAir")
        {
            Debug.Log(gameObject.name);
            Debug.Log(b);
        }
        if (!b) AddCollision(collider);
    }
    public void OnTriggerStay(Collider collision)
    {
        TriggerStay(collision);
    }

    public void OnTriggerExit(Collider col)
    {
        if(getColliderEvent == null)  RemoveCollsion(col);
    }
    public void OnCollisionExit(UnityEngine.Collision col)
    {
        if(getColliderEvent == null) RemoveCollsion(col);
    }
    void RemoveList(T p)
    {
        if (p != null)
        {
            var h = hitsList.Find(h => h.component.Equals(p));
            if (h != null)
            {
                OnExit.Invoke(h);
                hitsList.Remove(h);
                //if (gameObject.name == "Player") Debug.Log("Remove");
                onExit?.Invoke(h);
                Exit(h);
            }
        }
    }

    /// <summary>
    /// リストに加えるかどうか
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public virtual bool isAddList(CollisionData<T> col)
    {
        return true;
    }

    public bool isInAir()
    {
        return hits.Count == 0;
    }

    public virtual void Hit(CollisionData<T> t){}
    public virtual void Exit(CollisionData<T> t){}
}