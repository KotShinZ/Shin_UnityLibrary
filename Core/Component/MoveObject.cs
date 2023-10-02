using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveObject : CollisionList<Rigidbody>
{
    [TitleDescription] public string title = "動いている時に、触れているオブジェクトを一緒に動かす";

    Rigidbody _rigidbody;
    Vector3 prePosition;
    Vector3 velocity;
    public float power = 236;
    public float feedBack = 1;

    private Transform preParent;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public void Update()
    {
        /*velocity = rigidbody.velocity;
        velocity = transform.position - prePosition;
        prePosition = transform.position;
        foreach(var hit in hits)
        {
            hit.component.velocity = velocity;
        }*/
    }

    public override void Hit(CollisionData<Rigidbody> t)
    {
        preParent = t.component.gameObject.transform.parent;
        t.component.gameObject.transform.parent = transform;
    }
    public override void Exit(CollisionData<Rigidbody> t)
    {
        t.component.transform.parent = preParent;
    }
}
