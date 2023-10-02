using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICountableObject
{
    protected int _num { get; set; }

    public void Increase(ICountableObject item)
    {
        _num += 1;
    }
    public void Decrease(ICountableObject item)
    {
        _num -= 1;
    }
}

/// <summary>
/// �A�C�e���������Ă��鎖��ۏ؂���
/// </summary>
public interface ItemHoldInterface
{
    public ItemDataContainer _items { get; }
}
