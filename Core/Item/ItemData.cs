using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// �^�C�v�������œ����I�u�W�F�N�g�Ƃ݂Ȃ��B
/// �����邱�Ƃ��o���A���₵���茸�炵����o����B
/// </summary>
[System.Serializable]
public class ItemData : TypeEqaulObject, ICountableObject
{
    public int num;
    int ICountableObject._num { get => num; set => num = value; }

    public ItemData(int n = 1) { num = n; }
}

[System.Serializable]
public class ItemDataContainer : HashSetCountable<ItemData>
{
}
