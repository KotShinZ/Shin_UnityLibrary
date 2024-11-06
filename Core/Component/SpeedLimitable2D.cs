using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shin_UnityLibrary;

public class SpeedLimitable2D : MonoBehaviour
{
    Rigidbody2D _rigidbody;

    public bool isLimitVelocity = true;

    [HideInInspector] public Vector3 gravityDirection = new Vector3(0, -1, 0);

    public float upingLimitSpeed = 30;
    public float fallingLimitSpeed = 30;
    public float horizontalLimitSpeed = 30;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isLimitVelocity) { LimitVelocity(); }
    }

    /// <summary>
    /// �W�����v���x�𐧌�����
    /// </summary>
    void LimitVelocity()
    {
        bool isLimit = false;

        var prog = Utils.Projection(_rigidbody.linearVelocity, gravityDirection * -1); //�W�����v�����ɑ΂��鐳�ˉe�x�N�g��
        Vector3 vertical = prog;
        if (prog.magnitude > upingLimitSpeed)
        {
            isLimit = true;
            vertical = gravityDirection * upingLimitSpeed; //�㏸�̐���
        }
        else if (prog.magnitude < fallingLimitSpeed * -1)
        {
            isLimit = true;
            vertical = gravityDirection * fallingLimitSpeed; //���~�̐���
        }

        var horizontal = _rigidbody.linearVelocity.VectorTwoToThree() - prog;  //���������̐���
        if (horizontal.magnitude > horizontalLimitSpeed)
        {
            isLimit = true;
            horizontal = horizontal.normalized * horizontalLimitSpeed;
        }

        if (isLimit)
        {
            if ((vertical + horizontal).IsNaN() == false) _rigidbody.linearVelocity = vertical + horizontal;
        }
    }

    /// <summary>
    /// �㏸�����ǂ���
    /// </summary>
    /// <returns></returns>
    public bool isUping()
    {
        return Vector3.Dot(_rigidbody.linearVelocity, gravityDirection * -1) > 0;
    }
}
