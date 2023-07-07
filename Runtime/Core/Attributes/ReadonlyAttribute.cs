using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadonlyAttribute : PropertyAttribute
{
    public bool isReadonly;

    public ReadonlyAttribute()
    {
        this.isReadonly = true;
    }

    public ReadonlyAttribute(bool isReadonly)
    {
        this.isReadonly = isReadonly;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadonlyAttribute))]
public class ReadonlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        ReadonlyAttribute readonlyAttribute = (ReadonlyAttribute)attribute;

        EditorGUI.BeginDisabledGroup(readonlyAttribute.isReadonly);
        EditorGUI.PropertyField(_position, _property, _label);
        EditorGUI.EndDisabledGroup();
    }
}
#endif
