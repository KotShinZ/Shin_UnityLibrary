
using System;

public struct DInt : IAdditive<DInt>, IValueable<int>, IFormattable
{
    int m_value;
    public int value { get { return m_value; } set { m_value = value; } }

    public DInt(int value)
    {
        m_value = value; 
    }

    public DInt Add(DInt value)
    {
        return new DInt(m_value + value.value);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return m_value.ToString(format, formatProvider);
    }

    public static implicit operator DInt(int dint) {return new DInt(dint); }
}
