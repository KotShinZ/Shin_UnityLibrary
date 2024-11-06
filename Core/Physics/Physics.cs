using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shin_Physics : MonoBehaviour
{
    public static void AddForceLinear(Rigidbody rigidbody, Vector3 force, float feedBack = 1, bool isMass = false)
    {
        float power = force.magnitude;
        Vector3 direction = force.normalized;
        Vector3 targetVelocity = (isMass ? power / rigidbody.mass : power) * direction;
        Vector3 velocityDirection = Vector3.Dot(rigidbody.linearVelocity, direction.normalized) * direction.normalized; //�Ď��Ώۃx�N�g��

        rigidbody.AddForce((targetVelocity - velocityDirection) * power * feedBack, ForceMode.Acceleration);
    }
}
