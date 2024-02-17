using UniRx;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using Shin_UnityLibrary;
using UniRx.Triggers;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

public class SelectDerivedClassAttribute : PropertyAttribute
{
    public string typeString;
    public Type type
    {
        get { if (m_Type == null || m_Type.Name != typeString) m_Type = LoadType(typeString); return m_Type; }
    }
    Type m_Type;
    public string tytle;

    public SelectDerivedClassAttribute(string tytle = null)
    {
        this.tytle = tytle;
    }

    Type LoadType(string typeName)
    {
        Type type = Assembly.Load("Assembly-CSharp").GetType(typeName);
        if (type == null) type = Assembly.Load("Shin_UnityLibrary_StateMachine").GetType(typeName);
        return type;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(SelectDerivedClassAttribute))]
public class SelectDerivedClassDrawer : PropertyDrawer
{
    int pren = 0;
    int n = 0;
    SelectDerivedClassAttribute derivedClass { get { return (SelectDerivedClassAttribute)attribute; } }
    bool inited = false;

    bool fold = false;

    /// <summary>
    /// 選択に変更があった時
    /// </summary>
    public void SelectValidate()
    {

    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);
        EditorGUILayout.BeginVertical(GUI.skin.box);

        if (derivedClass.tytle != null) { EditorGUILayout.LabelField(derivedClass.tytle); }

        SerializedProperty nProp = prop.FindPropertyRelative("n");
        SerializedProperty typeProp = prop.FindPropertyRelative("typeString");
        SerializedProperty savedListProp = prop.FindPropertyRelative("allClassList");

        if (nProp != null && typeProp != null)
        {
            //���݂̑I�����擾
            n = nProp.intValue;
            pren = nProp.intValue;

            //�^�C�v���擾
            derivedClass.typeString = typeProp.stringValue;
            Type type = derivedClass.type;

            //�p�����X�g���擾
            var list = GetDerivedClass.instance.LoadDerivedClassListString(type);

            if (list != null && list.Count() != 0)
            {
                n = EditorGUILayout.MaskField(n, list.ToArray());
                if (pren != 0) SelectValidate();
                pren = n;
            }
            nProp.intValue = n;
        }

        #region Advance
        EditorGUI.indentLevel++;
        fold = EditorGUILayout.Foldout(fold, "Advance");
        if (fold)
        {
            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(true);

            if (savedListProp != null) EditorGUILayout.PropertyField(savedListProp);
            EditorGUILayout.IntField(GetDerivedClass.instance.LoadListCount());

            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("AddLoadList"))
            {
                var type = Utils.GetTypeFromString(typeProp.stringValue); //一応
                var list = GetDerivedClass.instance.LoadDerivedClassListString(type);//クラスの全リストを取得

                savedListProp.ClearArray();

                for(int i = 0; i < list.Count(); i++)
                {
                    // 必要であれば新しい要素を追加
                    savedListProp.arraySize += 1; // 新しい要素のスペースを追加
                    SerializedProperty newElement = savedListProp.GetArrayElementAtIndex(i);
                    newElement.stringValue = list[i];
                }
            }
            if (GUILayout.Button("DeleteLoadList")) GetDerivedClass.instance.DeleteLoadLists();
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
        #endregion

        EditorGUILayout.EndVertical();
        EditorGUI.EndProperty();
        EditorGUILayout.Space(10);
    }


    /*public void Init(string typeString, )
    {
#if UNITY_EDITOR
        var type = Utils.GetTypeFromString(typeString); //一応
        if (allClassList == null) allClassList = GetDerivedClass.instance.LoadDerivedClassListString(type);//クラスの全リストを取得
#endif
        if (allTypeList == null) allTypeList = allClassList.CastList(t => Utils.GetTypeFromString(t));

        _list = GetDerivedClass.instance.GetSelectedClassesByN(n, allTypeList); //選択しているクラスを取得
    }*/
}

#endif