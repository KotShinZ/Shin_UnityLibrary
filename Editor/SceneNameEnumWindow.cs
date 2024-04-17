using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneNameEnumWindow : EditorWindow
{
    public string folderPath = "Assets/Shin_UnityLibrary/Core/Shin_UnityLibrary_Data/SceneNameEnum.cs";

    [MenuItem("Tools/Shin_UnityLibrary/SceneNameEnumWindow")]
    public static void ShowWindow()
    {
        GetWindow<SceneNameEnumWindow>("SceneNameEnumWindow");
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
