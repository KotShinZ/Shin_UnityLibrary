using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class VisualizeTextureAttribute : PropertyAttribute
{
    public float imageSize;
    public bool isVisualize;

    public VisualizeTextureAttribute(float imageSize)
    {
        this.imageSize = imageSize;
        isVisualize = true;
    }
    public VisualizeTextureAttribute(float imageSize, bool isVisualize)
    {
        this.imageSize = imageSize;
        this.isVisualize = isVisualize;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(VisualizeTextureAttribute))]
public class VisualizeTextureDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        VisualizeTextureAttribute visualizeAttribute = (VisualizeTextureAttribute)attribute;

        Texture2D texture = (Texture2D)property.objectReferenceValue;

        if(texture != null && visualizeAttribute.isVisualize == true)
        {
            Rect rectC = EditorGUILayout.GetControlRect();
            position.x = rectC.width / 2 - visualizeAttribute.imageSize / 2;
            position.y += 10;
            position.height = visualizeAttribute.imageSize;
            position.width = visualizeAttribute.imageSize;

            GUI.DrawTexture(position, texture);
            GUILayout.Space(visualizeAttribute.imageSize);
        }
    }
}
#endif