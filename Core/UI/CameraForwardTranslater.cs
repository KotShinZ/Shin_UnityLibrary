using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;

public class CameraForwardTranslater : MonoBehaviour
{
    public float distance = 2;
    public Vector2 offset;
    public enum PosionalType
    {
        Sphere,
        Cylinder
    }
    [Space]
    public PosionalType type;
    public bool lookAtPlayerX = false;
    public bool lookAtOnlyStart = false;
    public bool lookAtInverse = true;
    
    Vector3 forward;
    Vector3 right;
    Vector3 up;

    public void Start()
    {
        SetTransform();
    }

    public void Update()
    {
        if (lookAtOnlyStart == false)
        {
            SetTransform();
        }
    }

    [ContextMenu("SetTransform")]
    public void SetTransform()
    {
        SetForward();

        var center = Camera.main.transform;

        transform.position = center.position + forward * distance + right * offset.x + up * offset.y;
        var pos = lookAtInverse ? transform.position - center.position : center.position - transform.position;
        transform.rotation = Quaternion.LookRotation(pos);

        if (!lookAtPlayerX) transform.eulerAngles.Set(0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    public void SetForward()
    {
        switch (type)
        {
            case PosionalType.Sphere:
                forward = Camera.main.transform.forward;
                right = Camera.main.transform.right;
                up = Camera.main.transform.up;
                break;
            case PosionalType.Cylinder:
                forward = Utils.GetCameraForward();
                right = Utils.GetCameraRight();
                up = Utils.GetCameraUp();
                break;
        }
    }
}
