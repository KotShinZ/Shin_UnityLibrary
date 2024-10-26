using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneNameEnumWindow : EditorWindow
{
    public static string defaultPath = "Assets/Shin_UnityLibrary/Core/Shin_UnityLibrary_Data/SceneNameEnum.cs";
    public string folderPath = "Assets/Shin_UnityLibrary/Core/Shin_UnityLibrary_Data/SceneNameEnum.cs";

    [MenuItem("Tools/Shin_UnityLibrary/SceneNameEnumWindow")]
    public static void ShowWindow()
    {
        GetWindow<SceneNameEnumWindow>("SceneNameEnumWindow");
    }

    [InitializeOnLoadMethod]
    static void InitCreate()
    {
        SceneNameEnumCreator.PATH = defaultPath;
        SceneNameEnumCreator.Create(false);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Scene Name Enum Creator");
        EditorGUILayout.LabelField("This create Enum from BuildSettings");
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);

        if (GUILayout.Button("Generate"))
        {
            SceneNameEnumCreator.PATH = folderPath;
            SceneNameEnumCreator.Create();
        }
    }
}
