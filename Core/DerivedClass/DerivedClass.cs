using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DerivedClass : IDisposable
{
    public Type type; //�e�N���X
    public string typeString;

    public int n; //�I����int�\��

    public List<Type> list { get { if (_list == null) FirstGetList(); return _list; } }
    public List<Type> _list;
    public List<string> allClassList;
    public List<Type> allTypeList;

    public List<string> noContentWords;

    public DerivedClass(Type type, List<string> _noContentWords = default, Component addToObject = null)
    {
        GetDerivedClass.GenerateThis();

        this.type = type;
        n = 0;
        typeString = type.FullName;
        noContentWords = _noContentWords;
        Init();

        /*if(addToObject != null) {
            addToObject.OnDestroyAsObservable().Subscribe(_ =>
            {
                Dispose();
                Debug.Log("dispose");
            });
        }*/

#if UNITY_EDITOR
        GetDerivedClass.instance.AddLoadLists(this);
#endif
    }

    /// <summary>
    /// ���߂�list���擾���悤�Ƃ����Ƃ�
    /// </summary>
    public void FirstGetList()
    {
        Init();
    }

    /// <summary>
    /// �R���p�C�����ɑI�����X�V����
    /// </summary>
    public void Load()
    {
#if UNITY_EDITOR
        if (type == null) type = Utils.GetTypeFromString(typeString); //�ꉞ
        n = GetDerivedClass.instance.CreateNewSelectN(type, n); //�I�����ێ����邽�߂ɐV����n�ɍX�V����
        Init();
#endif
    }

    /// <summary>
    /// �I�����擾����
    /// </summary>
    public void Init()
    {
#if UNITY_EDITOR
        if (type == null) type = Utils.GetTypeFromString(typeString); //�ꉞ
        if (allClassList == null) allClassList = GetDerivedClass.instance.LoadDerivedClassListString(type);//�N���X�̑S���X�g���擾
#endif
        if (allTypeList == null) allTypeList = allClassList.CastList(t => Utils.GetTypeFromString(t));

        _list = GetDerivedClass.instance.GetSelectedClassesByN(n, allTypeList); //�I�����Ă���N���X���擾
    }

    /// <summary>
    /// �R���p�C�����ɍX�V���Ȃ��悤�ɂ���
    /// </summary>
    public void Dispose()
    {
        Debug.Log("dispose");
        GetDerivedClass.instance.RemoveLoadLists(this);
    }
}