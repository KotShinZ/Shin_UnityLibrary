using System;

public struct DString : IAdditive<DString>, IValueable<string>, IFormattable
{
    string m_value;
    string IValueable<string>.value { get { return m_value; }set { m_value= value;} }

    public DString(string value)
    {
        m_value = value;
    }

    public DString Add(DString value)
    {
        return new DString(m_value + value.m_value);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return m_value;
    }

    public static implicit operator DString(string dint) { return new DString(dint); }
}