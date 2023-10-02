/*
 * Assets���̑SMaterial��Shader���ꊇ�Œu������Editor�g��
 * �g����
 * 1.Editor�f�B���N�g���z����Script��z�u
 * 2.Assets��ShaderReplace���ǉ������̂ŁA�����I�������Windows���J��
 * 3.Before�ɒu���O��Shader�AAfter�ɒu�����Shader��I��
 * 4.Replace�{�^���������ƑS����Material��Shader��u��
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SharderReplacer : EditorWindow
{
    private int beforeShaderNum = 1;
    private int selectBeforeShaderIndex;
    private int selectAfterShaderIndex;
    private string folderpath;
    private int[] beforeShaders = new int[100];

    [MenuItem("Assets/SharderReplace", false, 2000)]
    private static void Open()
    {
        GetWindow<SharderReplacer>();
    }

    private void OnGUI()
    {
        var sharders = ShaderUtil.GetAllShaderInfo();
        var sharderNames = sharders.Select(x => x.name).ToArray();

        folderpath = EditorGUILayout.TextField("Path",folderpath);
        EditorGUILayout.Space(15);
        beforeShaderNum = EditorGUILayout.IntField("BeforeShaderNum", beforeShaderNum);
        EditorGUILayout.Space(10);
        for ( int i = 0; i < beforeShaderNum; i++)
        {
            beforeShaders[i] = EditorGUILayout.Popup("Before" + i, beforeShaders[i], sharderNames);
        }
        EditorGUILayout.Space(10);
        selectAfterShaderIndex = EditorGUILayout.Popup("After", selectAfterShaderIndex, sharderNames);

        if (GUILayout.Button("Replace"))
        {
            List<string> selectBeforeShaders = new();
            for (int i = 0; i < beforeShaderNum; i++)
            {
                selectBeforeShaders.Add(sharderNames[beforeShaders[i]]);
            }
            ReplaceAll(selectBeforeShaders, sharderNames[selectAfterShaderIndex]);
        }
    }

    private void ReplaceAll(List<string> beforeShaderName, string afterShaderName)
    {
        List<Shader> shaders = new List<Shader>();
        foreach (string shaderName in beforeShaderName)
        {
            shaders.Add(Shader.Find(shaderName));
        }
        var afterShader = Shader.Find(afterShaderName);

        var guids = AssetDatabase.FindAssets("t: Material", new string[] { folderpath });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null && shaders.Contains(material.shader))
            {
                material.shader = afterShader;
            }

        }

        AssetDatabase.SaveAssets();
    }
}