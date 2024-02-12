using System;
using System.Reflection;
using UnityEngine;
[System.Serializable]
public struct AnyObjectValue
{
    public Int32 Int32Value;
    public Single SingleValue;
    public String StringValue;
    public Vector3 Vector3Value;
    public Vector2 Vector2Value;
    public UnityEngine.Object ObjectValue;

    public AnyObjectValue(int dummy)
    {
        Int32Value = default;
        SingleValue = default;
        StringValue = default;
        Vector3Value = default;
        Vector2Value = default;
        ObjectValue = default;
    }

    /// <summary>
    /// object型を変換
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public AnyObjectValue Parse(object value)
    {
        FieldInfo[] fields = typeof(AnyObjectValue).GetFields(BindingFlags.Public | BindingFlags.Instance);
        var Value = new AnyObjectValue();

        // フィールド名をリストに追加
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == value.GetType()) field.SetValue(Value,value);
        }
        return Value;
    }

    /// <summary>
    /// Type型の値を取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public object GetValue(Type type)
    {
        FieldInfo[] fields = typeof(AnyObjectValue).GetFields(BindingFlags.Public | BindingFlags.Instance);

        // フィールド名をリストに追加
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == type) return field.GetValue(this);
        }
        return null;
    }

    public AnyObjectValue Add(AnyObjectValue value1, AnyObjectValue value2)
    {
        value1.Int32Value += value2.Int32Value;
        value1.SingleValue += value2.SingleValue;
        value1.StringValue += value2.StringValue;
        value1.Vector3Value += value2.Vector3Value;
        value1.Vector2Value += value2.Vector2Value;
        value1.ObjectValue = value2.ObjectValue;

        return value1;
    }

}
