#define Shin_UnityLibrary_CineMachiine
#define Shin_UnityLibrary_InputAction
#define Shin_UnityLibrary_ModularMotion
#define Shin_UnityLibrary_SelectionGroup
#define Shin_UnityLibrary_SimpleAnimation
#define Shin_UnityLibrary_Splines
#define Shin_UnityLibrary_TimeLine
#define Shin_UnityLibrary_VisualCompositor

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor.Build;
using Shin_UnityLibrary;
using System.ComponentModel;

public class OverideDefineWindow : EditorWindow
{
    public static Dictionary<string, bool> dict = new() {
        { "CineMachiine", false },
        { "InputAction", false },
        { "ModularMotion", false },
        { "SelectionGroup", false },
        { "Splines", false },
        { "TimeLine", false },
        { "VisualCompositor", false },
    };
    static Dictionary<string, bool> defoltDict = new() {
        { "CineMachiine", false },
        { "InputAction", false },
        { "ModularMotion", false },
        { "SelectionGroup", false },
        { "Splines", false },
        { "TimeLine", false },
        { "VisualCompositor", false },
    };

    [MenuItem("Tools/Shin_UnityLibrary/Welcome")]
    static void Open()
    {
        var window = GetWindow<OverideDefineWindow>();
        window.titleContent = new GUIContent("Welcome");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Welcome!!");
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("�T�|�[�g����O���A�Z�b�g��I�����Ă�������");
        EditorGUILayout.Space(10);

        var tmpDict = new Dictionary<string, bool>(dict);
        foreach (var pair in tmpDict)
        {
            var key = pair.Key;
            dict[key] = EditorGUILayout.Toggle(pair.Key, dict[key]);
        }

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Suppot"))
        {
            GenerateDefines();
        }
    }

    public void GenerateDefines()
    {
        var target = EditorUserBuildSettings.selectedBuildTargetGroup;
        var prestr = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
        var listStr = prestr.Split(';').ToHashSet();

        foreach (var dictStr in dict)
        {
            if (dictStr.Value) //�ǉ�����
            {
                listStr.Add("Shin_UnityLibrary_" + dictStr.Key);
            }
            else
            {
                listStr.Remove("Shin_UnityLibrary_" + dictStr.Key);
            }
        }
        var defines = string.Join(";", listStr);

        //dict.ForEach(d => Debug.Log(d.Key));
        Utils.SaveJsonDictionary(dict, "Shin_UnityLibrary_Data", "OverideDefineData");
        if (defines != prestr) PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defines);
    }

    public static void ResetData()
    {
        dict = defoltDict;
    }


    bool isContainStr(IEnumerable<string> list, string key)
    {
        foreach (var s in list)
        {
            if (s == key)
            {
                return true;
            }
        }
        return false;
    }
}

[InitializeOnLoad]
public class LoadOverideDatas
{
    static LoadOverideDatas()
    {
        var dict = Utils.LoadJsonDictionary<string, bool>("Shin_UnityLibrary_Data", "OverideDefineData");
        if (dict != null && dict != OverideDefineWindow.dict) OverideDefineWindow.dict = dict;
    }
}
