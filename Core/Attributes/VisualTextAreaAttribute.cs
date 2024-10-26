using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class VisualTextAreaAttribute : PropertyAttribute
{
    public int minLines;
    public int maxLines;
    public int textSize;

    public VisualTextAreaAttribute(int minLines, int maxLines, int textSize)
    {
        this.minLines = minLines;
        this.maxLines = maxLines;
        this.textSize = textSize;
    }
}
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(VisualTextAreaAttribute))]
public class VisualTextAreaDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        VisualTextAreaAttribute textArea = (VisualTextAreaAttribute)attribute;

        // Property label
        EditorGUI.LabelField(position, label);

        // Adjust the height based on the number of lines
        position.y += EditorGUIUtility.singleLineHeight;
        position.height = EditorGUIUtility.singleLineHeight * textArea.maxLines;

        // Save the current GUI style and modify it for text area
        var originalStyle = GUI.skin.textArea;
        var customStyle = new GUIStyle(originalStyle);
        customStyle.fontSize = textArea.textSize;

        // Draw the text area with the custom style
        property.stringValue = EditorGUI.TextArea(position, property.stringValue, customStyle);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        VisualTextAreaAttribute textArea = (VisualTextAreaAttribute)attribute;
        return EditorGUIUtility.singleLineHeight * (textArea.maxLines + 1);
    }
}

#endif