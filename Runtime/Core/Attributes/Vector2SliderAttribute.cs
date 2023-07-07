using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct Vector2Slider
{
    public float x;
    public float y;

    public Vector2 vector => new Vector2(x, y);

    public Vector2Slider(float x, float y)
    {
        this.y = y;
        this.x = x;
    }

    public static Vector2Slider SetVector2Slider(Vector2 vector)
    {
        var m = new Vector2Slider(vector.x,vector.y);
        return m;
    }
}

public class Vector2SliderAttribute : PropertyAttribute
{
    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;

    public Vector2SliderAttribute(float xmin, float xmax,float ymin,float ymax)
    {
        this .xmin = xmin;
        this .xmax = xmax;
        this .ymin = ymin;
        this .ymax = ymax;
    }
    public Vector2SliderAttribute(float min, float max)
    {
        this.xmin = min;
        this.xmax = max;
        this.ymin = min;
        this.ymax = max;
    }
    public Vector2SliderAttribute()
    {
        this.xmin = 0;
        this.xmax = 1;
        this.ymin = 0;
        this.ymax = 1;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(Vector2SliderAttribute))]
public class Vector2SliderDrawer : PropertyDrawer
{
    Vector2SliderAttribute vector2SliderAttribute { get { return (Vector2SliderAttribute)attribute; } }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        SerializedProperty xProp = prop.FindPropertyRelative("x");
        SerializedProperty yProp = prop.FindPropertyRelative("y");

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.LabelField(label);
        xProp.floatValue = EditorGUILayout.Slider(xProp.floatValue, vector2SliderAttribute.xmin, vector2SliderAttribute.xmax);
        yProp.floatValue = EditorGUILayout.Slider(yProp.floatValue, vector2SliderAttribute.xmin, vector2SliderAttribute.xmax);
        EditorGUILayout.EndHorizontal();
    }
}
#endif