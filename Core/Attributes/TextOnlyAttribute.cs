using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// テキストだけ表示する。Boldにすることも出来る。
/// </summary>
public class TextDescriptionAttribute : PropertyAttribute
{

    //宣言時に渡された文字列
    public string Text;
    public bool bold;

    /// <summary>
    /// コンストラクタ(引数で文字列を渡す)
    /// </summary>
    public TextDescriptionAttribute(string text, bool bold = false)
    {
        Text = text;
        this.bold = bold;
    }
}

public class TitleDescriptionAttribute : PropertyAttribute
{
    //宣言時に渡された文字列
    public string Text;
    public bool bold = false;

    /// <summary>
    /// コンストラクタ(引数で文字列を渡す)
    /// </summary>
    public TitleDescriptionAttribute() { }
    public TitleDescriptionAttribute(string text)
    {
        Text = text;
    }
}

#if UNITY_EDITOR
//CustomPropertyDrawerでTestAttributeを指定(TestAttributeのInspector上の見た目を変更するため)
[CustomPropertyDrawer(typeof(TextDescriptionAttribute))]
public class TextDescriptionDrawer : PropertyDrawer
{
    

    //Inspector上の見た目を設定
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        

        //TestAttributeを取得
        TextDescriptionAttribute testAttribute = (TextDescriptionAttribute)attribute;

        //TestAttributeのTextに設定されている文字列をラベルに設定
        if(testAttribute.bold)
        {
            EditorGUI.LabelField(position, testAttribute.Text, EditorStyles.boldLabel);
        }
        else { EditorGUI.LabelField(position, testAttribute.Text); }
    }
}

[CustomPropertyDrawer(typeof(TitleDescriptionAttribute))]
public class TitleDescriptionDrawer : PropertyDrawer
{
    GUISkin skin;

    //Inspector上の見た目を設定
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.Space(6);
        skin ??= Resources.Load<GUISkin>("タイトル");

        //TestAttributeを取得
        TitleDescriptionAttribute testAttribute = (TitleDescriptionAttribute)attribute;

        string text =  testAttribute.Text ?? property.stringValue;

        EditorGUILayout.LabelField(text, skin.label);
        EditorGUILayout.Space(5);
    }
}
#endif