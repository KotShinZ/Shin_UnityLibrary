using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

/// <summary>
/// �V�[�������Ǘ����� Enum ���쐬����G�f�B�^�[�g��
/// 
/// �� ������ �� �����ς����Ă������������̂ł�
/// https://gist.github.com/baba-s/9286120#file-scenenamecreator-cs
/// </summary>
public static class SceneNameEnumCreator
{
    // �����ȕ������Ǘ�����z��
    private static readonly string[] INVALUD_CHARS =
    {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        "<", ",",
    };

    // �R�}���h��
    private const string ITEM_NAME = "Tools/Shin_UnityLibrary/Create Scene Name Enum";

    // �t�@�C���p�X
    public static string PATH = "Assets/Shin_UnityLibrary/Core/Shin_UnityLibrary_Data/SceneNameEnum.cs";

    // �t�@�C����(�g���q����)
    private static string FILENAME => Path.GetFileName(PATH);

    // �t�@�C����(�g���q�Ȃ�)
    private static string FILENAME_WITHOUT_EXTENSION
        => Path.GetFileNameWithoutExtension(PATH);

    /// <summary>
    /// �V�[�������Ǘ����� Enum ���쐬���܂�
    /// </summary>
    public static void Create(bool showLog = true)
    {
        if (!CanCreate()) return;

        CreateScript();

        if(showLog) EditorUtility.DisplayDialog(FILENAME, "�쐬���������܂���", "OK");
    }

    /// <summary>
    /// �X�N���v�g���쐬���܂�
    /// </summary>
    public static void CreateScript()
    {
        var builder = new StringBuilder();

        builder
            .AppendLine("/// <summary>")
            .AppendLine("/// �V�[�������Ǘ����� Enum")
            .AppendLine("/// </summary>")
            .AppendLine($"public enum {FILENAME_WITHOUT_EXTENSION}")
            .AppendLine("{")
            .AppendLine("None,");
        foreach (var n in EditorBuildSettings.scenes
            .Select(c => Path.GetFileNameWithoutExtension(c.path))
            .Distinct()
            .Select(c => new { var = RemoveInvalidChars(c), val = c }))
        {
            builder.AppendLine($"\t{n.var},");
        }

        builder.AppendLine("}");

        var directoryName = Path.GetDirectoryName(PATH);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        File.WriteAllText(PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    /// <summary>
    /// �V�[�������Ǘ����� Enum ���쐬�ł��邩�ǂ������擾���܂�
    /// </summary>
    [MenuItem(ITEM_NAME, true)]
    public static bool CanCreate()
        => !EditorApplication.isPlaying
            && !Application.isPlaying
            && !EditorApplication.isCompiling;

    /// <summary>
    /// �����ȕ������폜���܂�
    /// </summary>
    public static string RemoveInvalidChars(string str)
    {
        Array.ForEach(INVALUD_CHARS, c => str = str.Replace(c, string.Empty));
        return str;
    }
}