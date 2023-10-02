using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour, ItemHoldInterface
{
    [TitleDescription]
    public string title = "アイテムを持っていて送ることが出来る";

    public ItemDataContainer items = new();
    public HashSet<ItemData> __items  = new HashSet<ItemData>();
    public ItemDataContainer __itemsss = new();
    public ItemDataContainer _items { get => items;}

    public void Send(ItemHolder holder)
    {
        holder.items.Increase(_items);
    }
}
