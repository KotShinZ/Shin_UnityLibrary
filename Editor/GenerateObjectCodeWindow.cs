using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class GenerateObjectCodeWindow : EditorWindow
{
    private static string path = ""; // ���[�U�[����̓��͂�ێ�����ϐ�

    [MenuItem("Tool/Create/Generate")]
    public static void ShowWindow()
    {
        // �J�X�^���E�B���h�E��\��
        GenerateObjectCodeWindow window = GetWindow<GenerateObjectCodeWindow>("Generate"); // �^�C�g���o�[�ɕ\�������E�B���h�E�̃^�C�g��
        window.minSize = new Vector2(300, 100); // �E�B���h�E�̍ŏ��T�C�Y��ݒ�
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter Path:", EditorStyles.boldLabel);

        // �e�L�X�g���̓t�B�[���h��\��
        path = EditorGUILayout.TextField("Path", path);

        // �{�^����\�����A�N���b�N���̃A�N�V������ݒ�
        if (GUILayout.Button("GenerateObjectValue"))
        {
            GenerateStruct(path);
        }
    }

    private void GenerateStruct(string _path)
    {
        StringBuilder codeBuilder = new StringBuilder();

        codeBuilder.AppendLine("using System;");
        codeBuilder.AppendLine("using UnityEngine;");
        codeBuilder.AppendLine("[System.Serializable]");
        codeBuilder.AppendLine("public struct AnyObjectValue");
        codeBuilder.AppendLine("{");

        foreach (Type type in ObjectValueType.types)
        {
            string typeName = type.Name;
            string fieldName = typeName + "Value";

            codeBuilder.AppendLine($"    public {typeName} {fieldName};");
        }

        codeBuilder.AppendLine("}");

        codeBuilder.Append("public object GetValue(Type type)\r\n    {\r\n        FieldInfo[] fields = typeof(AnyObjectValue).GetFields(BindingFlags.Public | BindingFlags.Instance);\r\n\r\n        // �t�B�[���h�������X�g�ɒǉ�\r\n        foreach (FieldInfo field in fields)\r\n        {\r\n            if(field.FieldType == type) return field.GetValue(this);\r\n        }\r\n        return null;\r\n    }");

        // �R�[�h���t�@�C���ɏ�������
        var m_path = AbsoluteToAssetsPath(path);
        File.WriteAllText(m_path, codeBuilder.ToString());



        // Unity�̃R���p�C���ɒʒm���āA�V�����R�[�h���v���W�F�N�g�Ɏ�荞��
        UnityEditor.AssetDatabase.Refresh();

        Debug.Log($"AnyObjectValue struct generated at {_path}");
    }

    public static string AbsoluteToAssetsPath(string self)
    {
        return self.Replace("\\", "/").Replace(Application.dataPath, "Assets");
    }
}

