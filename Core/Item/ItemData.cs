using Shin_UnityLibrary;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// タイプが同じで同じオブジェクトとみなす。
/// 数えることが出来、増やしたり減らしたり出来る。
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
