using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;
using Shin_UnityLibrary;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

[System.Serializable]
public struct DerivedClass : IDerivedClassLoad
{
    public Type type; //�x�[�X�N���X
    public string typeString;
    public int n; //�I������Ă���N���X������int�\��
    public List<string> list
    {
        get 
        {
#if UNITY_EDITOR
            if (loaded) return savedList;

            if (savedList == null || savedList.Count == 0)
            {
                savedList = GetList();
                loaded = true;
            }
            return savedList;
#else
            return savedList;
#endif
        }
    }
    private List<string> savedList;
    private bool loaded;

    public List<string> noContentWords;

    public DerivedClass(Type type, List<string> _noContentWords= default)
    {
        this.type = type;
        n = 0;
        typeString = type.FullName;
        noContentWords = _noContentWords;
        savedList = new List<string>();
        loaded = false;

#if UNITY_EDITOR
        GetDerivedClass.AddIDerivedClassLoadLists(this);
#endif
    }

    private List<string> GetList()
    {
#if UNITY_EDITOR
        if (type == null) type = Utils.GetTypeFromString(typeString);
        GetDerivedClass.instance.LoadDerivedClassList(type);//�O�̂��߃^�C�v�̑S�̃��X�g���쐬�A�X�V
        return GetDerivedClass.instance.GetSelectedClassesByN(n, GetDerivedClass.instance.GetDerivedList(type)); //�I������Ă���N���X���擾
#else
        return null;
#endif
    }

    public void Load()
    {
#if UNITY_EDITOR
        GetDerivedClass.instance.LoadDerivedClassList(type, n);
#endif
    }
}

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

public interface IDerivedClassLoad { void Load(); }

#if UNITY_EDITOR

/// <summary>
/// �S�x�[�X�N���X�ƁA�e�x�[�X�N���X�̎q�N���X��ۑ����Ă���
/// </summary>
public class GetDerivedClass : ScriptableSingleton<GetDerivedClass>
{
    List<Type> types = new List<Type>(); //�x�[�X�N���X����
    private Dictionary<Type, List<string>> strings = new Dictionary<Type, List<string>>(); //�e�N���X�ɂ������āA���̃N���X����p������Ă���N���X�𕶎���ŕۑ����Ă���
    private static List<IDerivedClassLoad> derivedClassLoads = new List<IDerivedClassLoad>();

    public static void AddIDerivedClassLoadLists(IDerivedClassLoad load)
    {
        if(!derivedClassLoads.Equals(load)) derivedClassLoads.Add(load);
    }

    /// <summary>
    /// �x�[�X�N���X����V�����h���N���X�̃��X�g���擾���A�f�B�N�V���i���ɕۑ�
    /// </summary>
    /// <param name="type"></param>
    public void LoadDerivedClassList(Type type)
    {
        if(strings.ContainsKey(type) == false) //�V�K�x�[�X�N���X�̏ꍇ
        {
            types.Add(type);
        }
        strings[type] = GetDerivedClasses(type);
    }

    /// <summary>
    /// �x�[�X�N���X����V�����h���N���X�̃��X�g���擾���A�f�B�N�V���i���ɕۑ��B�O���int�\������V����int�\���ɕύX���邱�ƂŁA�I����s�ς�
    /// </summary>
    /// <param name="type"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public int LoadDerivedClassList(Type type, int n)
    {
        if (strings.ContainsKey(type) == false) //�V�K�x�[�X�N���X�̏ꍇ
        {
            types.Add(type);
            strings[type] = GetDerivedClasses(type);
        }

        var newList = GetDerivedClasses(type);
        var preList = strings[type];

        if(newList.AreListsEqual(preList)) return n;

        var newN = GetNewN(n, preList, newList);
        strings[type] = newList;
        return newN;
    }

    /// <summary>
    /// �O�̃��X�g�ƐV�������X�g����int�\�����X�V����B
    /// </summary>
    /// <param name="preN"></param>
    /// <param name="preClassList"></param>
    /// <param name="newClassList"></param>
    /// <returns></returns>
    public int GetNewN(int preN, List<string> preClassList, List<string> newClassList)
    {
        var selected = GetSelectedClassesByN(preN, preClassList);
        return GetNClassesBySelected(selected, newClassList);
    }

