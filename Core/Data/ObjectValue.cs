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

    public object GetValue(Type type)
    {
        FieldInfo[] fields = typeof(AnyObjectValue).GetFields(BindingFlags.Public | BindingFlags.Instance);

        // �t�B�[���h�������X�g�ɒǉ�
        foreach (FieldInfo field in fields)
        {
            if(field.FieldType == type) return field.GetValue(this);
        }
        return null;
    }
}
