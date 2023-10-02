using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class AttributeFloat : Attribute
{
    public float Value { get; private set; }

    public AttributeFloat(float value)
    {
        Value = value;
    }
}