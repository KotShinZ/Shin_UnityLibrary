using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DVector3 : IAdditive<DVector3>, IValueable<Vector3>, IFormattable
{
    Vector3 m_value;
    public Vector3 value { get { return m_value; } set { m_value = value; } }

    public DVector3(Vector3 value)
    {
        m_value = value;
    }

    public DVector3 Add(DVector3 value)
    {
        return new DVector3(this.value + value.value);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return m_value.ToString(format, formatProvider);
    }

    public static implicit operator DVector3(Vector3 dint) { return new DVector3(dint); }
}

public struct DVector2 : IAdditive<DVector2>, IValueable<Vector2>, IFormattable
{
    Vector2 m_value;
    public Vector2 value { get { return m_value; } set { m_value = value; } }

    public DVector2(Vector2 value)
    {
        m_value = value;
    }

    public DVector2 Add(DVector2 value)
    {
        return new DVector2(this.value + value.value);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return m_value.ToString(format, formatProvider);
    }

    public static implicit operator DVector2(Vector2 dint) { return new DVector2(dint); }
}