using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class EnumGeneratorWindow : EditorWindow
{
    private class EnumEntry
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public EnumEntry(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }

    private List<EnumEntry> enumEntries = new List<EnumEntry>
    {
        new EnumEntry("Example1", 0),
        new EnumEntry("Example2", 1)
    };
    private string enumName = "ExampleEnum";
    private string savePath = "Assets/Shin_UnityLibrary/Core/Shin_UnityLibrary_Data/";

    [MenuItem("Tools/Shin_UnityLibrary/Create Enum")]
    public static void ShowWindow()
    {
        GetWindow<EnumGeneratorWindow>("Enum Generator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Enum Name:", EditorStyles.boldLabel);
        enumName = EditorGUILayout.TextField(enumName);

        if (GUILayout.Button("Load Existing Enum"))
        {
            LoadExistingEnum();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Enum Entries:", EditorStyles.boldLabel);

        for (int i = 0; i < enumEntries.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            enumEntries[i].Name = EditorGUILayout.TextField(enumEntries[i].Name);
            enumEntries[i].Value = EditorGUILayout.IntField(enumEntries[i].Value);

            if (GUILayout.Button("-"))
            {
                enumEntries.RemoveAt(i);
                i--;
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add New Entry"))
        {
            enumEntries.Add(new EnumEntry("NewEntry", enumEntries.Count > 0 ? enumEntries[enumEntries.Count - 1].Value + 1 : 0));
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Save Path:", EditorStyles.boldLabel);
        savePath = EditorGUILayout.TextField(savePath);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Enum"))
        {
            GenerateEnum();
        }
    }

    private void GenerateEnum()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"public enum {enumName}");
        sb.AppendLine("{");
        foreach (var entry in enumEntries)
        {
            sb.AppendLine($"    {entry.Name} = {entry.Value},");
        }
        sb.AppendLine("}");

        string fullPath = $"{savePath}{enumName}.cs";
        File.WriteAllText(fullPath, sb.ToString());
        AssetDatabase.Refresh();
    }

    private void LoadExistingEnum()
    {
        string fullPath = $"{savePath}{enumName}.cs";
        if (File.Exists(fullPath))
        {
            string fileContent = File.ReadAllText(fullPath);
            var matches = Regex.Matches(fileContent, @"^\s*(\w+)\s*=\s*(\d+),", RegexOptions.Multiline);

            enumEntries.Clear();
            foreach (Match match in matches)
            {
                string entryName = match.Groups[1].Value;
                int entryValue = int.Parse(match.Groups[2].Value);
                enumEntries.Add(new EnumEntry(entryName, entryValue));
            }
        }
    }
}