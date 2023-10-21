using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public class GetDerivedClass : SingletonMonoBehaviour<GetDerivedClass>
{
    List<Type> types = new List<Type>(); 
    private Dictionary<Type, List<Type>> derivedTypes = new Dictionary<Type, List<Type>>(); //�e�N���X�Ƃ�����p������^�C�v�̈ꗗ
    private List<DerivedClass> derivedClassLoads = new List<DerivedClass>();

    /// <summary>
    /// �e�N���X����q�N���X�̈ꗗ��Ԃ�
    /// </summary>
    /// <param name="type"></param>
    public List<Type> LoadDerivedClassList(Type type)
    {
        if (derivedTypes.ContainsKey(type) == false)
        {
            types.Add(type);
            derivedTypes[type] = GetDerivedClasses(type);
        }
        return derivedTypes[type];
    }
    public List<string> LoadDerivedClassListString(Type type)
    {
        var d = LoadDerivedClassList(type);
        List<string> result = new List<string>();
        foreach (var c in d)
        {
            result.Add(c.Name);
        }
        return result;
    }

    /// <summary>
    /// �e�N���X�ƑI������V�����I�������
    /// </summary>
    /// <param name="type"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public int CreateNewSelectN(Type type, int n)
    {
        var preList = LoadDerivedClassList(type);
        var newList = GetDerivedClasses(type);

        if (newList.AreListsEqual(preList))
        {
            return n;
        }
        else
        {
            var newN = GetNewN(n, preList, newList);
            derivedTypes[type] = newList;
            return newN;
        }
    }

    /// <summary>
    /// �O�̑I�����玟�̑I�����X�V����
    /// </summary>
    /// <param name="preN"></param>
    /// <param name="preClassList"></param>
    /// <param name="newClassList"></param>
    /// <returns></returns>
    public int GetNewN(int preN, List<Type> preClassList, List<Type> newClassList)
    {
        var selected = GetSelectedClassesByN(preN, preClassList);
        return GetNClassesBySelected(selected, newClassList);
    }

    /// <summary>
    /// int����I������Ă���N���X��Ԃ�
    /// </summary>
    /// <param name="n">?I??????????N???X??????int??\????????</param>
    /// <param name="classList">?S???????i?I?????j</param>
    /// <returns></returns>
    public List<Type> GetSelectedClassesByN(int n, List<Type> classList)
    {
        var _list = new List<Type>();
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
    /// �I������Ă���ꗗ����int���擾����
    /// </summary>
    /// <param name="selectedList">?I??????????????</param>
    /// <param name="classList">?S???????i?I?????j</param>
    /// <returns></returns>
    public int GetNClassesBySelected(List<Type> selectedList, List<Type> classList)
    {
        var n = 0;
        foreach (var select in selectedList)
        {
            int i = classList.FindIndex(c => c == select);
            if (i != -1) { n += (1 << i); }
        }
        return n;
    }

    /// <summary>
    /// �p���N���X�̃��X�g��V������������
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns></returns>
    private List<Type> GetDerivedClasses(Type baseType)
    {
        List<Type> derivedClasses = new List<Type>();
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
                        derivedClasses.Add(type);
                    }
                }
            }
        }

        return derivedClasses;
    }
    /// <summary>
    /// �p���N���X�̃��X�g��String�ŕԂ�
    /// </summary>
    /// <param name="baseClassName"></param>
    /// <returns></returns>
    private List<string> GetDerivedClassesString(Type baseClassName)
    {
        var d = GetDerivedClasses(baseClassName);
        List<string> result = new List<string>();
        foreach (var c in d)
        {
            result.Add(c.Name);
        }
        return result;
    }

    #region �R���p�C�����Ƀ��[�h����
#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    public static void AllLoad()
    {
        instance.derivedClassLoads.ForEach(d => { if (d != null) d.Load(); });
    }
#endif

    /// <summary>
    /// �R���p�C������DerivedClass�̑I�����X�V����悤�ɂ���
    /// </summary>
    /// <param name="load"></param>
    public void AddLoadLists(DerivedClass load)
    {
        if (!derivedClassLoads.Contains(load)) derivedClassLoads.Add(load);
    }
    /// <summary>
    /// �R���p�C������DerivedClass�̑I�����X�V���Ȃ��悤�ɂ���
    /// </summary>
    /// <param name="load"></param>
    public void RemoveLoadLists(DerivedClass load)
    {
        if (derivedClassLoads.Contains(load)) derivedClassLoads.Remove(load);
    }
    /// <summary>
    /// LoadList��S�폜����
    /// </summary>
    public void DeleteLoadLists()
    {
        derivedClassLoads = new();
    }

    public int LoadListCount()
    {
        return derivedClassLoads.Count;
    }
    #endregion

    public static void GenerateThis()
    {
        // �q�G�����L�[��œ���̃R���|�[�l���g�������`�F�b�N
        if (FindFirstObjectByType<GetDerivedClass>() == false)
        {
            // �v���t�@�u����V�����I�u�W�F�N�g�𐶐�
            var generatedObject = new GameObject("DerivedClassObject");

            // ���������I�u�W�F�N�g�Ɏ����̃R���|�[�l���g���A�^�b�`
            generatedObject.AddComponent<GetDerivedClass>();
        }
    }
}
