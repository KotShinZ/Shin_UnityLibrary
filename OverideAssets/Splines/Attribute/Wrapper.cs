using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable]
public class UnityEventWrapper : Wrapper<UnityEvent>
{
    public UnityEventWrapper() { }
    public UnityEventWrapper(UnityEvent t) : base(t)
    {
    }
}

[System.Serializable]
public class Wrapper<T> where T : class, new()
{
    private T wrap_NoExistOtherProperty = new T();
    public T property
    {
        get { return wrap_NoExistOtherProperty; }
        set { wrap_NoExistOtherProperty = value;}
    }
    public virtual string name => "wrap";

    public Wrapper()
    {
    }
    public Wrapper(T t)
    {
        wrap_NoExistOtherProperty = t;
    }

#if UNITY_EDITOR
    public static void PropertyGUI(Rect _position, SerializedProperty _property, GUIContent label , bool layout = false)
    {
        SerializedProperty wrapProp = _property.FindPropertyRelative("wrap_NoExistOtherProperty");
        if (wrapProp != null)
        {
            //_position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //_position.height = EditorGUI.GetPropertyHeight(wrapProp);
            EditorGUI.PropertyField(_position, wrapProp);
        }
        else { 
            if(layout) EditorGUILayout.PropertyField(_property, true);
            else EditorGUI.PropertyField(_position, _property, true);
        }
    }

    public static float GetPropertyHeight(SerializedProperty property, GUIContent label , float defaultHeight)
    {
        SerializedProperty wrapProp = property.FindPropertyRelative("wrap_NoExistOtherProperty");
        if (wrapProp != null)
        {
            return EditorGUI.GetPropertyHeight(wrapProp);
        }
            
        return defaultHeight;
    }
#endif
}
