using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class GenerateObjectCodeWindow : EditorWindow
{
    private static string path = ""; // ユーザーからの入力を保持する変数

    [MenuItem("Tool/Create/Generate")]
    public static void ShowWindow()
    {
        // カスタムウィンドウを表示
        GenerateObjectCodeWindow window = GetWindow<GenerateObjectCodeWindow>("Generate"); // タイトルバーに表示されるウィンドウのタイトル
        window.minSize = new Vector2(300, 100); // ウィンドウの最小サイズを設定
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter Path:", EditorStyles.boldLabel);

        // テキスト入力フィールドを表示
        path = EditorGUILayout.TextField("Path", path);

        // ボタンを表示し、クリック時のアクションを設定
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

        codeBuilder.Append("public object GetValue(Type type)\r\n    {\r\n        FieldInfo[] fields = typeof(AnyObjectValue).GetFields(BindingFlags.Public | BindingFlags.Instance);\r\n\r\n        // フィールド名をリストに追加\r\n        foreach (FieldInfo field in fields)\r\n        {\r\n            if(field.FieldType == type) return field.GetValue(this);\r\n        }\r\n        return null;\r\n    }");

        // コードをファイルに書き込む
        var m_path = AbsoluteToAssetsPath(path);
        File.WriteAllText(m_path, codeBuilder.ToString());



        // Unityのコンパイラに通知して、新しいコードをプロジェクトに取り込む
        UnityEditor.AssetDatabase.Refresh();

        Debug.Log($"AnyObjectValue struct generated at {_path}");
    }

    public static string AbsoluteToAssetsPath(string self)
    {
        return self.Replace("\\", "/").Replace(Application.dataPath, "Assets");
    }
}

