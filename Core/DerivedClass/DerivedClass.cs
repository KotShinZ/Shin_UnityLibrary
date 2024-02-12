using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DerivedClass : IDisposable
{
    public Type type; //親クラス
    public string typeString;

    public int n; //選択のint表現

    public bool byString; //動かない時用

    public List<Type> list
    {
        get
        {
            if (allTypeList == null || allTypeList.Count == 0) { FirstGetList(); }
            return GetDerivedClass.instance.GetSelectedClassesByN(n, allTypeList); //選択しているクラスを取得;
        }
    }
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
    /// 初めてlistを取得しようとしたとき
    /// </summary>
    public void FirstGetList()
    {
        Init();
    }

    /// <summary>
    /// コンパイル時に選択を更新する
    /// </summary>
    public void Load()
    {
#if UNITY_EDITOR
        if (type == null) type = Utils.GetTypeFromString(typeString); //一応
        n = GetDerivedClass.instance.CreateNewSelectN(type, n); //選択を維持するために新しいnに更新する
        Init();
#endif
    }

    /// <summary>
    /// 選択を取得する
    /// </summary>
    public void Init()
    {
#if UNITY_EDITOR
        type = Utils.GetTypeFromString(typeString); //一応
        allClassList = GetDerivedClass.instance.LoadDerivedClassListString(type);//クラスの全リストを取得
#endif
        allTypeList = allClassList.CastList(t => Utils.GetTypeFromString(t));
    }

    /// <summary>
    /// コンパイル時に更新しないようにする
    /// </summary>
    public void Dispose()
    {
        Debug.Log("dispose");
        GetDerivedClass.instance.RemoveLoadLists(this);
    }
}