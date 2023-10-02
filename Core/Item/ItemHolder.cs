using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour, ItemHoldInterface
{
    [TitleDescription]
    public string title = "�A�C�e���������Ă��đ��邱�Ƃ��o����";

    public ItemDataContainer items = new();
    public HashSet<ItemData> __items  = new HashSet<ItemData>();
    public ItemDataContainer __itemsss = new();
    public ItemDataContainer _items { get => items;}

    public void Send(ItemHolder holder)
    {
        holder.items.Increase(_items);
    }
}
