using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// �e�L�X�g�����\������BBold�ɂ��邱�Ƃ��o����B
/// </summary>
public class TextDescriptionAttribute : PropertyAttribute
{

    //�錾���ɓn���ꂽ������
    public string Text;
    public bool bold;

    /// <summary>
    /// �R���X�g���N�^(�����ŕ������n��)
    /// </summary>
    public TextDescriptionAttribute(string text, bool bold = false)
    {
        Text = text;
        this.bold = bold;
    }
}

public class TitleDescriptionAttribute : PropertyAttribute
{
    //�錾���ɓn���ꂽ������
    public string Text;
    public bool bold = false;

    /// <summary>
    /// �R���X�g���N�^(�����ŕ������n��)
    /// </summary>
    public TitleDescriptionAttribute() { }
    public TitleDescriptionAttribute(string text)
    {
        Text = text;
    }
}

#if UNITY_EDITOR
//CustomPropertyDrawer��TestAttribute���w��(TestAttribute��Inspector��̌����ڂ�ύX���邽��)
[CustomPropertyDrawer(typeof(TextDescriptionAttribute))]
public class TextDescriptionDrawer : PropertyDrawer
{
    

    //Inspector��̌����ڂ�ݒ�
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        

        //TestAttribute���擾
        TextDescriptionAttribute testAttribute = (TextDescriptionAttribute)attribute;

        //TestAttribute��Text�ɐݒ肳��Ă��镶��������x���ɐݒ�
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

    //Inspector��̌����ڂ�ݒ�
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.Space(6);
        skin ??= Resources.Load<GUISkin>("�^�C�g��");

        //TestAttribute���擾
        TitleDescriptionAttribute testAttribute = (TitleDescriptionAttribute)attribute;

        string text =  testAttribute.Text ?? property.stringValue;

        EditorGUILayout.LabelField(text, skin.label);
        EditorGUILayout.Space(5);
    }
}
#endif