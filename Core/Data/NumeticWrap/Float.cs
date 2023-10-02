using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DFloat : IAdditive<DFloat>, IValueable<float>, IFormattable
{
    float m_value;
    public float value { get { return m_value; } set { m_value = value; } }

    public DFloat(float value)
    {
        m_value = value;
    }

    public DFloat Add(DFloat value)
    {
        return new DFloat(this.value + value.value);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return m_value.ToString(format, formatProvider);
    }

    public static implicit operator DFloat(float dint) { return new DFloat(dint); }
}