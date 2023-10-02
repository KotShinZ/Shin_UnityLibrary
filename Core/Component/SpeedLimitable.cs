using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shin_UnityLibrary;

public class SpeedLimitable : MonoBehaviour
{
    Rigidbody _rigidbody;

    public bool isLimitVelocity = true;

    [HideInInspector] public Vector3 gravityDirection = new Vector3(0, -1, 0);

    public float upingLimitSpeed = 30;
    public float fallingLimitSpeed = 30;
    public float horizontalLimitSpeed = 30;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isLimitVelocity) { LimitVelocity(); }
    }

    /// <summary>
    /// ジャンプ速度を制限する
    /// </summary>
    void LimitVelocity()
    {
        bool isLimit = false;

        var prog = Utils.Projection(_rigidbody.velocity, gravityDirection * -1); //ジャンプ方向に対する正射影ベクトル
        Vector3 vertical = prog;
        if (prog.magnitude > upingLimitSpeed)
        {
            isLimit = true;
            vertical = gravityDirection * upingLimitSpeed; //上昇の制限
        }
        else if (prog.magnitude < fallingLimitSpeed * -1)
        {
            isLimit = true;
            vertical = gravityDirection * fallingLimitSpeed; //下降の制限
        }

        var horizontal = _rigidbody.velocity - prog;  //水平方向の制限
        if (horizontal.magnitude > horizontalLimitSpeed)
        {
            isLimit = true;
            horizontal = horizontal.normalized * horizontalLimitSpeed;
        }

        if (isLimit)
        {
            if ((vertical + horizontal).IsNaN() == false) _rigidbody.velocity = vertical + horizontal;
        }
    }

    /// <summary>
    /// 上昇中かどうか
    /// </summary>
    /// <returns></returns>
    public bool isUping()
    {
        return Vector3.Dot(_rigidbody.velocity, gravityDirection * -1) > 0;
    }
}
