using System;
using System.Collections.Generic;
using UnityEngine;
using Shin_UnityLibrary;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class Hitted<T> : MonoBehaviour where T : Component
{
    public Action<CollisionData<T>> onEnter;
    public UnityEvent<CollisionData<T>> OnEnter;
    [FoldOut("hit")] public bool collision = true;
    [FoldOut("hit")] public bool trigger = true;
    [MinMaxRange(0,50)]
    public MinMax speedRange = new MinMax(0, 50);
    public GetColliderEvent getColliderEvent = null;

    private bool dummy = false;
    [HideInInspector] public CollisionData<T> nowHit = null;
    [HideInInspector] public bool nowHitFrame = false;
    [HideInInspector] public HitType nowHitType;
    [HideInInspector] public Rigidbody _rigidbody;
    List<Component> components = new();


    public virtual void Update()
    {
        NowHit();
    }

    public virtual void Start()
    {
        if(getColliderEvent != null && collision) getColliderEvent.onCollisionEnter += col=> HitCollision(col);
        if(getColliderEvent != null && trigger) getColliderEvent.onTriggerEnter+= col => HitCollision(col);

        if(_rigidbody == null)
        {
            if (getColliderEvent != null) _rigidbody = getColliderEvent.GetComponentWithGetColliderEvent<Rigidbody>();
            else _rigidbody = GetComponent<Rigidbody>();
        }
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

    /// <summary>
    /// 当たった時の処理
    /// </summary>
    /// <param name="col"></param>
    void HitCollision(Collision col)
    {
        T p = col.gameObject.GetComponentWithGetColliderEvent<T>();
        if (p != null)
        {
            nowHit = new CollisionData<T>(p, col, transform);
            if (isHit(nowHit) && isHitNoOveride(nowHit))
            {
                SetEnterCollision(nowHit);
            }
        }
    }
    void HitCollision(Collider col)
    {
        T p = col.gameObject.GetComponentWithGetColliderEvent<T>();
        if (p != null)
        {
            nowHit = new CollisionData<T>(p, col, transform);
            if (isHit(nowHit) && isHitNoOveride(nowHit))
            {
                SetEnterCollision(nowHit);
            }
        }
    }

    /// <summary>
    /// CollisionDataからその他のデータを設定
    /// </summary>
    /// <param name="data"></param>
    void SetEnterCollision(CollisionData<T> data)
    {
        nowHitType = data.hitType;
        nowHitFrame = true;

        onEnter?.Invoke(data);
        OnEnter.Invoke(data);
        Hit(data);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (trigger)
        {
            if(getColliderEvent == null) HitCollision(col);
        }
    }
    public void OnCollisionEnter(UnityEngine.Collision col)
    {
        if (collision)
        {
            if (getColliderEvent == null) HitCollision(col);
        }
    }

    /// <summary>
    /// 当たっていることになるかどうかを決めれる
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public virtual bool isHit(CollisionData<T> col)
    {
        return true;
    }

    public bool isHitNoOveride(CollisionData<T> col)
    {
        if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
        if(speedRange.IsInRange(_rigidbody.velocity.magnitude) || (speedRange.max == 50 && _rigidbody.velocity.magnitude > 50))
        {
            return true;
        }
        return false;
    }

    public virtual void Hit(CollisionData<T> t) { }

    /// <summary>
    /// ゲームオブジェクトからコンポーネントを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public C GetHitComponent<C>(GameObject obj) where C : Component
    {
        C t;
        if (isInComponentList<C>(out t))
        {
            return t;
        }
        else
        {
            return obj.GetComponent<C>();
        }
    }

    bool isInComponentList<C>(out C t) where C : Component
    {
        foreach (var component in components)
        {
            if (component is C) { t = component as C; return true; }
        }
        t = null;
        return false;
    }

    public async UniTask<CollisionData<T>> WaitHit(Predicate<CollisionData<T>> predicate, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        while (true)
        {
            await UniTask.WaitUntil(() => nowHitFrame, cancellationToken : token);
            if (predicate(nowHit))
            {
                return nowHit;
            }
        }
    }
    public async UniTask<CollisionData<T>> WaitHit(CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        return await WaitHit(_=> true, token);
    }
}