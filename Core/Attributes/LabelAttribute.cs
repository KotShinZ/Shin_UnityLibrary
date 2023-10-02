using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LabelAttribute : PropertyAttribute
{
    public string label;
    public LabelAttribute(string s)
    {
        this.label = s;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LabelAttribute))]
public class LabelDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        LabelAttribute labelAttribute = (LabelAttribute)attribute;

        EditorGUI.PropertyField(_position, _property, new GUIContent(labelAttribute.label));
    }
}
#endif