using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FoldOutAttribute : PropertyAttribute
{
    public string name;
    public bool init;

    public FoldOutAttribute(string name, bool init = false)
    {
        this.init = init;
        this.name = name;
    }
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomPropertyDrawer(typeof(FoldOutAttribute))]
public class FoldOutDrawer : PropertyDrawer
{
    static Dictionary<string, bool> foldDict = new Dictionary<string, bool>();
    

    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        FoldOutAttribute FoldOutAttribute = (FoldOutAttribute)attribute;
        if(!foldDict.ContainsKey(FoldOutAttribute.name)) { foldDict.Add(FoldOutAttribute.name, false); }

        if (FoldOutAttribute.init)
        {
            foldDict[FoldOutAttribute.name] = CustomUIFold.Foldout(FoldOutAttribute.name, foldDict[FoldOutAttribute.name]);
        }
        EditorGUI.indentLevel++;
        if(FoldOutAttribute.init)
        {
            if (foldDict[FoldOutAttribute.name]) Field(_position, _property, _label, true);
        }
        else
        {
            if (foldDict[FoldOutAttribute.name]) Field(_position, _property,_label, true);
        }
        EditorGUI.indentLevel--;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        FoldOutAttribute FoldOutAttribute = (FoldOutAttribute)attribute;
        if (!foldDict.ContainsKey(FoldOutAttribute.name)) { foldDict.Add(FoldOutAttribute.name, false); }

        if (FoldOutAttribute.init)
        {
            if (foldDict[FoldOutAttribute.name]) return Height(property, label) -16f;
        }
        if (foldDict[FoldOutAttribute.name]) return Height(property, label) -20f;
        return 0;
    }

    void Field(Rect _position, SerializedProperty _property, GUIContent label,bool layout)
    {
        Wrapper<UnityEvent>.PropertyGUI(_position, _property, label, layout);
    }

    float Height(SerializedProperty _property, GUIContent label)
    {
        return Wrapper<UnityEvent>.GetPropertyHeight(_property, label, base.GetPropertyHeight(_property, label));
    }
}

public static class CustomUIFold
{
    public static bool Foldout(string title, bool display)
    {
        var style = new GUIStyle("ShurikenModuleTitle");
        style.font = new GUIStyle(EditorStyles.label).font;
        style.border = new RectOffset(15, 7, 4, 4);
        style.fixedHeight = 22;
        style.contentOffset = new Vector2(20f, -2f);

        var rect = GUILayoutUtility.GetRect(16f, 22f, style);
        GUI.Box(rect, title, style);

        var e = Event.current;

        var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
        if (e.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
        }

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            display = !display;
            e.Use();
        }

        return display;
    }
}


#endif

