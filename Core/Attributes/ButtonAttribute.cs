using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Reflection;
using System;

public class NoiseButtonAttribute : PropertyAttribute
{
    public GUIStyle style;

    public NoiseButtonAttribute()
    {
        this.style = null;
    }

    public NoiseButtonAttribute(GUIStyle style)
    {
        this.style = style;
    }
}
/*
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NoiseButtonAttribute))]
public class ButtonAttributeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        NoiseButtonAttribute buttonAttribute = (NoiseButtonAttribute)attribute;
        Noise noise = null;
        if(noise == null)
        {
            string path = property.propertyPath;
            Type type = property.serializedObject.targetObject.GetType();
            noise = (Noise)AssetDatabase.LoadAssetAtPath(path, type);
        }


        if (property.propertyType == SerializedPropertyType.String)
        {
            if (buttonAttribute.style == null)
            {
                if (GUILayout.Button(property.stringValue))
                {
                    noise.SetNoiseMap();
                }
            }
            else
            {
                if (GUILayout.Button(property.stringValue, buttonAttribute.style))
                {
                    noise.SetNoiseMap();
                }
            }
        }
    }
}
#endif
*/