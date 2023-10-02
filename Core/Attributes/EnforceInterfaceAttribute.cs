using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnforceInterfaceAttribute : PropertyAttribute
{
    public System.Type type;

    public EnforceInterfaceAttribute(System.Type enforcedType)
    {
        type = enforcedType;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnforceInterfaceAttribute))]
public class PrettyListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnforceInterfaceAttribute propAttribute = attribute as EnforceInterfaceAttribute;
        EditorGUI.BeginProperty(position, label, property);

        MonoBehaviour obj = EditorGUI.ObjectField(position, property.objectReferenceValue, typeof(MonoBehaviour), true) as MonoBehaviour;
        if (obj != null && propAttribute.type.IsAssignableFrom(obj.GetType()) && !EditorGUI.showMixedValue)
        {
            property.objectReferenceValue = obj as MonoBehaviour;
        }
        EditorGUI.EndProperty();
    }
}
#endif 