    /// <summary>
    /// int�ƑS�̕����񂩂炩��I������Ă���N���X�𕶎���̃��X�g�ŕԂ�
    /// </summary>
    /// <param name="n">�I������Ă���N���X������int�ŕ\��������</param>
    /// <param name="classList">�S�̕�����i�I�����j</param>
    /// <returns></returns>
    public List<string> GetSelectedClassesByN(int n , List<string> classList)
    {
        var _list = new List<string>();
        for (int i = 0; i < classList.Count; i++)
        {
            if ((n & 1 << i) != 0)
            {
                _list.Add(classList[i]);
            }
        }
        return _list;
    }

    /// <summary>
    /// �I������Ă��镶����ƑS�̕����񂩂�A�I������Ă�����̂�int�\����Ԃ�
    /// </summary>
    /// <param name="selectedList">�I������Ă��镶����</param>
    /// <param name="classList">�S�̕�����i�I�����j</param>
    /// <returns></returns>
    public int GetNClassesBySelected(List<string> selectedList, List<string> classList)
    {
        var n = 0;
        foreach(var select in selectedList)
        {
            int i = classList.FindIndex(c => c == select);
            if (i != -1) { n += (1 << i); }
        }
        return n;
    }

    /// <summary>
    /// �x�[�X�^�C�v���p�����Ă���N���X�̃��X�g�𕶎���ŕԂ�
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns></returns>
    private List<string> GetDerivedClasses(Type baseType)
    {
        List<string> derivedClasses = new List<string>();
        if (baseType != null)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsSubclassOf(baseType) && !Regex.IsMatch(type.Name, @"^(Pre|End|No)") && !type.IsAbstract)
                    {
                        derivedClasses.Add(type.Name);
                    }
                }
            }
        }

        return derivedClasses;
    }
    private List<string> GetDerivedClasses(string baseClassName)
    {
        Type baseType = Type.GetType(baseClassName);
        return GetDerivedClasses(baseType);
    }

    public List<string> GetDerivedList(Type baseType)
    {
        if (!types.Contains(baseType)) LoadDerivedClassList(baseType);
        return strings[baseType];
    }

    [DidReloadScripts]
    public static void AllLoad()
    {
        derivedClassLoads.ForEach(d => { if(d != null) d.Load(); });
    }

    /*[InitializeOnLoadMethod]
    public static void CleanList()
    {

    }*/

}

/// <summary>
/// DerivedClass��p�����A����N���X���p�����Ă���N���X�̒����畡���I���o����B
/// </summary>
[CustomPropertyDrawer(typeof(SelectDerivedClassAttribute))]
public class SelectDerivedClassDrawer : PropertyDrawer
{
    int n = 0;
    SelectDerivedClassAttribute derivedClass { get { return (SelectDerivedClassAttribute)attribute; } }
    bool inited = false;

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);
        EditorGUILayout.BeginVertical(GUI.skin.box);

        if(derivedClass.tytle  != null) { EditorGUILayout.LabelField(derivedClass.tytle); }

        SerializedProperty nProp = prop.FindPropertyRelative("n");
        SerializedProperty typeProp = prop.FindPropertyRelative("typeString");

        if (nProp != null && typeProp != null)
        {
            //���݂̑I�����擾
            n = nProp.intValue;

            //�^�C�v���擾
            derivedClass.typeString = typeProp.stringValue;
            Type type = derivedClass.type;

            //�p�����X�g���擾
            var list = GetDerivedClass.instance.GetDerivedList(type);

            if (list != null && list.Count != 0)
            {
                n = EditorGUILayout.MaskField(n, list.ToArray());
            }
            nProp.intValue = n;
        }

        EditorGUILayout.EndVertical();
        EditorGUI.EndProperty();
        EditorGUILayout.Space(10);
    }
}

#endif