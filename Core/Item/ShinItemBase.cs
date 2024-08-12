using Shin_UnityLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "ItemData", menuName = "Shin_UnityLibrary/ScriptableObjects/ItemBase")]
public class ShinItemBase : ShinGetItemsBase, ITypeEqaulObject, ICountableObject, IObservable<int>, IGetItems
{
    public string name = "";
    public BaseDataInt _num = new(0);
    public int num { get => _num.value; set => _num.value = value; }

    List<IGetItems> IGetItems.itemsProtected => new List<IGetItems> { this };

    [Label("canRepeat")] public bool canRepeat = false;

    public ShinItemBase(int n = 1) { _num.value = n; }
    public IDisposable Subscribe(IObserver<int> observer)
    {
        var n = new ShinItemBase(_num.value);
        return _num.Subscribe(observer);
    }

    /// <summary>
    /// item��Key�������I�u�W�F�N�g��T�^�Ŏ擾
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    /// <returns></returns>
    public T FindTarget<T>(IGetItems item, out IGetItems hit) where T : class
    {
        var target = item[name];
        hit = target;
        if (target != null)
        {
            var asObj = target as T;
            if (asObj != null) { return asObj; }
        }
        return null;
    }

    public virtual bool isDestroy()
    {
        return num < 0;
    }

    public virtual bool Increase(ICountableObject item) { num += 1;  return true; }
    public virtual bool Decrease(ICountableObject item) { num -= 1; return true; }

    /// <summary>
    /// �A�C�e���𑝂₷
    /// </summary>
    /// <param name="item">���₷�Ώۂ̃A�C�e��</param>
    /// <param name="isNewCreate">false</param>
    /// <param name="oneItem">ignore</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    IGetItems IGetItems.Increase(IGetItems item, bool isNewCreate, bool oneItem)
    {
        var target = FindTarget<ICountableObject>(item, out var hit);
        if(target != null)
        {
            Increase(target);
            return hit;
        }
        return null;
    }

    IGetItems IGetItems.Decrease(IGetItems item, bool canDestroy, bool oneItem)
    {
        var target = FindTarget<ICountableObject>(item, out var hit);
        if (target != null)
        {
            Decrease(target);
            return hit;
        }
        return null;
    }

    IGetItems IGetItems.GetItemFromName(string _name)
    {
        if (name != "" && name == _name)
        {
            return this;
        }
        return null;
    }
}