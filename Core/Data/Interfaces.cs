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
/// ƒAƒCƒeƒ€‚ğ‚Á‚Ä‚¢‚é–‚ğ•ÛØ‚·‚é
/// </summary>
public interface ItemHoldInterface
{
    public ItemDataContainer _items { get; }
}
