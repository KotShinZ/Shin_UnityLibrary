using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class AttributeVector : Attribute, IGetParamAttribute<Vector3>
{
    float x;
    float y;
    float z;

    public AttributeVector(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 Get()
    {
        return new Vector3(x, y, z);
    }
}